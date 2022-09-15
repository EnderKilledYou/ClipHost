using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace BlazorQueue.ServiceInterface
{
    public abstract class BlazorInstanceFacadeBaseBase
    {
        protected HubConnection connection;

        

        protected BlazorInstanceFacadeBaseBase(HubConnectionInfo parentConnectionInfo, bool isRoot=false)
        {
            ParentConnection = parentConnectionInfo;
            IsRoot = isRoot;
            if (ParentConnection == null)
                return;
            
            Uri url = new Uri(ParentConnection.HostUrl + ParentConnection.HubName);
            connection = new HubConnectionBuilder()
                         .WithUrl(url, options =>
                         {
                             options.AccessTokenProvider = ParentConnection.accessTokenProvider;
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

        public bool Active => connection.State == HubConnectionState.Connected;
        public HubConnectionState State => connection.State;
        public HubConnectionInfo ParentConnection { get; }
        public bool IsRoot { get; }

        public virtual async Task Start()
        {
            if (connection == null) return;
            if (connection.State != HubConnectionState.Disconnected)
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

        protected virtual async Task Connection_Closed(Exception arg)
        {
        }

        protected virtual async Task Connection_Reconnected(string arg)
        {
        }

        protected virtual async Task Connection_Reconnecting(Exception arg)
        {
        }
    }
}