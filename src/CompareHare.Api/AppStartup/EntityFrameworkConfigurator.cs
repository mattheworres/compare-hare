#region usings

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

#endregion

namespace CompareHare.Api.AppStartup
{
    public static class EntityFrameworkConfigurator
    {
        public static void Configure(IConfiguration configuration, DbContextOptionsBuilder builder)
        {
            builder.UseMySql(
                configuration.GetConnectionString("CompareHareDbContext"),
                mySqlServerBuilder => mySqlServerBuilder
                    .MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));
        }
    }
}
