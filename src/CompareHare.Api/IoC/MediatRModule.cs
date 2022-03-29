using CompareHare.Api.MediatR.ValidationPipeline;
using Autofac;
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

            builder.Register<ServiceFactory>(
                ctxt =>
                {
                    var componentContext = ctxt.Resolve<IComponentContext>();
                    return t => componentContext.ResolveOptional(t);
                }).InstancePerLifetimeScope();

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
