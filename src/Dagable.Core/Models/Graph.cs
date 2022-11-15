using System.Collections.Generic;
using System.Linq;

namespace Dagable.Core
{
    public class Graph<N, E> : IGraph<N,E> where N : INode<N> where E : IEdge<N>, new()
    {
        public readonly HashSet<E> Edges;

        public readonly HashSet<N> Nodes;

        public Graph()
        {
            Edges = new HashSet<E>();
            Nodes = new HashSet<N>();
        }

        /// <summary>
        /// Constructor for a graph object. 
        /// </summary>
        /// <param name="n">A node that will act as a root node for the graph.</param>
        public Graph(N n) : this()
        {
            Nodes.Add(n);
        }

        /// <inheritdoc cref="IGraph.AddEdges(N, IEnumerable{N})" />
        public bool AddEdges(N i, IEnumerable<N> nextNodes)
        {
            return nextNodes.ToList().TrueForAll(x => AddEdge(new E
            {
                NextNode = x,
                PrevNode = i
            }));
        }

        /// <inheritdoc cref="IGraph.AddEdge(E)" />
        public bool AddEdge(E edge)
        {
            if (!Edges.Contains(edge))
            {
                edge.PrevNode.AddSuccessor(edge.NextNode);
                edge.NextNode.AddPredecessor(edge.PrevNode);
                Nodes.Add(edge.PrevNode);
                Nodes.Add(edge.NextNode);
                return Edges.Add(edge);
            }
            return false;
        }

        /// <inheritdoc cref="IGraph.AddNode(N)" />
        public bool AddNode(N n)
        {
            return Nodes.Add(n);
        }
    }
}
