using Dagable.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Dagable.Core
{
    internal class Graph<N, E> : IGraph<N,E> where N : INode<N> where E : IEdge<N>, new()
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
            return nextNodes.ToList().TrueForAll(x => AddEdge(i, x));
        }

        /// <inheritdoc cref="IGraph.AddEdges(IEnumerable{N}, N)" />
        public bool AddEdges(IEnumerable<N> prevNodes, N i)
        {
            return prevNodes.ToList().TrueForAll(x => AddEdge(i, x));
        }

        /// <inheritdoc cref="IGraph.AddEdge(N, N)" />
        public bool AddEdge(N prevNode, N nextNode)
        {
            Nodes.Add(prevNode.AddSuccessor(nextNode));
            Nodes.Add(nextNode.AddPredecessor(prevNode));
            var newEdge = new E
            {
                NextNode = nextNode,
                PrevNode = prevNode,
            };
            return Edges.Add(newEdge);
        }

        /// <inheritdoc cref="IGraph.AddEdge(N, N, E)" />
        public E AddEdge(N prevNode, N nextNode, E edge)
        {
            if (!Edges.Contains(edge))
            {
                Nodes.Add(prevNode);
                Nodes.Add(nextNode);
                Edges.Add(edge);
            }

            return edge;
        }

        /// <inheritdoc cref="IGraph.AddNode(N)" />
        public bool AddNode(N n)
        {
            return Nodes.Add(n);
        }
    }
}
