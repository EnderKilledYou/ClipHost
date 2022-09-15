using ClipHost.ServiceInterface;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System.Data;

[assembly: HostingStartup(typeof(ConfigureDb))]

namespace ClipHost
{

    // Database can be created with "dotnet run --AppTasks=migrate"
    public class ConfigureDb : IHostingStartup
    {
    

        public IDbConnectionFactory factory(IConfiguration configuration)
        {
            
            string dbuser = configuration["dbuser"];
            string dbpassword = configuration["dbpassword"];
            string dbserver = configuration["dbserver"];
            string dbname = configuration["dbname"];

            var connectionString = $"Host={dbserver};Database={dbname};UserId={dbuser};Password={dbpassword}";
            var dbfact = new OrmLiteConnectionFactory(connectionString, PostgreSqlDialect.Provider);
            using IDbConnection db = dbfact.Open();
            TableUp.DoAllTableUps(db);
            return dbfact;
        }

        public void Configure(IWebHostBuilder builder) => builder
            .ConfigureServices((context, services) =>
                        services.AddSingleton<IDbConnectionFactory>(factory(context.Configuration)));
    }
}