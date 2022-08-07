using Dagable.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Dagable.Core
{
    public class Graph<N, E> : IGraph<N,E> where N : INode<N> where E : IEdge<N>, new()
    {
        public HashSet<E> Edges { get; }

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

        /// <inheritdoc cref="IGraph.AddEdges(IEnumerable{N}, N)" />
        public bool AddEdges(IEnumerable<N> prevNodes, N i)
        {
            return prevNodes.ToList().TrueForAll(x => AddEdge(new E
            {
                NextNode = i,
                PrevNode = x
            }));
        }

        /// <inheritdoc cref="IGraph.AddEdge(E)" />
        public bool AddEdge(E edge)
        {
            edge.PrevNode.AddSuccessor(edge.NextNode);
            edge.NextNode.AddPredecessor(edge.PrevNode);
            if (!Edges.Contains(edge))
            {
                Nodes.Add(edge.PrevNode);
                Nodes.Add(edge.NextNode);
                Edges.Add(edge);
            }
            return true;
        }

        /// <inheritdoc cref="IGraph.AddNode(N)" />
        public bool AddNode(N n)
        {
            return Nodes.Add(n);
        }
    }
}
