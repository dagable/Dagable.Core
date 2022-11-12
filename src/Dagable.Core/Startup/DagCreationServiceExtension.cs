using Microsoft.Extensions.DependencyInjection;
using static Dagable.Core.DAG;

namespace Dagable.Core
{
    public static class DagCreationServiceExtension
    {
        public static IServiceCollection AddDagableCoreServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICriticalPathTaskGraph, CriticalPathTaskGraph>()
                .AddTransient<IStandardTaskGraph, StandardTaskGraph>()
                .AddTransient<IDagCreationService, DagCreationService>();
            

            return serviceCollection;
        }
    }
}
