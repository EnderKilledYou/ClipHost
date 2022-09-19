using Microsoft.AspNetCore.SignalR;
using ServiceStack;
using ServiceStack.Host;
using ServiceStack.Testing;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;

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
            return Connections.TryRemove(connectionId, out _);
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
    }

    public class ServiceGatewayHub<T> : Hub<T> where T : class
    {
        private readonly MethodInfo? _mi;

        private readonly MethodInfo? _puba;

        public ServiceGatewayHub()
        {
            _mi = typeof(InProcessServiceGateway).GetMethod("SendAllAsync");

            _puba = typeof(InProcessServiceGateway).GetMethod("PublishAllAsync");
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
                return await GetServiceGateway()
                    .SendAsync(responseType, requestDto.Data.Deserialize(requestType))
                    .ConfigureAwait(false);
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
            if (_mi == null)
                return null;

            Type? responseType = requestDtos.GetResponseType();
            Type? requestType = requestDtos.GetRequestType();


            if (responseType == null || requestType == null)
                return null;
            var obj = requestDtos.Data.Deserialize(requestType);
            var fooRef = _mi?.MakeGenericMethod(responseType);
            if (fooRef == null) return null;
            if (@fooRef.Invoke(GetServiceGateway(), new[] { obj, null }) is not Task result) return null;
            await result.ConfigureAwait(false);
            return result.GetType().GetProperty("Result")?.GetValue(result);
        }

        public async Task PublishAsync(MetaPacket requestDto)
        {
            var responseType = requestDto.GetResponseType();
            var requestType = requestDto.GetRequestType();

            if (responseType != null && requestType != null)
            {
                await GetServiceGateway().PublishAsync(requestDto.Data.Deserialize(requestType))
                    .ConfigureAwait(false);
            }
        }

        public async Task PublishAllAsync(MetaPacket requestDtos)

        {
            if (_puba == null) return;
            var responseType = requestDtos.GetResponseType();
            var requestType = requestDtos.GetRequestType();

            if (responseType != null && requestType != null)
            {
                if (_puba.Invoke(GetServiceGateway(),
                        new[]
                            { requestDtos.Data.Deserialize(requestType), null }) is not Task result)
                    return;

                await result.ConfigureAwait(false);
            }
        }
    }
}