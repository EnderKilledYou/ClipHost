using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace BlazorQueue.ServiceInterface
{
    public abstract class BlazorConnectionManager
    {
        private HubConnection connection;

        public bool Active => connection.State == HubConnectionState.Connected;
        public HubConnectionState State => connection.State;
        public HubConnectionInfo HubConnectionInfo { get; }


        public async Task Start()
        {
            if (connection.State != HubConnectionState.Disconnected)
            {
                throw new Exception("Already connected or connecting");
            }
            await connection.StartAsync();
        }
        public async Task Stop()
        {
            await connection.StopAsync();
        }
    }
}