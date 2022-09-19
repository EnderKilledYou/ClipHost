using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using ServiceStack;
using System;

namespace BlazorQueue
{
    public class BlazorInstanceTransmitter : BlazorInstanceFacadeBase
    {
        protected readonly HubConnection? connection;
        public BlazorInstanceTransmitter(HubConnectionInfo? parentConnectionInfo, bool isRoot = false)
        {
            if (parentConnectionInfo == null) return;
            Uri url = new Uri(parentConnectionInfo.HostUrl + parentConnectionInfo.HubName);
            connection = new HubConnectionBuilder()
                         .WithUrl(url, options =>
                         {
                             options.AccessTokenProvider = parentConnectionInfo.AccessTokenProvider;
                         }) 
                    .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.Zero, TimeSpan.FromSeconds(1) })
                .Build();
            connection.Closed += Connection_Closed;
            connection.Reconnected += Connection_Reconnected;
            connection.Reconnecting += Connection_Reconnecting;
        }

        private async Task Connection_Reconnecting(Exception? arg)
        {
           
        }

        private async Task Connection_Reconnected(string? arg)
        {
       
        }

        private async Task Connection_Closed(Exception? arg)
        {
            
        }

        public async Task Start()
        {
            if (connection == null) return;
            await connection.StartAsync();
            await connection!.SendAsync("Register", Environment.ProcessId);
        }

        public async Task Stop()
        {
            if (connection == null) return;
            await connection.StopAsync();
            
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
            if (connection == null) return;
            await connection.InvokeAsync<IEnumerable<TResponse>>("PublishAllAsync", requestDtos.ToMetaPacket<TResponse, R[]>(), token);
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
            if (connection == null) return;
            await connection.InvokeAsync<IEnumerable<TResponse>>("PublishAsync", requestDto.ToMetaPacket<TResponse, R>(), token);
        }

        public async Task<IEnumerable<TResponse>?> SendAllAsync<TResponse, R>(IEnumerable<R> requestDtos, CancellationToken token = default) where R : IReturn<TResponse>
        {
            if (connection == null) return default;
            return await connection.InvokeAsync<IEnumerable<TResponse>>("SendAllAsync", requestDtos.ToMetaPacket<TResponse, R[]>(), token);
        }

        /// <summary>
        /// Sends an item over the wire and gets a response.
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="requestDto"></param>
        /// <param name="token"></param>
        /// <returns>Task of TResponse</returns>
        public async Task<TResponse?> SendAsync<TResponse, R>(object requestDto, CancellationToken token = default) where R : IReturn<TResponse>
        {
            if (connection == null) return default;
            return await connection.InvokeAsync<TResponse>("SendAsync", requestDto.ToMetaPacket<TResponse, R>(), token);
        }
    }
}