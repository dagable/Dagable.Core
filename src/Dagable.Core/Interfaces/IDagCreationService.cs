namespace Dagable.Core
{
    public interface IDagCreationService
    {
        string GenerateGraphAsString(GraphType graphType);
        string GenerateGraphAsString(GraphType graphType, int layers, int nodes, double probability);
        string GenerateGraphAsString(GraphType graphType, int minComp, int maxComp, int minComm, int maxComm, int layers, int nodeCount, double probability);
    }
}