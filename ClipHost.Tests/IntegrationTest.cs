using Funq;
using ServiceStack;
using NUnit.Framework;
using ClipHost.ServiceInterface;
using ClipHost.ServiceModel;
using Microsoft.Extensions.DependencyInjection;
using BlazorQueue.ServiceInterface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http.Connections;

namespace ClipHost.Tests;

public class IntegrationTest
{
    const string BaseUri = "http://localhost:2000/";
    private readonly WebApplication appHost;
 

    public IntegrationTest()
    {

        var builder = WebApplication.CreateBuilder( );
        builder.Services.AddSignalR(e => e.EnableDetailedErrors = true) ;
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
            app.UseHttpsRedirection();
        }
        app.Urls.Add(BaseUri);
        
        app.UseServiceStack(new AppHost());
        app.MapHub<ServiceGatewayHub>("/ServiceGatewayHub", options =>
        {
            options.Transports =
                HttpTransportType.WebSockets |
                HttpTransportType.LongPolling;
        });
        app.Start();
        appHost = app;

    }

    [OneTimeTearDown]
    public void OneTimeTearDown() => appHost.StopAsync();

    public BlazorInstanceFacade CreateClient() => new BlazorInstanceFacade(BaseUri, "ServiceGatewayHub","");
     
    [Test]
    public async Task Can_call_Hello_Service()
    {
        var client = CreateClient();
        client.RegisterType<HelloResponse,Hello>();
        await client.Start();
        Assert.That(client.Active, Is.True);
        
        var response = await client.SendAsync<HelloResponse>(new Hello() { Name = "World" });

        Assert.That(response.Result, Is.EqualTo("Hello, World!"));
    }
}