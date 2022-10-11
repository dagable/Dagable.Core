using static Dagable.Core.DAG;

namespace Dagable.Core
{
    public sealed class DagCreationService : IDagCreationService
    {
        public IStandardTaskGraph<StandardTaskGraph> GenerateStandardTaskGraph()
        {
            return new StandardTaskGraph();
        }

        public IStandardTaskGraph<StandardTaskGraph> GenerateStandardTaskGraph(int layers, int nodes, double probability)
        {
            return new StandardTaskGraph(layers, nodes, probability);
        }

        public ICriticalPathTaskGraph GenerateCriticalPathTaskGraph()
        {
            return new CriticalPathTaskGraph();
        }

        public ICriticalPathTaskGraph GenerateCriticalPathTaskGraph(int layers, int nodes, double probability)
        {
            return new CriticalPathTaskGraph(layers, nodes, probability);
        }

        public ICriticalPathTaskGraph GenerateCriticalPathTaskGraph(int layers, int nodes, double probability, int minComp, int maxComp, int minComm, int maxComm)
        {
            return new CriticalPathTaskGraph(minComp, maxComp, minComm, maxComm, layers, nodes, probability);
        }
    }
}
