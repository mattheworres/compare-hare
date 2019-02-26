#region usings

using Autofac;
using Microsoft.AspNetCore.Http;

#endregion

namespace CompareHare.Api.IoC
{
    public class AuthenticationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ctxt => ctxt.Resolve<IHttpContextAccessor>().HttpContext.User).InstancePerLifetimeScope();
        }
    }
}
