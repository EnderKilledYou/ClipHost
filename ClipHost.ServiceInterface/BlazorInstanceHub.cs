using ClipHost.ServiceModel;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.SignalR;
using ServiceStack;
using ServiceStack.Host;
using ServiceStack.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorQueue.ServiceInterface
{
    public class ServiceGatewayHub : Hub<IBlazorInstanceFacade>
    {
        private readonly MethodInfo mi;
        private readonly MethodInfo pub;
        private readonly MethodInfo puba;

        public ServiceGatewayHub()
        {
            mi = typeof(InProcessServiceGateway).GetMethod("SendAllAsync");

            puba = typeof(InProcessServiceGateway).GetMethod("PublishAllAsync");
            
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
            var responseTypeJson = props.FirstOrDefault(a => a.Name == "ResponseType");
            var typeJson = props.FirstOrDefault(a => a.Name == "Type");
            var data = props.FirstOrDefault(a => a.Name == "Data");

            Type responseType = GetTypeByName(responseTypeJson.Value.ToString());
            Type requestType = GetTypeByName(typeJson.Value.ToString());
            if (responseType != null && requestType != null)
            {
                var obj = JsonSerializer.Deserialize(data.Value, requestType);

                var result = await GetServiceGateway().SendAsync(responseType, obj).ConfigureAwait(false); ;
                return result;
            }

            return null;


        }

        private IServiceGateway GetServiceGateway()
        {
            var request = new MockHttpRequest();

            var serviceGatewayAsync = HostContext.AppHost.GetServiceGateway(request);
            return serviceGatewayAsync;
        }



        public async Task<object> SendAllAsync(object requestDtos)
        {
            var props = ((JsonElement)requestDtos).EnumerateObject().ToArray();
            var responseTypeJson = props.FirstOrDefault(a => a.Name == "ResponseType");
            var typeJson = props.FirstOrDefault(a => a.Name == "Type");
            var data = props.FirstOrDefault(a => a.Name == "Data");

            Type responseType = GetTypeByName(responseTypeJson.Value.ToString());
            Type requestType = GetTypeByName(typeJson.Value.ToString());
            if (responseType != null && requestType != null)
            {
                var bv = GetServiceGateway();
                var obj = JsonSerializer.Deserialize(data.Value, requestType);
                var fooRef = mi.MakeGenericMethod(responseType);
                var result = (Task)@fooRef.Invoke(GetServiceGateway(), new object?[] { obj, null });
                await result.ConfigureAwait(false);
                //var result = await GetServiceGateway().SendAllAsync((IEnumerable<IReturn<object>>) obj);
                var resultProperty = result.GetType().GetProperty("Result");
#if DEBUG
                await Clients.Caller.PublishAsyncToCaller<HelloTestResponse, HelloTest>(new HelloTest() { });
#endif 
                return resultProperty.GetValue(result);
            }

            return null;
        }

        public async Task PublishAsync(object requestDto)
        {
            var props = ((JsonElement)requestDto).EnumerateObject().ToArray();
            var responseTypeJson = props.FirstOrDefault(a => a.Name == "ResponseType");
            var typeJson = props.FirstOrDefault(a => a.Name == "Type");
            var data = props.FirstOrDefault(a => a.Name == "Data");

            Type responseType = GetTypeByName(responseTypeJson.Value.ToString());
            Type requestType = GetTypeByName(typeJson.Value.ToString());
            if (responseType != null && requestType != null)
            {
                var obj = JsonSerializer.Deserialize(data.Value, requestType);
                await GetServiceGateway().PublishAsync(obj).ConfigureAwait(false);

            }

        }

        public async Task PublishAllAsync(object requestDtos)

        {
            var props = ((JsonElement)requestDtos).EnumerateObject().ToArray();
            var responseTypeJson = props.FirstOrDefault(a => a.Name == "ResponseType");
            var typeJson = props.FirstOrDefault(a => a.Name == "Type");
            var data = props.FirstOrDefault(a => a.Name == "Data");

            Type responseType = GetTypeByName(responseTypeJson.Value.ToString());
            Type requestType = GetTypeByName(typeJson.Value.ToString());
            if (responseType != null && requestType != null)
            {
                var bv = GetServiceGateway();

                var obj = JsonSerializer.Deserialize(data.Value, requestType);
                var result = (Task)puba.Invoke(GetServiceGateway(), new object?[] { obj, null });
                await result.ConfigureAwait(false);

            }


        }
    }
    public static class BlazorInstanceHubHelper
    {
        public async static Task PublishAsyncToCaller<TResponse, R>(this IBlazorInstanceFacade caller, object requestDto, CancellationToken token = default) where R : IReturn<TResponse>
        {

            string retfullName = typeof(TResponse).FullName;
            string typefullname = typeof(R).FullName;
            await caller.LocalPublishAsync(new { ResponseType = retfullName, Type = typefullname, Data = requestDto });
        }

    }
}