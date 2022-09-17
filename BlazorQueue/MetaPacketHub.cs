using Microsoft.AspNetCore.SignalR;
using ServiceStack;
using ServiceStack.Testing;
using System.Reflection;
using System.Text.Json;

namespace BlazorQueue
{
    
        public class MetaPacketHub<T> : Hub<T> where T : class
    {
        protected readonly MethodInfo? mi;
        protected readonly MethodInfo? puba;
        public MetaPacketHub()
        {
               
        mi = typeof(InProcessServiceGateway).GetMethod("SendAllAsync");
        puba = typeof(InProcessServiceGateway).GetMethod("PublishAllAsync");
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

        public async Task PublishAsync(MetaPacket requestDto)
        {
            Type? responseType = requestDto.GetResponseType();
            Type? requestType = requestDto.GetRequestType();

            if (responseType != null && requestType != null)
            {
                await GetServiceGateway().PublishAsync(JsonSerializer.Deserialize(requestDto.Data, requestType)).ConfigureAwait(false);

            }

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
    }
}