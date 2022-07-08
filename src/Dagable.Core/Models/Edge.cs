using System;
using System.Diagnostics.CodeAnalysis;

namespace Dagable.Core
{
    public class Edge : IEdge<Node, Edge>, IComparable<Edge>
    {
        public Node PrevNode { get; set; }
        public Node NextNode { get; set; }

        public Edge() { }

        public Edge(Node prevNode, Node nextNode)
        {
            PrevNode = prevNode;
            NextNode = nextNode;
            prevNode.AddSuccessor(nextNode);
            nextNode.AddPredecessor(prevNode);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Edge)obj;

            if (NextNode.Equals(other.NextNode))
            {
                return PrevNode.Equals(other.PrevNode);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(PrevNode.Id, NextNode.Id).GetHashCode();
        }

        /// <summary>
        /// Method used to determine if one edge is the same as another edge
        /// </summary>
        /// <param name="other">The second edge that we want to compare the first edge to</param>
        /// <returns>0 if both edges are the same, any other value if they are different.</returns>
        public int CompareTo([AllowNull] Edge other)
        {
            var prevNodeComparison = PrevNode.CompareTo(other.PrevNode);
            return prevNodeComparison == 0 ? prevNodeComparison : NextNode.CompareTo(other.NextNode);
        }
    }
}
