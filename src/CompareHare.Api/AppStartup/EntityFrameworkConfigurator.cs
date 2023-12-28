using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace CompareHare.Api.AppStartup
{
    public static class EntityFrameworkConfigurator
    {
        public static void Configure(IConfiguration config, DbContextOptionsBuilder builder)
        {
            builder.UseMySQL(config.GetConnectionString("CompareHareDbContext"),
                mySqlServerBuilder => mySqlServerBuilder
                    .MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));
        }
    }
}