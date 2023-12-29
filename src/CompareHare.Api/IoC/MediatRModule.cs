using Autofac;
using Autofac.Extensions.DependencyInjection;
using CompareHare.Api.MediatR.ValidationPipeline;
using MediatR;

namespace CompareHare.Api.IoC
{
    public class MediatRModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Mediator>()
                   .As<IMediator>()
                   .InstancePerLifetimeScope();

            var services = new ServiceCollection();

            // builder.Register<ServiceFactory>(
            //     ctxt =>
            //     {
            //         var componentContext = ctxt.Resolve<IComponentContext>();
            //         return t => componentContext.ResolveOptional(t);
            //     }).InstancePerLifetimeScope();
            builder.Populate(services);

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .AsClosedTypesOf(typeof(INotificationHandler<>));

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .AsClosedTypesOf(typeof(IRequestHandler<>));

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterGeneric(typeof(ValidatableRequestPreProcessorBehavior<>))
                   .As(typeof(IPipelineBehavior<,>));

            builder.RegisterGeneric(typeof(ValidatableRequestPreProcessor<>))
                   .As(typeof(IFluentValidationRequestPreProcessor<>))
                   .As(typeof(ICustomValidationRequestPreProcessor<>))
                   .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ValidatableModelRequestPreProcessor<>))
                   .As(typeof(IFluentValidationRequestPreProcessor<>))
                   .As(typeof(ICustomValidationRequestPreProcessor<>))
                   .InstancePerLifetimeScope();
        }
    }
}
