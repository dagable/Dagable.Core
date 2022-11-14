namespace Dagable.Core
{
    public interface IDagCreationService
    {
        /// <summary>
        /// Method used to create a Critical Path task graph using default settings
        /// </summary>
        /// <returns>The created Critical Path task graph</returns>
        ICriticalPathTaskGraph GenerateCriticalPathTaskGraph();

        /// <summary>
        /// Method used to create a Critical Path task graph using default settings
        /// </summary>
        /// <param name="layers">The number of layers that the graph should have</param>
        /// <param name="nodes">The total number of nodes that the task graph should contain</param>
        /// <param name="probability">The probability that a node has to be connected to a node on the next layer</param>
        /// <returns>The created Critical Path task graph based on options passed to it</returns>
        ICriticalPathTaskGraph GenerateCriticalPathTaskGraph(int layers, int nodes, double probability);

        /// <summary>
        /// Method used to create a Critical Path task graph using default settings
        /// </summary>
        /// <param name="layers">The number of layers that the graph should have</param>
        /// <param name="nodes">The total number of nodes that the task graph should contain</param>
        /// <param name="probability">The probability that a node has to be connected to a node on the next layer</param>
        /// <param name="minComp">The minimum computation time that a node can have</param>
        /// <param name="maxComp">The maximum computation time that a node can have</param>
        /// <param name="minComm">The minimum communication time that an edge can have</param>
        /// <param name="maxComm">The maximum communication time that an edge can have</param>
        /// <returns>The created Critical Path task graph based on options passed to it</returns>
        ICriticalPathTaskGraph GenerateCriticalPathTaskGraph(int layers, int nodes, double probability, int minComp, int maxComp, int minComm, int maxComm);
      
        /// <summary>
        /// Method used to create a Standard task graph using default settings
        /// </summary>
        /// <returns>The created Standard task graph</returns>
        IStandardTaskGraph GenerateStandardTaskGraph();

        /// <summary>
        /// Method used to create a Standard task graph using default settings
        /// </summary>
        /// <param name="layers">The number of layers that the graph should have</param>
        /// <param name="nodes">The total number of nodes that the task graph should contain</param>
        /// <param name="probability">The probability that a node has to be connected to a node on the next layer</param>
        /// <returns>The created Standard task graph based on options passed to it</returns>
        IStandardTaskGraph GenerateStandardTaskGraph(int layers, int nodes, double probability);
    }
}