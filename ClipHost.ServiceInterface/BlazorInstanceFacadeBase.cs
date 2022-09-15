using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorQueue.ServiceInterface
{
    public abstract class BlazorInstanceFacadeBase : BlazorInstanceFacadeBaseBase
    {
        protected readonly MethodInfo mi;
        protected readonly MethodInfo puba;
        private IDisposable LocalSendAsyncConnection;
        private IDisposable LocalSendAllAsyncConnection;
        private IDisposable LocalPublishAsyncConnection;
        private IDisposable LocalPublishAllAsyncConnection;
        public BlazorInstanceFacadeBase(HubConnectionInfo parentConnectionInfo,bool isRoot = false) : base(parentConnectionInfo, isRoot)
        {
      
            mi = typeof(InProcessServiceGateway).GetMethod("SendAllAsync");
            puba = typeof(InProcessServiceGateway).GetMethod("PublishAllAsync");
        }

        public ValueTask DisposeAsync()
        {
            LocalPublishAllAsyncConnection.Dispose();
            LocalPublishAsyncConnection.Dispose();
            LocalSendAllAsyncConnection.Dispose();
            LocalSendAsyncConnection.Dispose();
            if (connection != null)
            {
                var val = connection.DisposeAsync();
                GC.SuppressFinalize(this);

                return val;
            }
            return default;
        }

        public void OnDeserialized()
        {
            Start().Wait();
        }

        public abstract Task LocalSendAsync(object requestDto);

        public abstract Task LocalSendAllAsync(object requestDto);
        public abstract Task LocalPublishAsync(object requestDto);
        public abstract Task LocalPublishAllAsync(object requestDto);

        public void RegisterType<TResponse, R>() where R : IReturn<TResponse>
        {
            LocalSendAsyncConnection = connection.On<object>("LocalSendAsync", LocalSendAsync);
            LocalSendAllAsyncConnection = connection.On<object>("LocalSendAllAsync", LocalSendAllAsync);
            LocalPublishAsyncConnection = connection.On<object>("LocalPublishAsync", LocalPublishAsync);
            LocalPublishAllAsyncConnection = connection.On<object>("LocalPublishAllAsync", LocalPublishAllAsync);
        }
    }
}