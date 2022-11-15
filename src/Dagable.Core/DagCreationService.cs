using static Dagable.Core.TaskGraph;

namespace Dagable.Core
{
    public sealed class DagCreationService : IDagCreationService
    {

        /// <inheritdoc cref="IDagCreationService.GenerateStandardTaskGraph"/>
        public IStandardTaskGraph<StandardNode, StandardEdge<StandardNode>> GenerateStandardTaskGraph()
        {
            return new Standard().Generate();
        }

        /// <inheritdoc cref="IDagCreationService.GenerateStandardTaskGraph(int, int, double)"/>
        public IStandardTaskGraph<StandardNode, StandardEdge<StandardNode>> GenerateStandardTaskGraph(int layers, int nodes, double probability)
        {
            return new Standard(layers, nodes, probability).Generate();
        }

        /// <inheritdoc cref="IDagCreationService.GenerateCriticalPathTaskGraph"/>
        public ICriticalPathTaskGraph GenerateCriticalPathTaskGraph()
        {
            return new CriticalPath().Generate();
        }

        /// <inheritdoc cref="IDagCreationService.GenerateCriticalPathTaskGraph(int, int, double)"/>
        public ICriticalPathTaskGraph GenerateCriticalPathTaskGraph(int layers, int nodes, double probability)
        {
            return new CriticalPath(layers, nodes, probability).Generate();
        }

        /// <inheritdoc cref="IDagCreationService.GenerateCriticalPathTaskGraph(int, int, double, int, int, int, int)"/>
        public ICriticalPathTaskGraph GenerateCriticalPathTaskGraph(int layers, int nodes, double probability, int minComp, int maxComp, int minComm, int maxComm)
        {
            return new CriticalPath(minComp, maxComp, minComm, maxComm, layers, nodes, probability).Generate();
        }
    }
}
