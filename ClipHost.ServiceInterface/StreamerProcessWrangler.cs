using BlazorQueue;
using ClipHost.ServiceModel;
using Microsoft.AspNetCore.SignalR;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
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
        base(HostContext.AppSettings)
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
    protected async Task RetrieveAssignedStreams(List<Streamer> streamersOut)
    {
 
        using var db = await HostContext.Resolve<IDbConnectionFactory>().OpenAsync();
        var mineStatement = db.From<StreamerCommandCenter>();
        var streamers = mineStatement.Where(a => a.CommandCenterId == commandCenter.Id)
            .Join<Streamer>((a, b) => a.CommandCenterId == b.Id);
        var results = db.SelectMulti<StreamerCommandCenter, Streamer>(streamers);
        streamersOut.AddRange(results.Select(result => result.Item2));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (HostUrl == null)
        {
            throw new ArgumentException("No host setting, set ClipHuntaUrlSetting in app settings ");
        }

        if (string.IsNullOrEmpty(ClipHuntaProcessPath))
        {
            //nothing for them to connect to
            return;
        }

        List<Streamer> streamersOut = new();
        StartPrograms();
        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckWork(streamersOut, stoppingToken);
            streamersOut.Clear();
            CleanDeadProcesses();
            StartPrograms();
            await Task.Delay(100, stoppingToken).ConfigureAwait(false);
        }
    }

    private async Task CheckWork(List<Streamer> streamersOut, CancellationToken stoppingToken)
    {
        if (Count() > 0)
        {
            await RetrieveAssignedStreams(streamersOut);
            await FillUnusedSlots(streamersOut, stoppingToken);
        }
    }

    private async Task FillUnusedSlots(List<Streamer> streamersOut, CancellationToken stoppingToken)
    {
        var items = streamersOut.Where(a =>
            !dtoProgramInstances.Any(b =>
                b.DtoId == a.Id && !string.IsNullOrEmpty(b.ConnectionId()))).ToArray();

        foreach (var nR in items)
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

    private async Task<AssignDtoResult> AssignStreamer(Streamer nR, CancellationToken stoppingToken)
    {
        var instance = dtoProgramInstances.FirstOrDefault(a => !((DtoProgramInstance)a).DtoId.HasValue);
        if (instance == null)
        {
            return new AssignDtoResult(Started: false, ReasonString: "no available processes",
                AssignDtoResult.EReason.NoProcess);
        }

        var client = _hubContext.Clients.Client(instance.ConnectionId());


        instance.DtoId = nR.Id;
        await client.Watch(nR.Name);
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
}