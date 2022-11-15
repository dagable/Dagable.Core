using System;
using System.Diagnostics.CodeAnalysis;

namespace Dagable.Core.Models
{
    public class CriticalPathEdge : StandardEdge<CriticalPathNode>, IComparable<CriticalPathEdge>, IEdge<CriticalPathNode>
    {
        public int CommTime { get; set; }

        public CriticalPathEdge() : base() { }

        public CriticalPathEdge(CriticalPathNode prevNode, CriticalPathNode nextNode, int commTime) : base(prevNode, nextNode)
        {
            CommTime = commTime;
        }

        public int CompareTo([AllowNull] CriticalPathEdge other)
        {
            var baseCompare = base.CompareTo(other);
            return baseCompare == 0 ? CommTime - other.CommTime : baseCompare;
        }

        /// <inheritdoc cref="IEdge{N}.Equals(object)"/>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (CriticalPathEdge)obj;

            if (NextNode.Equals(other.NextNode))
            {
                return PrevNode.Equals(other.PrevNode);
            }

            return false;
        }

        /// <inheritdoc cref="IEdge{N}.GetHashCode"/>
        public override int GetHashCode()
        {
            return Tuple.Create(PrevNode.Id, NextNode.Id, CommTime).GetHashCode();
        }
    }
}
