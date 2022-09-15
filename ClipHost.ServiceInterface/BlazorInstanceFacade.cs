using Microsoft.AspNetCore.SignalR.Client;
using ServiceStack;
using ServiceStack.Testing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorQueue.ServiceInterface
{
    public record HubConnectionInfo
    {
        public string HostUrl { get; init; }
        public string HubName { get; init; }
        public Func<Task<string>> accessTokenProvider { get; init; }
    }

    public class BlazorInstanceFacade : BlazorInstanceFacadeBase, IAsyncDisposable, IJsonOnDeserialized, IConnectFacadesBinary, IBlazorInstanceFacade
    {
        protected BlazorInstanceFacade parentFacade { get; init; }

        protected BlazorInstanceFacade[] _subFacades = new BlazorInstanceFacade[2];
        /// <summary>
        ///  Pass in the parent facade 
        ///  
        /// </summary>
        /// <param name="parent">The parent facade to connect to</param>

        public BlazorInstanceFacade(BlazorInstanceFacade parent, bool isRoot = false) : base(parent?.ParentConnection, isRoot: isRoot)
        {
            parentFacade = parent;
        }

        public BlazorInstanceFacade(HubConnectionInfo info) : base(info, isRoot: true)
        {

        }

        private IServiceGateway GetServiceGateway(string function, string requestName, string responseName)
        {
            var request = new MockHttpRequest(function, "GET", "json", $"/{requestName}/{responseName}", new NameValueCollection
            {
            }, Stream.Null, new NameValueCollection() { });

            var serviceGatewayAsync = HostContext.AppHost.GetServiceGateway(request);
            return serviceGatewayAsync;
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

        /// <summary>
        /// Responds to a send request
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public override async Task<object> LocalSendAsync(object requestDto)
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

                var result = await GetServiceGateway("LocalSendAsync", requestType.Name, responseType.Name).SendAsync(responseType, obj).ConfigureAwait(false); ;
                return result;
            }
            return null;
        }

        /// <summary>
        /// Responds to send a list of items to gate/send
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        public override async Task<object> LocalSendAllAsync(object requestDtos)
        {
            var props = ((JsonElement)requestDtos).EnumerateObject().ToArray();
            var responseTypeJson = props.FirstOrDefault(a => a.Name == "ResponseType");
            var typeJson = props.FirstOrDefault(a => a.Name == "Type");
            var data = props.FirstOrDefault(a => a.Name == "Data");

            Type responseType = GetTypeByName(responseTypeJson.Value.ToString());
            Type requestType = GetTypeByName(typeJson.Value.ToString());
            if (responseType != null && requestType != null)
            {
                var bv = GetServiceGateway("LocalSendAsync", requestType.Name, responseType.Name);
                var obj = JsonSerializer.Deserialize(data.Value, requestType);
                var fooRef = mi.MakeGenericMethod(responseType);
                var result = (Task)@fooRef.Invoke(bv, new object?[] { obj, null });
                await result.ConfigureAwait(false);
                //var result = await GetServiceGateway().SendAllAsync((IEnumerable<IReturn<object>>) obj);
                var resultProperty = result.GetType().GetProperty("Result");
                return resultProperty.GetValue(result);
            }

            return null;
        }

        /// <summary>
        /// Actions to a request to publish to a service
        ///
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public override async Task LocalPublishAsync(object requestDto)
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
                await GetServiceGateway("LocalSendAsync", requestType.Name, responseType.Name).PublishAsync(obj).ConfigureAwait(false);
#if DEBUG
                LocalHit = true;
#endif
            }
        }
#if DEBUG
        public bool LocalHit { get; set; } = false;
#endif
        /// <summary>
        /// Actions a list of requests to publish to a service.
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        public override async Task LocalPublishAllAsync(object requestDtos)

        {
            var props = ((JsonElement)requestDtos).EnumerateObject().ToArray();
            var responseTypeJson = props.FirstOrDefault(a => a.Name == "ResponseType");
            var typeJson = props.FirstOrDefault(a => a.Name == "Type");
            var data = props.FirstOrDefault(a => a.Name == "Data");

            Type responseType = GetTypeByName(responseTypeJson.Value.ToString());
            Type requestType = GetTypeByName(typeJson.Value.ToString());
            if (responseType != null && requestType != null)
            {
                var bv = GetServiceGateway("LocalSendAsync", requestType.Name, responseType.Name);

                var obj = JsonSerializer.Deserialize(data.Value, requestType);
                var result = (Task)puba.Invoke(bv, new object?[] { obj, null });
                await result.ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Sends an item over the wire and gets a response.
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="requestDto"></param>
        /// <param name="token"></param>
        /// <returns>Task of TResponse</returns>
        public async Task<TResponse> SendAsync<TResponse, R>(object requestDto, CancellationToken token = default) where R : IReturn<TResponse>
        {
            return await connection.InvokeAsync<TResponse>("SendAsync", new { ResponseType = typeof(TResponse).FullName, Type = typeof(R).FullName, Data = requestDto }, token);
        }

        public async Task<IEnumerable<TResponse>> SendAllAsync<TResponse, R>(IEnumerable<R> requestDtos, CancellationToken token = default) where R : IReturn<TResponse>
        {
            string retfullName = typeof(TResponse).FullName;
            string typefullname = typeof(IEnumerable<R>).FullName;
            return await connection.InvokeAsync<IEnumerable<TResponse>>("SendAllAsync", new { ResponseType = retfullName, Type = typefullname, Data = requestDtos.ToArray() }, token);
        }

        /// <summary>
        /// Sends an item of the wire and waits for it to be published. No return is sent back.
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="requestDto"></param>
        /// <param name="token"></param>
        /// <returns>Task</returns>
        public async Task PublishAsync<TResponse, R>(object requestDto, CancellationToken token = default) where R : IReturn<TResponse>
        {
            string retfullName = typeof(TResponse).FullName;
            string typefullname = typeof(R).FullName;
            await connection.InvokeAsync<IEnumerable<TResponse>>("PublishAsync", new { ResponseType = retfullName, Type = typefullname, Data = requestDto }, token);
        }

        /// <summary>
        /// Sends a list of items over the wire and waits for them to be published. No return is sent back
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="requestDtos"></param>
        /// <param name="token"></param>
        /// <returns>Task</returns>
        public async Task PublishAllAsync<TResponse, R>(IEnumerable<R> requestDtos, CancellationToken token = default) where R : IReturn<TResponse>
        {
            string retfullName = typeof(TResponse).FullName;
            string typefullname = typeof(IEnumerable<R>).FullName;
            await connection.InvokeAsync<IEnumerable<TResponse>>("PublishAllAsync", new { ResponseType = retfullName, Type = typefullname, Data = requestDtos.ToArray() }, token);
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
            return parentFacade;
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

    public interface IConnectFacades
    {
        public void GetAllFacades(IConnectFacades[] connectFacades);
    }

    public interface IConnectFacadesBinary : IConnectFacades
    {
        public IConnectFacadesBinary GetParentFacade();

        public IConnectFacadesBinary GetLeftFacade();

        public void SetLeftFacade(IConnectFacadesBinary left);

        public void SetRightFacade(IConnectFacadesBinary right);


        public IConnectFacadesBinary GetRightFacade();
    }
}