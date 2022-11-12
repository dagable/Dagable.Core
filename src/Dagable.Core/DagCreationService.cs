using static Dagable.Core.DAG;

namespace Dagable.Core
{
    public sealed class DagCreationService : IDagCreationService
    {
        public IStandardTaskGraph GenerateStandardTaskGraph()
        {
            return new StandardTaskGraph().Generate();
        }

        public IStandardTaskGraph GenerateStandardTaskGraph(int layers, int nodes, double probability)
        {
            return new StandardTaskGraph(layers, nodes, probability).Generate();
        }

        public ICriticalPathTaskGraph GenerateCriticalPathTaskGraph()
        {
            return new CriticalPathTaskGraph().Generate();
        }

        public ICriticalPathTaskGraph GenerateCriticalPathTaskGraph(int layers, int nodes, double probability)
        {
            return new CriticalPathTaskGraph(layers, nodes, probability).Generate();
        }

        public ICriticalPathTaskGraph GenerateCriticalPathTaskGraph(int layers, int nodes, double probability, int minComp, int maxComp, int minComm, int maxComm)
        {
            return new CriticalPathTaskGraph(minComp, maxComp, minComm, maxComm, layers, nodes, probability).Generate();
        }
    }
}
