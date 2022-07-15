using System;
using System.Diagnostics.CodeAnalysis;

namespace Dagable.Core
{
    public class Edge<N> : IComparable<Edge<N>>, IEdge<N> where N : INode<N>, new ()
    {
        public N PrevNode { get; set; }
        public N NextNode { get; set; }

        public Edge() { }

        public Edge(N prevNode, N nextNode)
        {
            PrevNode = prevNode;
            NextNode = nextNode;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Edge<N>)obj;

            if (NextNode.Equals(other.NextNode))
            {
                return PrevNode.Equals(other.PrevNode);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(PrevNode.GetId(), NextNode.GetId()).GetHashCode();
        }

        /// <summary>
        /// Method used to determine if one edge is the same as another edge
        /// </summary>
        /// <param name="other">The second edge that we want to compare the first edge to</param>
        /// <returns>0 if both edges are the same, any other value if they are different.</returns>
        public int CompareTo([AllowNull] Edge<N> other)
        {
            var prevNodeComparison = PrevNode.CompareTo(other.PrevNode);
            return prevNodeComparison == 0 ? prevNodeComparison : NextNode.CompareTo(other.NextNode);
        }
    }
}
