using static Dagable.Core.DAG;

namespace Dagable.Core
{
    public interface IDagCreationService
    {
        ICriticalPathTaskGraph GenerateCriticalPathTaskGraph();
        ICriticalPathTaskGraph GenerateCriticalPathTaskGraph(int layers, int nodes, double probability);
        ICriticalPathTaskGraph GenerateCriticalPathTaskGraph(int layers, int nodes, double probability, int minComp, int maxComp, int minComm, int maxComm);
        IStandardTaskGraph GenerateStandardTaskGraph();
        IStandardTaskGraph GenerateStandardTaskGraph(int layers, int nodes, double probability);
    }
}