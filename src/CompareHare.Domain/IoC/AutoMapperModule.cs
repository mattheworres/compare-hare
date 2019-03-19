#region usings

using System.Collections.Generic;
using System.Linq;
using Autofac;
using AutoMapper;

#endregion

namespace CompareHare.Domain.IoC
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ctxt => new MapperConfiguration(
                                 cfg =>
                                 {
                                     var profiles = ctxt.Resolve<IEnumerable<Profile>>();
                                     foreach (var profile in profiles) cfg.AddProfile(profile);
                                 }))
                   .As<MapperConfiguration>()
                   .As<IConfigurationProvider>()
                   .SingleInstance();

            builder.Register(ctxt =>
                             {
                                 var scope = ctxt.Resolve<ILifetimeScope>();
                                 return ctxt.Resolve<MapperConfiguration>().CreateMapper(scope.Resolve);
                             });

            RegisterProfiles(builder);
            RegisterTypeConverters(builder);
            RegisterValueResolvers(builder);
        }

        private void RegisterProfiles(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                   .AssignableTo<Profile>()
                   .As<Profile>();
        }

        private void RegisterTypeConverters(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.GetInterfaces().Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == typeof(ITypeConverter<,>)))
                   .AsSelf();
        }

        private void RegisterValueResolvers(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.GetInterfaces().Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == typeof(IValueResolver<,,>)))
                   .AsSelf();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(t => t.GetInterfaces().Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == typeof(IMemberValueResolver<,,,>)))
                   .AsSelf();
        }
    }
}
