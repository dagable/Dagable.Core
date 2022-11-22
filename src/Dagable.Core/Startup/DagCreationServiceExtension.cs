using Microsoft.Extensions.DependencyInjection;
using static Dagable.Core.TaskGraph;

namespace Dagable.Core
{
    public static class DagCreationServiceExtension
    {
        public static IServiceCollection AddDagableCoreServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICriticalPathTaskGraph, CriticalPath>()
                .AddTransient<IStandardTaskGraph<StandardNode, StandardEdge<StandardNode>>, Standard>()
                .AddTransient<IDagCreationService, DagCreationService>();
            

            return serviceCollection;
        }
    }
}
