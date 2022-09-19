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
    Task Clip(string clipId);
    Task Watch(string streamer);
}



public class ClipHub : ServiceGatewayHub<IClipStreams>
{
    private readonly StreamerProcessWrangler _clipProcessWrangler;
    public void Register(int processId)
    {
        _clipProcessWrangler.SetConnectionId(Context.ConnectionId, processId);
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