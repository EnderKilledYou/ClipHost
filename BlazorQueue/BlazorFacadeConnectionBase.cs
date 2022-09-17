using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace BlazorQueue
{
    public interface IConnectToBlazor
    {
        public Task Start();

        public Task Stop();
    }
    public abstract class BlazorFacadeConnectionBase  : BlazorInstanceTransmitter, IConnectToBlazor
    {
        protected HubConnection? connection;
        private readonly IDisposable? LocalSendAsyncConnection;
        private readonly IDisposable? LocalSendAllAsyncConnection;
        private readonly IDisposable? LocalPublishAsyncConnection;
        private readonly IDisposable? LocalPublishAllAsyncConnection;
        public void OnDeserialized()
        {
            Start().Wait();
        }


        public ValueTask DisposeAsync()
        {
            LocalPublishAllAsyncConnection?.Dispose();
            LocalPublishAsyncConnection?.Dispose();
            LocalSendAllAsyncConnection?.Dispose();
            LocalSendAsyncConnection?.Dispose();
            if (connection != null)
            {
                var val = connection.DisposeAsync();
                GC.SuppressFinalize(this);

                return val;
            }
            return default;
        }
        public abstract Task LocalSendAsync(MetaPacket requestDto);

        public abstract Task LocalSendAllAsync(MetaPacket requestDto);
        public abstract Task LocalPublishAsync(MetaPacket requestDto);
        public abstract Task LocalPublishAllAsync(MetaPacket requestDto);

        protected BlazorFacadeConnectionBase(HubConnectionInfo? parentConnectionInfo, bool isRoot = false) : base(parentConnectionInfo, isRoot)
        {
            ParentConnection = parentConnectionInfo;
            IsRoot = isRoot;
            if (ParentConnection == null)
                return;

            Uri url = new Uri(ParentConnection.HostUrl + ParentConnection.HubName);
            connection = new HubConnectionBuilder()
                         .WithUrl(url, options =>
                         {
                             options.AccessTokenProvider = ParentConnection.AccessTokenProvider;
                         }).AddJsonProtocol(options =>
                         {
                             options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                         })
                    .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.Zero, TimeSpan.FromSeconds(1) })
                .Build();
            connection.Closed += Connection_Closed;
            connection.Reconnected += Connection_Reconnected;
            connection.Reconnecting += Connection_Reconnecting;
            LocalSendAsyncConnection = connection.On<MetaPacket>("LocalSendAsync", LocalSendAsync);
            LocalSendAllAsyncConnection = connection.On<MetaPacket>("LocalSendAllAsync", LocalSendAllAsync);
            LocalPublishAsyncConnection = connection.On<MetaPacket>("LocalPublishAsync", LocalPublishAsync);
            LocalPublishAllAsyncConnection = connection.On<MetaPacket>("LocalPublishAllAsync", LocalPublishAllAsync);

        }

        public bool? Active => connection?.State == HubConnectionState.Connected;
        public HubConnectionState? State => connection?.State;
        public HubConnectionInfo? ParentConnection { get; }
        public bool IsRoot { get; }

        public virtual async Task Start()
        {

            if (connection?.State != HubConnectionState.Disconnected)
            {
                throw new Exception("Already connected or connecting");
            }
            await connection.StartAsync();
        }
        public virtual async Task Stop()
        {
            if (connection == null) return;
            await connection.StopAsync();
        }

        protected virtual async Task Connection_Closed(Exception? arg)
        {
            
        }

        protected virtual async Task Connection_Reconnected(string? arg)
        {
        }

        protected virtual async Task Connection_Reconnecting(Exception? arg)
        {
        }
    }
}