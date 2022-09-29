using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ClipHost;
using ClipHost.ServiceModel;
using Microsoft.AspNetCore.Connections.Features;

namespace BlazorQueue;

public interface IClipStreams
{
    Task Clip(string clipId, string twitchClip, int twitchStreamId);
    Task Watch(string streamer, int twitchStreamId);
}



public class ClipHub : ServiceGatewayHub<IClipStreams>
{
    private readonly StreamerProcessWrangler _clipProcessWrangler;
    public void Register(int processId)
    {
        _clipProcessWrangler.SetConnectionId(Context.ConnectionId, processId);
    }
    public void RegisterRemote(int processId/*, int commandCenterId */)
    {
        _clipProcessWrangler.AddRemoteConnectionId(Context.ConnectionId, processId);
    }
    public void StreamFinished(int twitchStreamId)
    {
        _clipProcessWrangler.DtoMarkStreamProcessFinished(Context.ConnectionId, twitchStreamId);
    }
    public void ClipFinished(int twitchClipId)
    {
        _clipProcessWrangler.DtoMarkClipProcessFinished(Context.ConnectionId, twitchClipId);
    }
    public void StreamError(int twitchStreamId, string stack, string message)
    {
        _clipProcessWrangler.DtoMarkStreamProcessFinished(Context.ConnectionId, twitchStreamId);
    }
    public void ClipError(int twitchClipId, string stack, string message)
    {
        _clipProcessWrangler.DtoMarkClipProcessFinished(Context.ConnectionId, twitchClipId);
    }

    public void UpdateQueues(QueueReport[] queueReports)
    {
        _clipProcessWrangler.UpdateReports(queueReports);
    }

    public ClipHub(StreamerProcessWrangler clipProcessWrangler)
    {
        this._clipProcessWrangler = clipProcessWrangler;
    }
}