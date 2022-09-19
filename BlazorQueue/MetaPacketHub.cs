using Microsoft.AspNetCore.SignalR;
using ServiceStack;
using ServiceStack.Testing;
using System.Reflection;
using System.Text.Json;

namespace BlazorQueue
{
    
        public class MetaPacketHub<T> : Hub<T> where T : class
    {
        private readonly MethodInfo? _mi;
        private readonly MethodInfo? _puba;
        public MetaPacketHub()
        {
               
        _mi = typeof(InProcessServiceGateway).GetMethod("SendAllAsync");
        _puba = typeof(InProcessServiceGateway).GetMethod("PublishAllAsync");
    }

        public async Task PublishAllAsync(MetaPacket requestDtos)

        {
            if (_puba == null) return;
            var responseType = requestDtos.GetResponseType();
            var requestType = requestDtos.GetRequestType();

            if (responseType != null && requestType != null)
            {
                if (_puba.Invoke(GetServiceGateway(), new object?[] { requestDtos.Data.Deserialize(requestType), null }) is not Task result) return;

                await result.ConfigureAwait(false);

            }


        }

        public async Task PublishAsync(MetaPacket requestDto)
        {
            Type? responseType = requestDto.GetResponseType();
            Type? requestType = requestDto.GetRequestType();

            if (responseType != null && requestType != null)
            {
                await GetServiceGateway().PublishAsync(requestDto.Data.Deserialize(requestType)).ConfigureAwait(false);

            }

        }



        public async Task<object?> SendAllAsync(MetaPacket requestDtos)
        {
            if (_mi == null)
                return null;

            var responseType = requestDtos.GetResponseType();
            var requestType = requestDtos.GetRequestType();


            if (responseType == null || requestType == null) 
                return null;
            var obj = requestDtos.Data.Deserialize(requestType);
            var fooRef = _mi?.MakeGenericMethod(responseType);
            if (fooRef == null) return null;
            if (@fooRef.Invoke(GetServiceGateway(), new object?[] { obj, null }) is not Task result) return null;
            await result.ConfigureAwait(false);
            return result.GetType()?.GetProperty("Result")?.GetValue(result);

        }

        public async Task<object?> SendAsync(MetaPacket requestDto)
        {


            var responseType = requestDto.GetResponseType();
            var requestType = requestDto.GetRequestType();
            if (responseType == null || requestType == null) return null;
            return await GetServiceGateway().SendAsync(responseType, requestDto.Data.Deserialize(requestType)).ConfigureAwait(false); ;

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