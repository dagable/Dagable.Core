using System.Collections.Generic;
using System.Linq;

namespace Dagable.Core
{
    internal sealed class Graph : IGraph
    {
        public HashSet<Edge> Edges { get; }

        public readonly HashSet<Node> Nodes;

        public Graph()
        {
            Edges = new HashSet<Edge>();
            Nodes = new HashSet<Node>();
        }

        /// <summary>
        /// Constructor for a graph object. 
        /// </summary>
        /// <param name="n">A node that will act as a root node for the graph.</param>
        public Graph(Node n) : this()
        {
            Nodes.Add(n);
        }

        /// <inheritdoc cref="IGraph.AddEdges(Node, IEnumerable{Node})" />
        public bool AddEdges(Node i, IEnumerable<Node> nextNodes)
        {
            return nextNodes.ToList().TrueForAll(x => AddEdge(i, x));
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

        /// <inheritdoc cref="IGraph.AddNode(Node)" />
        public bool AddNode(Node n)
        {
            return Nodes.Add(n);
        }
    }
}
