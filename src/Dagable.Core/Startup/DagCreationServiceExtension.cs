using Microsoft.Extensions.DependencyInjection;
using static Dagable.Core.DAG;

namespace Dagable.Core
{
    public static class DagCreationServiceExtension
    {
        public static IServiceCollection AddDagableCoreServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IDagCriticalPathCreation, CriticalPath>()
                .AddTransient<IDagCreation<Standard> , Standard >()
                .AddTransient<IDagCreationService, DagCreationService>();

            return serviceCollection;
        }
    }
}
