#region usings

using Autofac;
using CompareHare.Api.Features.Shared.Validation.Interfaces;
using FluentValidation;

#endregion

namespace CompareHare.Api.IoC
{
    public class ValidationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                   .AsClosedTypesOf(typeof(IValidator<>))
                   .AsImplementedInterfaces()
                   .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .AsClosedTypesOf(typeof(ICustomValidator<>))
                   .AsImplementedInterfaces();
        }
    }
}
