using System.Collections.Generic;

namespace Dagifier.Core
{
    public interface IGraph
    {
        /// <summary>
        /// Method used to add a new edge to the graph, given two integers that will act as ids for the nodes.
        /// </summary>
        /// <param name="i">the node the edge is connecting from</param>
        /// <param name="j">the node the edge is connecting to</param>
        /// <returns>true if the node was added, false otherwise.</returns>
        bool AddEdge(int i, int j);

        /// <summary>
        /// Method used to add a new edge to the graph given two nodes
        /// </summary>
        /// <param name="i">the node we want to create the edge from.</param>
        /// <param name="j">the node we want to create the edge to.</param>
        /// <returns>true if the node was added false if the node already existed.</returns>
        bool AddEdge(Node i, Node j);
    }

    public sealed class Graph : IGraph
    {
        public HashSet<Edge> Edges { get; }

        private HashSet<Node> Nodes;

        public Graph() {
            Edges = new HashSet<Edge>();
            Nodes = new HashSet<Node>();
        }

        /// <inheritdoc cref="IGraph.AddEdge(int, int)" />
        public bool AddEdge(int i, int j)
        {
            var firstNode = new Node(i);
            var secondNode = new Node(j);
            Nodes.Add(firstNode);
            Nodes.Add(secondNode);
            return Edges.Add(new Edge(firstNode, secondNode));
        }

        /// <inheritdoc cref="IGraph.AddEdge(Node, Node)" />
        public bool AddEdge(Node i, Node j)
        {
            Nodes.Add(i);
            Nodes.Add(j);
            return Edges.Add(new Edge(i, j));
        }
    }
}
