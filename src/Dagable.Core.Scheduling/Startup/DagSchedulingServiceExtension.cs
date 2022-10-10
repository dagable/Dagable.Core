using Microsoft.Extensions.DependencyInjection;

namespace Dagable.Core.Scheduling.Startup
{
    public static class DagSchedulingServiceExtension
    {
        public static IServiceCollection AddDagableSchedulingServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IScheduler, DSLScheduler>();

            return serviceCollection;
        }
    }
}
