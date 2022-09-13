using ClipHost.ServiceInterface;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System.Data;

[assembly: HostingStartup(typeof(ClipHost.ConfigureDb))]

namespace ClipHost;

// Database can be created with "dotnet run --AppTasks=migrate"
public class ConfigureDb : IHostingStartup
{
    public ConfigureDb(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public IDbConnectionFactory factory()
    {
        string dbuser = Configuration["dbuser"];
        string dbpassword = Configuration["dbpassword"];
        string dbserver = Configuration["dbserver"];
        string dbname = Configuration["dbname"];

        var connectionString = $"Host={dbserver};Database={dbname};UserId={dbuser};Password={dbpassword}";
        var dbfact = new OrmLiteConnectionFactory(connectionString, PostgreSqlDialect.Provider);
        using IDbConnection db = dbfact.Open();
        TableUp.DoAllTableUps(db);
        return dbfact;
    }

    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices((context, services) =>
                    services.AddSingleton<IDbConnectionFactory>(factory()));
}