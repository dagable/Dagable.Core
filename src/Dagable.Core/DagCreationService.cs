using static Dagable.Core.DAG;

namespace Dagable.Core
{
    public sealed class DagCreationService : IDagCreationService
    {
        private IDagCreation<Standard> Standard { get; }
        private IDagCriticalPathCreation CriticalPath { get; }

        public DagCreationService(IDagCreation<Standard> standard, IDagCriticalPathCreation criticalPath)
        {
            Standard = standard;
            CriticalPath = criticalPath;
        }

        public string GenerateGraphAsString(GraphType graphType)
        {
            if (graphType == GraphType.Standard)
            {
                return Standard.Generate().AsJson();
            }
            return CriticalPath.Generate().AsJson();
        }

        public string GenerateGraphAsString(GraphType graphType, int layers, int nodes, double probability)
        {
            if (graphType == GraphType.Standard)
            {
                return Standard.Setup(layers, nodes, probability).Generate().AsJson();
            }
            return CriticalPath.Setup(layers, nodes, probability).Generate().AsJson();
        }

        public string GenerateGraphAsString(GraphType graphType, int minComp, int maxComp, int minComm, int maxComm, int layers, int nodeCount, double probability)
        {
            if (graphType == GraphType.Standard)
            {
                return Standard.Setup(layers, nodeCount, probability).Generate().AsJson();
            }
            return CriticalPath.Setup(minComp, maxComp, minComm, maxComm, layers, nodeCount, probability).Generate().AsJson();
        }

    }
}
