using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace BlazorQueue
{
    public abstract class BlazorConnectionManager
    {
        private readonly HubConnection? connection;

        public bool Active => connection?.State == HubConnectionState.Connected;
        public HubConnectionState? State =>  connection?.State;
        public HubConnectionInfo? HubConnectionInfo { get; }


        public async Task Start()
        {
            if (connection == null) return;

            if (connection.State != HubConnectionState.Disconnected)
            {
                throw new Exception("Already connected or connecting");
            }
            await connection.StartAsync();
        }
        public async Task Stop()
        {
            if (connection == null) return;
            await connection.StopAsync();
        }
    }
}