using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
 

namespace BlazorQueue
{
    public interface IConnectToBlazor
    {
        public Task Start();

        public Task Stop();
    }
    public abstract class BlazorFacadeConnectionBase  : BlazorInstanceTransmitter, IConnectToBlazor
    {
        
        private readonly IDisposable? _localSendAsyncConnection;
        private readonly IDisposable? _localSendAllAsyncConnection;
        private readonly IDisposable? _localPublishAsyncConnection;
        private readonly IDisposable? _localPublishAllAsyncConnection;
        public void OnDeserialized()
        {
            Start().Wait();
        }


        public ValueTask DisposeAsync()
        {
            _localPublishAllAsyncConnection?.Dispose();
            _localPublishAsyncConnection?.Dispose();
            _localSendAllAsyncConnection?.Dispose();
            _localSendAsyncConnection?.Dispose();
            if (Connection == null) return default;
            var val = Connection.DisposeAsync();
            GC.SuppressFinalize(this);

            return val;
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
            Connection = new HubConnectionBuilder()
                         .WithUrl(url, options =>
                         {
                             options.AccessTokenProvider = ParentConnection.AccessTokenProvider;
                         }).AddJsonProtocol(options =>
                         {
                             options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                         })
                    .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.Zero, TimeSpan.FromSeconds(1) })
                .Build();
            Connection.Closed += Connection_Closed;
            Connection.Reconnected += Connection_Reconnected;
            Connection.Reconnecting += Connection_Reconnecting;
            _localSendAsyncConnection = Connection.On<MetaPacket>("LocalSendAsync", LocalSendAsync);
            _localSendAllAsyncConnection = Connection.On<MetaPacket>("LocalSendAllAsync", LocalSendAllAsync);
            _localPublishAsyncConnection = Connection.On<MetaPacket>("LocalPublishAsync", LocalPublishAsync);
            _localPublishAllAsyncConnection = Connection.On<MetaPacket>("LocalPublishAllAsync", LocalPublishAllAsync);

        }

        public bool? Active => Connection?.State == HubConnectionState.Connected;
        public HubConnectionState? State => Connection?.State;
        protected HubConnectionInfo? ParentConnection { get; }
        public bool IsRoot { get; }


        protected abstract Task Connection_Closed(Exception? arg);

        protected abstract   Task Connection_Reconnected(string? arg);

        protected abstract Task Connection_Reconnecting(Exception? arg);
    }
}