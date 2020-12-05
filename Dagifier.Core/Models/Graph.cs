using System.Collections.Generic;
using System.Linq;

namespace Dagifier.Core
{
    public sealed class Graph : IGraph
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

        public List<Node> KhansTopologySort()
        {
            var sortedNodes = new List<Node>();

            var noIncomingEdges = new HashSet<Node>(Nodes.Where(n => Edges.All(x => x.NextNode.Equals(n) == false)));

            while (noIncomingEdges.Any())
            {
                var n = noIncomingEdges.First();
                noIncomingEdges.Remove(n);

                sortedNodes.Add(n);

                foreach (var e in Edges.Where(e => e.PrevNode.Equals(n)).ToList())
                {
                    var m = e.NextNode;

                    // remove edge e from the graph
                    Edges.Remove(e);

                    // if m has no other incoming edges then
                    if (Edges.All(me => me.NextNode.Equals(m) == false))
                    {
                        // insert m into S
                        noIncomingEdges.Add(m);
                    }
                }
            }
            // if graph has edges then
            if (Edges.Any())
            {
                // return error (graph has at least one cycle)
                return null;
            }
            else
            {
                // return L (a topologically sorted order)
                return sortedNodes;
            }
        }
    }
}
