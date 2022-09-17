using Microsoft.AspNetCore.SignalR.Client;
using ServiceStack;
using ServiceStack.Host;
using ServiceStack.Web;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorQueue
{
    public record HubConnectionInfo(string HostUrl, string HubName, Func<Task<string?>>? AccessTokenProvider);

    public class BlazorInstanceFacade : BlazorFacadeConnectionBase,  IAsyncDisposable, IJsonOnDeserialized, IConnectFacadesBinary, IBlazorInstanceFacade
    {
        protected BlazorInstanceFacade? ParentFacade { get; init; }

        protected BlazorInstanceFacade[] _subFacades = new BlazorInstanceFacade[2];
        /// <summary>
        ///  Pass in the parent facade 
        ///  
        /// </summary>
        /// <param name="parent">The parent facade to connect to</param>

        public BlazorInstanceFacade(BlazorInstanceFacade? parent, bool isRoot = false) : base(parent?.ParentConnection, isRoot: isRoot)
        {
            ParentFacade = parent;
        }

        public BlazorInstanceFacade(HubConnectionInfo info) : base(info, isRoot: true)
        {

        }

        private IServiceGateway GetServiceGateway(string function, string requestName, string responseName)
        {
            var request = new BasicRequest()
            {
             Verb = "GET",
             ContentType = "application/json",
             Headers = new NameValueCollection()
             {

             },
             Files = Array.Empty<IHttpFile>(),
             RawUrl = $"/{requestName}/{responseName}",
             
            };

            var serviceGatewayAsync = HostContext.AppHost.GetServiceGateway(request);
            return serviceGatewayAsync;
        }

     

        /// <summary>
        /// Responds to a send request
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public override async Task<object?> LocalSendAsync(MetaPacket requestDto)
        {
            Type? responseType = requestDto.GetResponseType();
            Type? requestType = requestDto.GetRequestType();
            if (responseType != null && requestType != null)
            {
             
                var result = await GetServiceGateway("LocalSendAsync", requestType.Name, responseType.Name).SendAsync(responseType, JsonSerializer.Deserialize(requestDto.Data, requestType)).ConfigureAwait(false); ;
                return result;
            }
            return null;
        }

        /// <summary>
        /// Responds to send a list of items to gate/send
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        public override async Task<object?> LocalSendAllAsync(MetaPacket requestDtos)
        {
            if(mi == null)
            {
                return null;
            }
            Type? responseType = requestDtos.GetResponseType();
            Type? requestType = requestDtos.GetRequestType();
            if (responseType != null && requestType != null)
            {
                var bv = GetServiceGateway("LocalSendAsync", requestType.Name, responseType.Name);
                var obj = JsonSerializer.Deserialize(requestDtos.Data, requestType);
                var fooRef = mi.MakeGenericMethod(responseType);
                if (@fooRef.Invoke(bv, new object?[] { obj, null }) is not Task result)
                    return null;
                await result.ConfigureAwait(false);
                return result.GetType().GetProperty("Result")?.GetValue(result);
            }

            return null;
        }

        /// <summary>
        /// Actions to a request to publish to a service
        ///
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public override async Task LocalPublishAsync(MetaPacket requestDto)
        {
            Type? responseType = requestDto.GetResponseType();
            Type? requestType = requestDto.GetRequestType();

            if (responseType != null && requestType != null)
            {
                var obj = JsonSerializer.Deserialize(requestDto.Data, requestType);
                await GetServiceGateway("LocalPublishAsync", requestType.Name, responseType.Name).PublishAsync(obj).ConfigureAwait(false);
 
            }
        }
 
        /// <summary>
        /// Actions a list of requests to publish to a service.
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        public override async Task LocalPublishAllAsync(MetaPacket requestDtos)

        {
            if(puba == null)
            {
                return;
            }
            Type responseType = requestDtos.GetResponseType();
            Type requestType = requestDtos.GetRequestType();
            if (responseType != null && requestType != null)
            {
                var bv = GetServiceGateway("LocalPublishAllAsync", requestType.Name, responseType.Name);

                var obj = JsonSerializer.Deserialize(requestDtos.Data, requestType);
                if (puba.Invoke(bv, new object?[] { obj, null }) is not Task result)
                {
                    return;
                }
                await result.ConfigureAwait(false);
            }
        }

        /// <summary>
        ///  return the first item or null
        /// </summary>
        /// <returns></returns>
        public IConnectFacadesBinary GetLeftFacade()
        {
            return _subFacades[0];
        }

        /// <summary>
        /// Returns the second item or null if length < 2
        /// </summary>
        /// <returns></returns>
        public IConnectFacadesBinary GetRightFacade()
        {
            return _subFacades[1];
        }

        public void GetAllFacades(IConnectFacades[] connectFacades)
        {
            if (connectFacades.Length != _subFacades.Length + 1)
            {
                throw new ArgumentException($"Need array of size {_subFacades.Length + 1}");
            }
            connectFacades[0] = GetParentFacade();
            for (int i = 0; i < _subFacades.Length; i++)
            {
                connectFacades[i + 1] = _subFacades[i];
            }
        }

        public void GetAllFacades(IConnectFacadesBinary[] connectFacades)
        {
            if (connectFacades.Length != 3)
            {
                throw new ArgumentException("Need array of size 3");
            }
            connectFacades[0] = GetParentFacade();
            connectFacades[1] = GetLeftFacade();
            connectFacades[2] = GetRightFacade();
        }

        public IConnectFacadesBinary GetParentFacade()
        {
            return ParentFacade;
        }

        public void SetLeftFacade(IConnectFacadesBinary left)
        {
            _subFacades[0] = (BlazorInstanceFacade)left;
        }

        public void SetRightFacade(IConnectFacadesBinary right)
        {
            _subFacades[1] = (BlazorInstanceFacade)right;
        }
    }
}
