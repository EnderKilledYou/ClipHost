 
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
        protected readonly MethodInfo? Mi;
        protected readonly MethodInfo? Puba;
       
        public BlazorInstanceFacadeBase( ) 
        {
      
            Mi = typeof(InProcessServiceGateway).GetMethod("SendAllAsync");
            Puba = typeof(InProcessServiceGateway).GetMethod("PublishAllAsync");

        }

      

    

     

       
    }
}