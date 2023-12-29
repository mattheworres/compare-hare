using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace CompareHare.Api.AppStartup
{
    public static class EntityFrameworkConfigurator
    {
        public static void Configure(IConfiguration config, DbContextOptionsBuilder builder)
        {
            var connString = config.GetConnectionString("CompareHareDbContext");
            // Long term I'd prefer not to hard code version of DB, rather rely on conn string.
            // If this doesn't work well, we may just stuff that in the appsettings as well.
            var version = ServerVersion.AutoDetect(connString);
            // UseMySql === Pomelo/MySqlConnector
            builder.UseMySql(connString, version, mySqlOptions => {
                mySqlOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
        }
    }
}
