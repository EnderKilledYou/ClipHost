using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using ServiceStack.Testing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorQueue.ServiceInterface
{
    public class BlazorInstanceFacade : IAsyncDisposable, IJsonOnDeserialized
    {
        public string HostUrl { get; }
        public string HubName { get; }
        public string Token { get; }
        public bool Active => connection.State == HubConnectionState.Connected;
        public HubConnectionState State => connection.State;
        private HubConnection connection;
        private IDisposable LocalSendAsyncConnection;
        private IDisposable LocalSendAllAsyncConnection;
        private IDisposable LocalPublishAsyncConnection;
        private IDisposable LocalPublishAllAsyncConnection;

        public ValueTask DisposeAsync()
        {
            LocalPublishAllAsyncConnection.Dispose();
            LocalPublishAsyncConnection.Dispose();
            LocalSendAllAsyncConnection.Dispose();
            LocalSendAsyncConnection.Dispose();
            var val = connection.DisposeAsync();
            GC.SuppressFinalize(this);

            return val;
        }

        public BlazorInstanceFacade(string HostUrl, string HubName, Func<Task<string>> accessTokenProvider)
        {
            //   serviceGatewayAsync = HostContext.Resolve<IServiceGatewayAsync>();
            this.HostUrl = HostUrl;
            this.HubName = HubName;
           

            Uri url = new Uri(HostUrl + HubName);
            connection = new HubConnectionBuilder()
                         .WithUrl(url, options =>
                         {
                             options.AccessTokenProvider = accessTokenProvider;
                         }).AddJsonProtocol(options =>
                         {
                             options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                         })
                    .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.Zero, TimeSpan.FromSeconds(1) })
                .Build();
            connection.Closed += Connection_Closed;
            connection.Reconnected += Connection_Reconnected;
            connection.Reconnecting += Connection_Reconnecting;
        }
 

        public async Task Start()
        {
            if (connection.State != HubConnectionState.Disconnected)
            {
                throw new Exception("Already connected or connecting");
            }
            await connection.StartAsync();
        }

        public void RegisterType<TResponse, R>() where R : IReturn<TResponse>
        {
            LocalSendAsyncConnection = connection.On<IReturn<TResponse>>("LocalSendAsync", LocalSendAsync);
            LocalSendAllAsyncConnection = connection.On<List<R>>("LocalSendAllAsync", LocalSendAllAsync<TResponse,R>);
            LocalPublishAsyncConnection = connection.On<IReturn<TResponse>>("LocalPublishAsync", LocalPublishAsync);
            LocalPublishAllAsyncConnection = connection.On<List<IReturn<TResponse>>>("LocalPublishAllAsync", LocalPublishAllAsync);
        }

        private async Task Connection_Reconnecting(Exception arg)
        {
        }

        private async Task Connection_Reconnected(string arg)
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

        private async Task Connection_Closed(Exception arg)
        {
        }

        public async Task Stop()
        {
            await connection.StopAsync();
        }

        public void OnDeserialized()
        {
            Start().Wait();
        }

        public async Task<TResponse> LocalSendAsync<TResponse>(IReturn<TResponse> requestDto) 
        {
            return await GetServiceGateway("LocalSendAsync", requestDto.GetType().Name, typeof(TResponse).Name).SendAsync(requestDto);
        }

        public async Task<List<TResponse>> LocalSendAllAsync<TResponse, T>(IEnumerable<T> requestDtos) where T : IReturn<TResponse>
        {
            return await GetServiceGateway("LocalSendAllAsync", typeof(T).Name, typeof(TResponse).Name).SendAllAsync((IEnumerable<IReturn<TResponse>>)requestDtos);
        }

        public async Task LocalPublishAsync<TResponse>(IReturn<TResponse> requestDto)
        {
            await GetServiceGateway("LocalPublishAsync", requestDto.GetType().Name, typeof(TResponse).Name).PublishAsync(requestDto);
        }

        public async Task LocalPublishAllAsync(IEnumerable<object> requestDtos)
        {
            await GetServiceGateway("LocalPublishAllAsync", "", "").PublishAllAsync(requestDtos);
        }

        public async Task<TResponse> SendAsync<TResponse,R>(object requestDto, CancellationToken token = default) where R : IReturn<TResponse>
        {
            return await connection.InvokeAsync<TResponse>("SendAsync", new { ResponseType = typeof(TResponse).FullName, Type = typeof(R).FullName, Data = requestDto }, token);
        }

        public async Task<IEnumerable<TResponse>> SendAllAsync<TResponse,R>(IEnumerable<R>   requestDtos, CancellationToken token = default) where R:IReturn<TResponse>
        {
            string retfullName = typeof(TResponse).FullName;
            string typefullname = typeof(IEnumerable<R>).FullName;
            return await connection.InvokeAsync<IEnumerable<TResponse>>("SendAllAsync", new { ResponseType = retfullName, Type = typefullname, Data = requestDtos.ToArray() }, token);
        }

        public async Task PublishAsync<TResponse,R>(object requestDto, CancellationToken token = default) where R : IReturn<TResponse>
        {
            string retfullName = typeof(TResponse).FullName;
            string typefullname = typeof(R).FullName;
            await connection.InvokeAsync<IEnumerable<TResponse>>("PublishAsync", new { ResponseType = retfullName, Type = typefullname, Data = requestDto }, token);
        }

        public async Task PublishAllAsync<TResponse, R>(IEnumerable<R> requestDtos, CancellationToken token = default) where R : IReturn<TResponse>
        {
            string retfullName = typeof(TResponse).FullName;
            string typefullname = typeof(IEnumerable<R>).FullName;
            await connection.InvokeAsync<IEnumerable<TResponse>>("PublishAllAsync", new { ResponseType = retfullName, Type = typefullname, Data = requestDtos.ToArray() }, token);
        }
    }
}