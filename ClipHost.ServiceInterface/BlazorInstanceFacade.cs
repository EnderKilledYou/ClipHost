using ClipHost.ServiceModel.CreateCommandCenterModels;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using ServiceStack.Testing;
using System;
using System.Collections.Generic;
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
            var val = connection.DisposeAsync();
            GC.SuppressFinalize(this);
            return val;
        }

        public BlazorInstanceFacade(string HostUrl, string HubName, string token)
        {
            //   serviceGatewayAsync = HostContext.Resolve<IServiceGatewayAsync>();
            this.HostUrl = HostUrl;
            this.HubName = HubName;
            Token = token;
   

            Uri url = new Uri(HostUrl + HubName);
            connection = new HubConnectionBuilder()
                         .WithUrl(url, options =>
                         {
                             options.Headers.Add("Authorization", "bearer " + Token);
                         }).AddJsonProtocol(options => {
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

            if ( connection.State != HubConnectionState.Disconnected)
            {
                throw new Exception("Already connected or connecting");
            }
            await connection.StartAsync();
      

        }
        public void RegisterType<TResponse,IReturn>()
        {
            LocalSendAsyncConnection = connection.On<IReturn<TResponse>>("LocalSendAsync", LocalSendAsync);
            LocalSendAllAsyncConnection = connection.On<List<IReturn<TResponse>>>("LocalSendAllAsync", LocalSendAllAsync);
            LocalPublishAsyncConnection = connection.On<IReturn<TResponse>>("LocalPublishAsync", LocalPublishAsync);
            LocalPublishAllAsyncConnection = connection.On<List<IReturn<TResponse>>>("LocalPublishAllAsync", LocalPublishAllAsync);
        }

        private async Task Connection_Reconnecting(Exception arg)
        {
            
        }

        private  async Task Connection_Reconnected(string arg)
        {
            
        }
        private IServiceGateway GetServiceGateway()
        {
            var request = new MockHttpRequest();

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
            return await GetServiceGateway().SendAsync(requestDto);
        }

        public async Task<List<TResponse>> LocalSendAllAsync<TResponse>(IEnumerable<IReturn<TResponse>> requestDtos)
        {
            return await GetServiceGateway().SendAllAsync(requestDtos);
        }

        public async Task LocalPublishAsync<TResponse>(IReturn<TResponse> requestDto)
        {
            await GetServiceGateway().PublishAsync(requestDto);
        }

        public async Task LocalPublishAllAsync(IEnumerable<object> requestDtos)
        {
            await GetServiceGateway().PublishAllAsync(requestDtos);
        }

        public async Task<TResponse> SendAsync<TResponse>(object requestDto, CancellationToken token = default)
        {
            return await connection.InvokeAsync<TResponse>("SendAsync",new {ResponseType = typeof(TResponse).FullName,   Type = requestDto.GetType().FullName, Data = requestDto}, token);
        }

        public async Task<List<TResponse>> SendAllAsync<TResponse>(IEnumerable<object> requestDtos, CancellationToken token = default)
        {
            return await connection.InvokeAsync<List<TResponse>>("SendAllAsync", requestDtos, token);
        }

        public async Task PublishAsync<TResponse>(IReturn<TResponse> requestDto, CancellationToken token = default)
        {
            await connection.SendAsync("PublishAsync", requestDto, token);
        }

        public async Task PublishAllAsync<TResponse>(IEnumerable<IReturn<TResponse>> requestDtos, CancellationToken token = default)
        {
            await connection.SendAsync("PublishAllAsync", requestDtos, token);
        }
    }
}