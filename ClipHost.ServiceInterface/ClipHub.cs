using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections.Features;

namespace BlazorQueue;

public interface IClipStreams
{
    Task Clip(string clipId);
    Task Watch(string streamer);
}

 

public class ClipHub : ServiceGatewayHub<IClipStreams>
{
   
    public ClipHub(ClipIdManager blazorHubIdManager) : base(blazorHubIdManager)
    {
            
    }
}