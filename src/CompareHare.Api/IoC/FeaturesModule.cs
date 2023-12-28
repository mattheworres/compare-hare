using System.Reflection;
using Autofac;
using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Api.IoC;

public class FeaturesModule : Autofac.Module
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
