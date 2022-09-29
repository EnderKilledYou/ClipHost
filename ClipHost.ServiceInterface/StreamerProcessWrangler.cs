using BlazorQueue;
using ClipHost.ServiceModel.Types;
using Microsoft.AspNetCore.SignalR;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClipHost;

public class StreamerProcessWrangler : ProcessWranglerBase<DtoProgramInstance>
{
    private readonly IHubContext<ClipHub, IClipStreams> _hubContext;
    private readonly CommandCenter commandCenter;

    public DtoProgramInstance[] Instances { get => dtoProgramInstances.Where(a => a.DtoId.HasValue).ToArray(); }

    public StreamerProcessWrangler(IHubContext<ClipHub, IClipStreams> hubContext, CommandCenter commandCenter) :
        base(HostContext.AppSettings, commandCenter.MaxStreamers)
    {
        _hubContext = hubContext;
        this.commandCenter = commandCenter;
    }

    /// <summary>
    /// Converts the list of processes to a list of status reports.
    /// </summary>
    /// <param name="items"></param>
    public virtual void Report(List<DToProgramInstanceReport> items)

    {
        items.AddRange(dtoProgramInstances.Select(item => item.ToReport()));
    }
    protected async Task RetrieveAssignedStreams(List<(Streamer streamer, TwitchOauthTokens tokens, int twitchStreamId)> streamersOut)
    {
        var ConsumerSecret = settings.Get<string>("Twitch:Secret");
        using var db = await HostContext.Resolve<IDbConnectionFactory>().OpenAsync();
        var streamers = db.From<StreamerCommandCenter>().Join<StreamerCommandCenter, Streamer>((streamerCommandCenterTable, streamerTable) => streamerCommandCenterTable.StreamerId == streamerTable.Id).Join<StreamerCommandCenter, CommandCenter>((streamerCommandCenterTable, commandCenter) => streamerCommandCenterTable.CommandCenterId == commandCenter.Id)
            .Join<Streamer, TwitchOauthTokens>((a, b) => a.Name == b.UserName)
            .Where<Streamer>(a => a.Enabled)
            .Where<StreamerCommandCenter>(a => a.CommandCenterId == commandCenter.Id);


        var results = await db.SelectMultiAsync<StreamerCommandCenter, Streamer, TwitchOauthTokens>(streamers);
        foreach (var result in results)
        {
            (StreamerCommandCenter scc, Streamer streamer, TwitchOauthTokens tokens) = result;
            if (dtoProgramInstances.Any(a => a.DtoId == streamer.Id))
            {
                continue;
            }
            TwitchLib.Api.Helix.Models.Streams.GetStreams.Stream streamIsLive = await getLiveStream(streamer, tokens);

            var streamRecord = db.Single<TwitchStream>(a => a.StreamId == streamIsLive.Id);
            if (streamRecord == null)
            {
                streamRecord = new TwitchStream()
                {
                    StreamerId = streamer.Id,
                    StreamId = streamIsLive.Id
                };
                streamRecord.Id = (int)db.Insert(streamRecord, true);
            }
            if (streamIsLive != null)
            {
                streamersOut.Add((streamer, tokens, streamRecord.Id));
            }

        }

    }
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        if (HostUrl == null)
        {
            throw new ArgumentException("No host setting, set ClipHuntaProcessPath in app settings ");
        }
        if (string.IsNullOrEmpty(ClipHuntaProcessPath))
        {
            throw new ArgumentException("No process setting, set ClipHuntaProcessPath in app settings ");

        }
        await base.StartAsync(cancellationToken);
        await EmptyPrograms();
        StartPrograms();
    }

    public override void Dispose()
    {
        foreach (var instance in dtoProgramInstances)
        {
            if (!instance.Process().HasExited)
                instance.Process().Kill();
            instance.ProcessDispose();

        }
        base.Dispose();
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {

        await base.StopAsync(cancellationToken);
    }
    private async Task<TwitchLib.Api.Helix.Models.Streams.GetStreams.Stream> getLiveStream(Streamer streamer, TwitchOauthTokens tokens)
    {
        try
        {
            //todo: add proccess that removes deauthorized accounts
            var api = await settings.GetRefreshedApi(tokens.AccessToken, tokens.RefreshToken);
            var streamIsLive = await api.GetLiveStreamAsync(streamer.Name);
            return streamIsLive;
        }
        catch (Exception ex)
        {

            return null;
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        List<(Streamer, TwitchOauthTokens, int)> streamersOut = new();

        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckWork(streamersOut, stoppingToken);
            streamersOut.Clear();
            CleanDeadProcesses();
            StartPrograms();
            RecordErrors();

            await Task.Delay(5000, stoppingToken).ConfigureAwait(false);
        }
    }

    private void RecordErrors()
    {
        var count = streamErrorQueue.Count;
        var count2 = clipErrorQueue.Count;
        if (count + count2 == 0)
        {
            return;
        }
        using var Db = HostContext.Resolve<IDbConnectionFactory>().Open();
        for (int i = 0; i < count; i++)
        {
            if (streamErrorQueue.TryDequeue(out var error))
            {
                Db.Insert(new TwitchStreamError()
                {
                    Context = error.twitchStreamId.ToString(),
                    Location = error.processId.ToString(),
                    Message = error.errorMessage,
                    StackTrace = error.stackTrace,
                    TwitchStreamId = error.twitchStreamId
                });
            }
        }
        for (int i = 0; i < count2; i++)
        {
            if (clipErrorQueue.TryDequeue(out var error))
            {
                Db.Insert(new TwitchClipError()
                {
                    Context = error.twitchClipId.ToString(),
                    Location = error.processId.ToString(),
                    Message = error.errorMessage,
                    StackTrace = error.stackTrace,
                    TwitchClipId = error.twitchClipId
                });
            }
        }
    }

    protected override void CleanDeadProcesses()
    {
        using var Db = HostContext.Resolve<IDbConnectionFactory>().Open();
        for (var i = dtoProgramInstances.Count - 1; i >= 0; i--)
        {
            var dtoProgramInstance = dtoProgramInstances[i];
            if (!dtoProgramInstance.ProcessExited()) continue;
            dtoProgramInstances.RemoveAt(i);

            var item = Db.Single<ProcessReport>(a => a.ProcessId == dtoProgramInstance.ProcessId());
            if (item != null)
            {
                item.ExitCode = dtoProgramInstance.Process().ExitCode;
                item.IsRunning = false;
            }
            dtoProgramInstance.ProcessDispose();
        }
    }
    private async Task EmptyPrograms()
    {
        using var Db = await HostContext.Resolve<IDbConnectionFactory>().OpenAsync();
        Db.UpdateOnly<ProcessReport>(() => new ProcessReport { IsRunning = false }, a => a.IsRunning);

    }
    protected override void StartPrograms()
    {
        if (_maxInstances == null)
        {
            throw new ArgumentException(
                "No max instances, set ClipHuntaMaxInstances in app settings for initial value or call MaxInstances(i)");
        }

        var count = _maxInstances - dtoProgramInstances.Count;
        if (count <= 0) return;
        using var db = HostContext.Resolve<IDbConnectionFactory>().Open();

        for (var i = 0; i < count; i++)
        {
            var started = StartProgram();
            if (started != null)
            {
                int id = started.ProcessId();
                started.DatabaseId = (int)db.Insert(new ProcessReport()
                {
                    ExitCode = -1,
                    IsRunning = true,
                    ProcessId = id,
                    ReportText = "Started",
                    StreamerCommandCenterId = -1
                }, true);
            }
        }
    }

    private async Task CheckWork(List<(Streamer streamer, TwitchOauthTokens tokens, int twitchStreamId)> streamersOut, CancellationToken stoppingToken)
    {

        await RetrieveAssignedStreams(streamersOut);
        await FillUnusedSlots(streamersOut, stoppingToken);

    }

    private async Task FillUnusedSlots(List<(Streamer streamer, TwitchOauthTokens tokens, int twitchStreamId)> streamersOut, CancellationToken stoppingToken)
    {

        foreach (var nR in streamersOut)
        {
            if (stoppingToken.IsCancellationRequested)
                break;
            var result = await AssignStreamer(nR, stoppingToken);
            if (result.Started)
                continue;

            switch (result.Reason)
            {
                case AssignDtoResult.EReason.NoProcess:
                    return;

                case AssignDtoResult.EReason.NoClient:
                    //no connection or connection failed
                    //kill the process
                    result.Instance?.Process()?.Kill();
                    //it'll be removed on process on exit call back
                    break;
                case AssignDtoResult.EReason.Started:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    private async Task<AssignDtoResult> AssignClip((Streamer streamer, TwitchOauthTokens tokens, int twitchStreamId, string twitchClip) nR, CancellationToken stoppingToken)
    {
        var instance = remoteDtoProgramInstances.FirstOrDefault(a => !a.DtoId.HasValue);
        if (instance == null)
        {

            instance = dtoProgramInstances.FirstOrDefault(a => !a.DtoId.HasValue);
            if (instance == null)
            {

                return new AssignDtoResult(Started: false, ReasonString: "no available processes",
                    AssignDtoResult.EReason.NoProcess);
            }
        }

        var client = _hubContext.Clients.Client(instance.ConnectionId());


        instance.DtoId = nR.streamer.Id;
        await client.Clip(nR.streamer.Name, nR.twitchClip, nR.twitchStreamId);
        return new AssignDtoResult(true, "", AssignDtoResult.EReason.Started);
    }

    private async Task<AssignDtoResult> AssignStreamer((Streamer streamer, TwitchOauthTokens tokens, int twitchStreamId) nR, CancellationToken stoppingToken)
    {
        var instance = remoteDtoProgramInstances.FirstOrDefault(a => !a.DtoId.HasValue);
        if (instance == null)
        {

            instance = dtoProgramInstances.FirstOrDefault(a => !a.DtoId.HasValue);
            if (instance == null)
            {

                return new AssignDtoResult(Started: false, ReasonString: "no available processes",
                    AssignDtoResult.EReason.NoProcess);
            }
        }

        var client = _hubContext.Clients.Client(instance.ConnectionId());


        instance.DtoId = nR.streamer.Id;
        await client.Watch(nR.streamer.Name, nR.twitchStreamId);
        return new AssignDtoResult(true, "", AssignDtoResult.EReason.Started);
    }

    public void UpdateReports(QueueReport[] queueReports)
    {
        foreach (var queue in queueReports)
        {
            DtoProgramInstance process = dtoProgramInstances.FirstOrDefault(a => a.ProcessId() == queue.ProcessId);
            if (process != null)
                process.UpdateReport(queue);
        }

    }
    ConcurrentQueue<(string connectionId, int twitchStreamId, string stackTrace, string errorMessage, int? processId, int? streamId, int? processDtoId)> streamErrorQueue = new();
    public void DtoMarkStreamProcessError(string connectionId, int twitchStreamId, string stackTrace, string errorMessage)
    {
        var instance = remoteDtoProgramInstances.FirstOrDefault(a => a.ConnectionId() == connectionId);
        if (instance == null)
        {

            instance = dtoProgramInstances.FirstOrDefault(a => a.ConnectionId() == connectionId);
            if (instance == null)
            {
                streamErrorQueue.Enqueue((connectionId, twitchStreamId, stackTrace, errorMessage, null, null, null));
                //todo: What if some how it's not here? Only could happen on reconnect after server reboot
                //possibly send a reregister message to the client
                return;
            }
        }
        instance.DtoId = null;
        streamErrorQueue.Enqueue((connectionId, twitchStreamId, stackTrace, errorMessage, instance.ProcessId(), instance.DtoId, instance.DatabaseId));
        //todo: update records associated with this run
    }
    ConcurrentQueue<(string connectionId, int twitchClipId, string stackTrace, string errorMessage, int? processId, int? streamId, int? processDtoId)> clipErrorQueue = new();
    public void DtoMarkClipProcessError(string connectionId, int twitchClipId, string stackTrace, string errorMessage)
    {
        var instance = remoteDtoProgramInstances.FirstOrDefault(a => a.ConnectionId() == connectionId);
        if (instance == null)
        {

            instance = dtoProgramInstances.FirstOrDefault(a => a.ConnectionId() == connectionId);
            if (instance == null)
            {
                clipErrorQueue.Enqueue((connectionId, twitchClipId, stackTrace, errorMessage, null, null, null));
                //todo: What if some how it's not here? Only could happen on reconnect after server reboot
                //possibly send a reregister message to the client
                return;
            }
        }
        clipErrorQueue.Enqueue((connectionId, twitchClipId, stackTrace, errorMessage, instance.ProcessId(), instance.DtoId, instance.DatabaseId));
        instance.DtoId = null; //this is the id of the stream but we have tha if we look u p the other table
        //todo: update records associated with this run
    }
    public void DtoMarkStreamProcessFinished(string connectionId, int twitchStreamId)
    {
        var instance = remoteDtoProgramInstances.FirstOrDefault(a => a.ConnectionId() == connectionId);
        if (instance == null)
        {

            instance = dtoProgramInstances.FirstOrDefault(a => a.ConnectionId() == connectionId);
            if (instance == null)
            {
                //todo: What if some how it's not here? Only could happen on reconnect after server reboot
                //possibly send a reregister message to the client
                return;
            }
        }
        instance.DtoId = null;
        //todo: update records associated with this run
    }
    public void DtoMarkClipProcessFinished(string connectionId, int twitchClipId)
    {
        var instance = remoteDtoProgramInstances.FirstOrDefault(a => a.ConnectionId() == connectionId);
        if (instance == null)
        {

            instance = dtoProgramInstances.FirstOrDefault(a => a.ConnectionId() == connectionId);
            if (instance == null)
            {
                //todo: What if some how it's not here? Only could happen on reconnect after server reboot
                //possibly send a reregister message to the client
                return;
            }
        }

        instance.DtoId = null; //this is the id of the stream but we have tha if we look u p the other table
        //todo: update records associated with this run
    }
}