using ClipHost.ServiceInterface;
using ClipHost.ServiceModel;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System.Data;

[assembly: HostingStartup(typeof(ConfigureDb))]

namespace ClipHost
{

    // Database can be created with "dotnet run --AppTasks=migrate"
    public class ConfigureDb : IHostingStartup
    {


        public IDbConnectionFactory factory(IServiceCollection services, IConfiguration configuration)
        {

            string dbuser = configuration["dbuser"];
            string dbpassword = configuration["dbpassword"];
            string dbserver = configuration["dbserver"];
            string dbname = configuration["dbname"];

            var connectionString = $"Host={dbserver};Database={dbname};UserId={dbuser};Password={dbpassword}";
            var dbfact = new OrmLiteConnectionFactory(connectionString, PostgreSqlDialect.Provider);
            using IDbConnection db = dbfact.Open();
            TableUp.DoAllTableUps(db);
            var cc = db.Single<CommandCenter>(a => a.Id == 1);
            if (cc == null)
            {
                db.Insert(new CommandCenter() { MaxStreamers = 4, Name = "LocalHost", StreamerCount = 0 }, true);
                cc = db.Single<CommandCenter>(a => a.Id == 1);

            }
            if (cc != null)
            {
                services.AddSingleton(cc);
            }

            //return new CreateCommandCenterResponse { Id = id };
            return dbfact;
        }

        public void Configure(IWebHostBuilder builder) => builder
            .ConfigureServices((context, services) =>
                        services.AddSingleton<IDbConnectionFactory>(factory(services, context.Configuration)));
    }
}