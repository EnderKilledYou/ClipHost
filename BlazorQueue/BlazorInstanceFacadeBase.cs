 
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorQueue
{
    public abstract class BlazorInstanceFacadeBase 
    {
        protected readonly MethodInfo? mi;
        protected readonly MethodInfo? puba;
       
        public BlazorInstanceFacadeBase( ) 
        {
      
            mi = typeof(InProcessServiceGateway).GetMethod("SendAllAsync");
            puba = typeof(InProcessServiceGateway).GetMethod("PublishAllAsync");

        }

      

    

     

       
    }
}