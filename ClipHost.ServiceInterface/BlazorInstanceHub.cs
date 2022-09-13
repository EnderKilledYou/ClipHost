using ClipHost.ServiceModel;
using Microsoft.AspNetCore.SignalR;
using ServiceStack;
using ServiceStack.Host;
using ServiceStack.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorQueue.ServiceInterface
{
    public class ServiceGatewayHub : Hub
    {
        public ServiceGatewayHub()
        {
        }

        public List<OperationDto> ServiceDiscovery()
        {
            return HostContext.Metadata.GetOperationDtos();
        }

        public Type GetTypeByName(string name)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type responseType = assembly.GetType(name);
                if (responseType != null) return responseType;
            }
            return null;
        }

        public async Task<object> SendAsync(object requestDto)
        {
            var props = ((JsonElement)requestDto).EnumerateObject().ToArray();
            var responseTypeJson = props.FirstOrDefault(a => a.Name == "responseType");
            var typeJson = props.FirstOrDefault(a => a.Name == "type");
            var data = props.FirstOrDefault(a => a.Name == "data");
            Hello h = new Hello();
            var tname = h.GetType().FullName;

            Type responseType = GetTypeByName(responseTypeJson.Value.ToString());
            Type requestType = GetTypeByName(typeJson.Value.ToString());
            if (responseType != null && requestType != null)
            {
                var obj = JsonSerializer.Deserialize(data.Value, requestType);

                var result = await GetServiceGateway().SendAsync(responseType, obj);
                return result;
            }

            return null;

            //
            //
        }

        private IServiceGateway GetServiceGateway()
        {
            var request = new MockHttpRequest();

            var serviceGatewayAsync = HostContext.AppHost.GetServiceGateway(request);
            return serviceGatewayAsync;
        }

        public async Task<List<object>> SendAllAsync(IEnumerable<IReturn<object>> requestDtos)
        {
            return await GetServiceGateway().SendAllAsync<object>(requestDtos);
        }

        public async Task PublishAsync(object requestDto)
        {
            await GetServiceGateway().PublishAsync(requestDto);
        }

        public async Task PublishAllAsync(IEnumerable<object> requestDtos)
        {
            await GetServiceGateway().PublishAllAsync(requestDtos);
        }
    }
}