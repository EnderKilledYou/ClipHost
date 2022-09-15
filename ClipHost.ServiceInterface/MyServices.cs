using System;
using ServiceStack;
using ClipHost.ServiceModel;
using ServiceStack.Testing;

namespace ClipHost.ServiceInterface
{
    
    public class MyServices : Service
    {
        public IServerEvents ServerEvents { get; set; }

        public MyServices(IServerEvents serverEvents)
        {
            ServerEvents = serverEvents;
        }

        public object Any(HelloTest request)
        {
            
            return new HelloTestResponse { Result = $"Hello, {request.Name}!" };
        }
        public object Any(Hello request)
        {
             
            return new HelloResponse { Result = $"Hello, {request.Name}!" };
        }
    }
}
