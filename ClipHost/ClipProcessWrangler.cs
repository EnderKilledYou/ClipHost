using BlazorQueue;
using Microsoft.AspNetCore.SignalR;

namespace ClipHost;

 
    public class ClipProcessWrangler : BackgroundService
    {
    private readonly IHubContext<ClipHub> _hubContext;
    private readonly ClipIdManager _clipIdManager;

    public ClipProcessWrangler(IHubContext<ClipHub> hubContext,ClipIdManager clipIdManager)
    {
        _hubContext = hubContext;
        _clipIdManager = clipIdManager;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            Console.WriteLine(_hubContext);
       
            var started = false;
            while (stoppingToken.IsCancellationRequested)
            {
                if (!started &&  _clipIdManager.Count() > 1)
                {
                    IClipStreams clientProxy = (IClipStreams)_hubContext.Clients.Client("").ConvertTo<IClipStreams>();
                    await clientProxy.Clip("");
                    started = true;
                }
                await Task.Delay(200, stoppingToken);
            }
        }
    }

 