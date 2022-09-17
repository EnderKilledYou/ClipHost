using ClipHost.ServiceModel;
using ServiceStack;

namespace ClipHost.ServiceInterface;

public class MyServices : Service
{
    public MyServices(IServerEvents serverEvents)
    {
        ServerEvents = serverEvents;
    }

    public IServerEvents ServerEvents { get; set; }

    public object Any(HelloTest request)
    {
        return new HelloTestResponse { Result = $"Hello, {request.Name}!" };
    }

    public object Any(Hello request)
    {
        return new HelloResponse { Result = $"Hello, {request.Name}!" };
    }
}