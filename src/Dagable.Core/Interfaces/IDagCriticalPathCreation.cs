using static Dagable.Core.DAG;

namespace Dagable.Core
{
    public interface IDagCriticalPathCreation : IDagCreation<CriticalPath>
    {
        CriticalPath Setup(int minComp, int maxComp, int minComm, int maxComm, int layers, int nodeCount, double probability);
    }
}
