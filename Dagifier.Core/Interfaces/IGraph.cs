namespace Dagifier.Core
{
    internal interface IGraph
    {
        /// <summary>
        /// Method used to add a new edge to the graph given two nodes
        /// </summary>
        /// <param name="i">the node we want to create the edge from.</param>
        /// <param name="j">the node we want to create the edge to.</param>
        /// <returns>true if the node was added false if the node already existed.</returns>
        bool AddEdge(Node i, Node j);

        /// <summary>
        /// Method that will add an edge to the graph and return the newly added edge
        /// </summary>
        /// <param name="i">The node that we want to create the edge from</param>
        /// <param name="j">The node that we want to create the edge to</param>
        /// <parma name="e">The edge that we want to add to the graph</parma>
        /// <returns>The mewly created edge</returns>
        Edge AddEdge(Node i, Node j, Edge e);
    }
}
