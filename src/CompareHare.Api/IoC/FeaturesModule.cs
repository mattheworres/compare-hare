#region usings

using System.Reflection;
using Autofac;
using CompareHare.Domain.Features.Interfaces;
using Module = Autofac.Module;

#endregion

namespace CompareHare.Api.IoC
{
    public class FeaturesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(
                        Assembly.Load("CompareHare.Domain"),
                        ThisAssembly)
                   .AssignableTo<IFeatureService>()
                   .AsImplementedInterfaces()
                   .AsSelf();
        }
    }
}
