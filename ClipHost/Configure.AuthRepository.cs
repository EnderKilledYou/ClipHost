using ServiceStack;
using ServiceStack.Web;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using TwitchLib.Api.Core.Models.Undocumented.TwitchPrimeOffers;
using ClipHost.ServiceModel.Types;

[assembly: HostingStartup(typeof(ClipHost.ConfigureAuthRepository))]

namespace ClipHost
{
    // Custom User Table with extended Metadata properties
    public class AppUser : UserAuth
    {
        public string? ProfileUrl { get; set; }
        public string? LastLoginIp { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }

    public class AppUserAuthEvents : AuthEvents
    {
        public override void OnAuthenticated(IRequest req, IAuthSession session, IServiceBase authService,
            IAuthTokens tokens, Dictionary<string, string> authInfo)
        {
            var authRepo = HostContext.AppHost.GetAuthRepository(req);
            using (authRepo as IDisposable)
            {
                var userAuth = (AppUser)authRepo.GetUserAuth(session.UserAuthId);
                userAuth.ProfileUrl = session.GetProfileUrl();
                userAuth.LastLoginIp = req.UserHostAddress;
                userAuth.LastLoginDate = DateTime.UtcNow;
                authRepo.SaveUserAuth(userAuth);
            }



            using var Db = HostContext.Resolve<IDbConnectionFactory>().Open();
            var streamerExists = Db.Single<Streamer>(a => a.Name == tokens.UserName);
            if (streamerExists == null)
            {
                streamerExists = new Streamer()
                {
                    Name = tokens.UserName
                };
                streamerExists.Id = (int)Db.Insert(streamerExists, true);
            }

            var tokenExists = Db.Single<TwitchOauthTokens>(a => a.StreamerLogin == tokens.UserName);
            var tmp = tokens.ConvertTo<TwitchOauthTokens>();

            


            if (tokenExists != null)
            {
                tmp.Id = tokenExists.Id;
                tmp.StreamerLogin = tmp.UserName;
                Db.Update(tmp);
            }
            else
            {
                tmp.StreamerLogin = tmp.UserName;
                Db.Insert(tmp);
            }

            if (!streamerExists.Enabled)
            {
                return;
            }

            var commandCenter = HostContext.Resolve<CommandCenter>();
            var ccExists = Db.Single<StreamerCommandCenter>(a => a.StreamerId == streamerExists.Id);
            if (ccExists == null)
            {
                Db.Insert(new StreamerCommandCenter()
                {
                    CommandCenterId = commandCenter.Id,
                    StreamerId = streamerExists.Id
                });
            }
        }
    }

    public class ConfigureAuthRepository : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder) => builder
            .ConfigureServices(services => services.AddSingleton<IAuthRepository>(c =>
                new InMemoryAuthRepository<AppUser, UserAuthDetails>()))
            .ConfigureAppHost(appHost =>
            {
                var authRepo = appHost.Resolve<IAuthRepository>();
                authRepo.InitSchema();
                CreateUser(authRepo, "admin@email.com", "Admin User", "passsa", roles: new[] { RoleNames.Admin });
            }, afterConfigure: appHost =>
                appHost.AssertPlugin<AuthFeature>().AuthEvents.Add(new AppUserAuthEvents()));

        // Add initial Users to the configured Auth Repository
        public void CreateUser(IAuthRepository authRepo, string email, string name, string password, string[] roles)
        {
            if (authRepo.GetUserAuthByUserName(email) == null)
            {
                var newAdmin = new AppUser { Email = email, DisplayName = name };
                var user = authRepo.CreateUserAuth(newAdmin, password);
                authRepo.AssignRoles(user, roles);
            }
        }
    }
}
