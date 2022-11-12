using Microsoft.Extensions.DependencyInjection;

namespace Dagable.Core.Scheduling
{
    public static class DagSchedulingServiceExtension
    {
        public static IServiceCollection AddDagableSchedulingServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ITaskGraphSchedulingService, TaskGraphSchedulingService>();
            return serviceCollection;
        }
    }
}
