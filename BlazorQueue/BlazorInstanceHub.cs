
using Microsoft.AspNetCore.SignalR;
using ServiceStack;
using ServiceStack.Host;
using ServiceStack.Testing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorQueue
{
    public abstract class HubIdManager
    {
        private ConcurrentDictionary<string, int> Connections { get; } = new();

        public virtual bool AddConnection(string connectionId)
        {
            return Connections.TryAdd(connectionId, 0);
        }

        public virtual bool RemoveConnection(string connectionId)
        {
            return Connections.TryRemove(connectionId, out var value);
        }

        public virtual (string, int)[] GetConnections()
        {
            return Connections.Select(a => (a.Key, a.Value)).ToArray();
        }
        public virtual int Count()
        {
            return Connections.Count;
        }
    }

    public class BlazorHubIdManager : HubIdManager
    {
        
    }
    public class BlazorHub : ServiceGatewayHub<IBlazorInstanceFacade>
    {
        public BlazorHub(BlazorHubIdManager blazorHubIdManager) : base(blazorHubIdManager)
        {
            
        }
    }
    public class ServiceGatewayHub<T> : Hub<T> where T : class
    {

        private readonly HubIdManager _idManager;

        public ServiceGatewayHub(HubIdManager idManager)
        {
            _idManager = idManager;
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine("ok");
            _idManager.AddConnection(this.Context.ConnectionId);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _idManager.RemoveConnection(this.Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        private readonly MethodInfo? mi;

        private readonly MethodInfo? puba;

        public ServiceGatewayHub()
        {
            mi = typeof(InProcessServiceGateway).GetMethod("SendAllAsync");

            puba = typeof(InProcessServiceGateway).GetMethod("PublishAllAsync");
        }
        public List<OperationDto> ServiceDiscovery()
        {
            return HostContext.Metadata.GetOperationDtos();
        }



        public async Task<object?> SendAsync(MetaPacket requestDto)
        {


            Type? responseType = requestDto.GetResponseType();
            Type? requestType = requestDto.GetRequestType();
            if (responseType != null && requestType != null)
            {

                return await GetServiceGateway().SendAsync(responseType, JsonSerializer.Deserialize(requestDto.Data, requestType)).ConfigureAwait(false); ;

            }

            return null;


        }

        private IServiceGateway GetServiceGateway()
        {
            var request = new MockHttpRequest();

            var serviceGatewayAsync = HostContext.AppHost.GetServiceGateway(request);
            return serviceGatewayAsync;
        }



        public async Task<object?> SendAllAsync(MetaPacket requestDtos)
        {
            if (mi == null)
                return null;

            Type? responseType = requestDtos.GetResponseType();
            Type? requestType = requestDtos.GetRequestType();


            if (responseType != null && requestType != null)
            {
                var obj = JsonSerializer.Deserialize(requestDtos.Data, requestType);
                var fooRef = mi?.MakeGenericMethod(responseType);
                if (fooRef == null) return null;
                if (@fooRef.Invoke(GetServiceGateway(), new object?[] { obj, null }) is not Task result) return null;
                await result.ConfigureAwait(false);
                return result.GetType()?.GetProperty("Result")?.GetValue(result);
            }

            return null;
        }

        public async Task PublishAsync(MetaPacket requestDto)
        {
            Type? responseType = requestDto.GetResponseType();
            Type? requestType = requestDto.GetRequestType();

            if (responseType != null && requestType != null)
            {
                await GetServiceGateway().PublishAsync(JsonSerializer.Deserialize(requestDto.Data, requestType)).ConfigureAwait(false);

            }

        }

        public async Task PublishAllAsync(MetaPacket requestDtos)

        {
            if (puba == null) return;
            Type? responseType = requestDtos.GetResponseType();
            Type? requestType = requestDtos.GetRequestType();

            if (responseType != null && requestType != null)
            {
                if (puba.Invoke(GetServiceGateway(), new object?[] { JsonSerializer.Deserialize(requestDtos.Data, requestType), null }) is not Task result) return;

                await result.ConfigureAwait(false);

            }


        }
    }

}