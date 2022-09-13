using System;
using ServiceStack;
using ClipHost.ServiceModel;

namespace ClipHost.ServiceInterface
{
    
    public class MyServices : Service
    {
        public IServerEvents ServerEvents { get; set; }

        public MyServices(IServerEvents serverEvents)
        {
            ServerEvents = serverEvents;
        }

        
        public object Any(Hello request)
        {
             
            return new HelloResponse { Result = $"Hello, {request.Name}!" };
        }
    }
}
