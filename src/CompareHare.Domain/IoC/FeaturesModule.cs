using Autofac;
using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Domain.IoC
{
    public class FeaturesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                   .AssignableTo<IFeatureService>()
                   .AsImplementedInterfaces()
                   .AsSelf();
        }
    }
}
