using BlazorQueue; 
using ClipHost.ServiceModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using ServiceStack;
using System;
using System.Threading.Tasks;
using static ServiceStack.Diagnostics.Events;

namespace ClipHost.Tests;

public class IntegrationTest
{
    private const string BaseUri = "http://localhost:2000/";

    private const string BaseUri2 = "http://localhost:2001/";
    private WebApplication app2;
    private WebApplication[] apps;
    public IntegrationTest()
    {
        apps = new[] { MakeApp(BaseUri) };
    }
    WebApplication MakeApp(string baseUri)
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddSignalR(e => e.EnableDetailedErrors = true).AddJsonProtocol(options => {
            options.PayloadSerializerOptions.PropertyNamingPolicy = null;
        }); 
        var app = builder.Build();
         
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error"); 
            app.UseHsts();
            app.UseHttpsRedirection();
        }
        app.Urls.Add(baseUri);

        app.UseServiceStack(new AppHost());
        app.MapHub < BlazorHub>("/BlazorHub", options =>
        {
            options.Transports =
                HttpTransportType.WebSockets |
                HttpTransportType.LongPolling;
        });
        app.Start();
        
        return app;
    }
    

    public async Task<string> AccessTokenProvider()
    {
        return "";
        //try
        //{
        //    var jsonClient = new JsonServiceClient(BaseUri);
        //    var resp = await jsonClient.PostAsync(new Authenticate()
        //    {
        //        UserName = "admin@gmail.com",
        //        Password = "passsa"
        //    });
        //    return resp.BearerToken;
        //}
        //catch (Exception ex)
        //{
        //    return "";
        //    //throw fix later
        //}
    }
    [OneTimeTearDown]
    public void TD()
    {
        foreach(var app in apps)
        {
            app.StopAsync();
        }
    }
 
    public BlazorInstanceFacade CreateDefaultclient()
    {
        var info = new HubConnectionInfo(BaseUri, "BlazorHub", AccessTokenProvider);
 

        BlazorInstanceFacade parent = new(info);
        return  new BlazorInstanceFacade(parent);
      
    } 

    [Test]
    public async Task Can_call_Hello_Service()
    {
        
        var client = CreateDefaultclient();
         
        await client.Start();
        Assert.That(client.Active, Is.True);

        var response = await client.SendAsync<HelloResponse, Hello>(new Hello() { Name = "World" });

        Assert.That(response.Result, Is.EqualTo("Hello, World!"));
    }


    [Test]
    public async Task Can_call_several_Hello_Service_with_client_response()
    {
        var client = CreateDefaultclient(); 
        await client.Start();
        Assert.That(client.Active, Is.True);

        var response = await client.SendAllAsync<HelloTestResponse, HelloTest>(new HelloTest[] { new HelloTest() { Name = "World" } });

        HelloTestResponse helloResponse = response.FirstNonDefault();
     
        Assert.That(helloResponse, Is.Not.Null);
        Assert.That(helloResponse.Result, Is.EqualTo("Hello, World!"));
    }
    [Test]
    public async Task Can_call_several_Hello_Service()
    {
        var client = CreateDefaultclient(); 
        await client.Start();
        Assert.That(client.Active, Is.True);

        var response = await client.SendAllAsync<HelloResponse, Hello>(new Hello[] { new Hello() { Name = "World" } });

        HelloResponse helloResponse = response.FirstNonDefault();
        Assert.That(helloResponse, Is.Not.Null);
        Assert.That(helloResponse.Result, Is.EqualTo("Hello, World!"));
    }

    [Test]
    public async Task Can_call_faf_all_Hello_Service()
    {
        var client = CreateDefaultclient(); 
        await client.Start();
        Assert.That(client.Active, Is.True);

        await client.PublishAllAsync<HelloResponse, Hello>(new Hello[] { new Hello() { Name = "World" } });
    }

    [Test]
    public async Task Can_call_faf_Hello_Service()
    {
        var client = CreateDefaultclient(); 
        await client.Start();
        Assert.That(client.Active, Is.True);

        await client.PublishAsync<HelloResponse, Hello>(new Hello() { Name = "World" });
    }
}