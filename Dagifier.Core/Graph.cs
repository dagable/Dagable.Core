using System.Collections.Generic;

namespace Dagifier.Core
{
    internal sealed class Graph : IGraph
    {
        public HashSet<Edge> Edges { get; }

        private HashSet<Node> Nodes;

        public Graph() {
            Edges = new HashSet<Edge>();
            Nodes = new HashSet<Node>();
        }

        /// <inheritdoc cref="IGraph.AddEdge(Node, Node)" />
        public bool AddEdge(Node prevNode, Node nextNode)
        {
            Nodes.Add(prevNode);
            Nodes.Add(nextNode);
            return Edges.Add(new Edge(prevNode, nextNode));
        }

        /// <inheritdoc cref="IGraph.AddEdge(Node, Node, Edge)" />
        public Edge AddEdge(Node prevNode, Node nextNode, Edge edge)
        {
            if (!Edges.Contains(edge))
            {
                Nodes.Add(prevNode);
                Nodes.Add(nextNode);
                Edges.Add(edge);
            }

            return edge;
        }
    }
}
