#region usings

using System.Data.Common;
using Autofac;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Sql;
using CompareHare.Domain.Sql.Interfaces;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

#endregion

namespace CompareHare.Domain.IoC
{
    public class SqlModule : Module
    {
        public const string ConnectionStringKey = "ConnectionString";

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ctxt => ctxt.Resolve<IConfiguration>().GetConnectionString("CompareHareDbContext"))
                   .Keyed<string>(ConnectionStringKey);

            builder.Register(ctxt => new MySqlConnection(ctxt.ResolveKeyed<string>(ConnectionStringKey)))
                   .As<DbConnection>()
                   .As<MySqlConnection>();

            builder.Register(ctxt => ctxt.Resolve<CompareHareDbContext>().Database.CreateExecutionStrategy());

            builder.RegisterType<SqlExecutor>().As<ISqlExecutor>();
        }
    }
}
