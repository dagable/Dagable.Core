using System;
using System.Diagnostics.CodeAnalysis;

namespace Dagable.Core
{
    public class StandardEdge<N> : IComparable<StandardEdge<N>>, IEdge<N> where N : INode<N>, new ()
    {
        public N PrevNode { get; set; }
        public N NextNode { get; set; }

        public StandardEdge() { }

        public StandardEdge(N prevNode, N nextNode)
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

            var other = (StandardEdge<N>)obj;

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

        /// <inheritdoc cref="IEdge{N}.C"/>
        public int CompareTo([AllowNull] StandardEdge<N> other)
        {
            var prevNodeComparison = PrevNode.CompareTo<N>(other.PrevNode);
            return prevNodeComparison == 0 ? prevNodeComparison : NextNode.CompareTo<N>(other.NextNode);
        }
    }
}
