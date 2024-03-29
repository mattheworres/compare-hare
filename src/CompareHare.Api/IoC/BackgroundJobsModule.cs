#region usings

using Autofac;
using CompareHare.Api.BackgroundJobs.Interfaces;

#endregion

namespace CompareHare.Api.IoC
{
    public class BackgroundJobsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                   .AsClosedTypesOf(typeof(IJobRunner<>));
            builder.RegisterAssemblyTypes(ThisAssembly)
                    .AsClosedTypesOf(typeof(ISyncJobRunner<>));
        }
    }
}
