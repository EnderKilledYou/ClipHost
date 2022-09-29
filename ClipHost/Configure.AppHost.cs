using Funq;
using ServiceStack;
using ClipHost.ServiceInterface;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Configuration;
using System.Data;
using ServiceStack.Validation;
using System.Threading.Channels;
using static ServiceStack.Diagnostics.Events;
using ClipHost.ServiceModel;
using System;
using ServiceStack.Auth;
using System.Net;
using BlazorQueue;
using ServiceStack.Text;

[assembly: HostingStartup(typeof(ClipHost.AppHost))]

namespace ClipHost;

public partial class AppHost : AppHostBase, IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices(services =>
        {
            // Configure ASP.NET Core IOC Dependencies
        });

    public AppHost() : base("ClipHost", typeof(MyServices).Assembly) { }

 
    public override void Configure(Container container)
    {

        Plugins.Add(new SharpPagesFeature
        {
            EnableSpaFallback = true
        });
        this.
        SetConfig(new HostConfig
        {
            AddRedirectParamsToQueryString = true,
            UseCamelCase = false,

        });
        Plugins.Add(new ValidationFeature());
        Plugins.Add(new ServerEventsFeature()
        {
            OnCreated = (sub, req) =>
            {
                var session = req.GetSession();
                if (!session.IsAuthenticated) return;
                //sub.Meta["Nickname"] = session.Nickname;           // channel subscribers
                //sub.ConnectArgs["Email"] = session.Email;          // client subscriber 
                //sub.ServerArgs["PostalCode"] = session.PostalCode; // server
            },
            LimitToAuthenticatedUsers = false,
        });
        JsConfig.TextCase = TextCase.PascalCase;

        container.AddSingleton<TagMangager>();
        var sef = new ServerEventsFeature
        {
            OnConnect = (sub, req) =>
            {


            },
            OnSubscribeAsync = async (sub) =>
            {

            }
        };
        Plugins.Add(sef);






        ServicePointManager.DefaultConnectionLimit = 1000;

    }


    public class ParentReceiver : ServerEventReceiver

    {

        public override void NoSuchMethod(string selector, object message)
        {
            base.NoSuchMethod(selector, message);
        }
    }
    class ParentServerConnector : BackgroundService
    {
        private readonly ParentServer parentServer;
        private readonly ILogger logger;

        public ParentServerConnector(ParentServer parentServer, ILogger logger)
        {
            this.parentServer = parentServer;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var guid = new Guid();
            parentServer.Authenticate(new Authenticate
            {
                provider = CredentialsAuthProvider.Name,
                UserName = "user",
                Password = "pass",
                RememberMe = true,
            });
            var parent = parentServer.Start();

            if (parent == null)
            {
                logger.LogError("Could not connect to parent");
                //todo: exception
                throw new Exception("Could not connect");

            }

            parentServer.OnCommand += onCommand;
            parentServer.OnConnect += onConnect;
            parentServer.OnException += OnException;
            parentServer.OnJoin += OnJoin;
            parentServer.OnLeave += OnLeave;
            parentServer.OnMessage += OnMessage;
            parentServer.OnLeave += OnLeave;
            parentServer.OnUpdate += OnUpdate;
            parentServer.ServiceClient.Post(new Hello());
            parentServer.RegisterReceiver<ParentReceiver>();



        }

        private void OnUpdate(ServerEventUpdate obj)
        {
            throw new NotImplementedException();
        }

        private void OnMessage(ServerEventMessage obj)
        {

            throw new NotImplementedException();
        }

        private void OnLeave(ServerEventLeave obj)
        {
            throw new NotImplementedException();
        }

        private void OnJoin(ServerEventJoin obj)
        {
            throw new NotImplementedException();
        }

        private void OnException(Exception obj)
        {
            throw new NotImplementedException();
        }

        private void onConnect(ServerEventConnect obj)
        {
            throw new NotImplementedException();
        }

        private void onCommand(ServerEventMessage obj)
        {
            throw new NotImplementedException();
        }
    }
    class TagMangager
    {
        public List<string> Tags { get; set; } = new();
    }
    class ParentServer : ServerEventsClient
    {
        public ParentServer(string baseUri, params string[] channels) : base(baseUri, channels)
        {
        }
    }


}
