using System;
using System.Diagnostics.CodeAnalysis;

namespace Dagable.Core.Models
{
    public class CPathEdge : Edge<CPathNode>, IComparable<CPathEdge>, IEdge<CPathNode>
    {
        private int CommTime { get; set; }
        public new CPathNode NextNode { get; set; }
        public new CPathNode PrevNode { get; set; }

        public CPathEdge() : base() { }

        public CPathEdge(CPathNode prevNode, CPathNode nextNode, int commTime) : base(prevNode, nextNode)
        {
            CommTime = commTime;
        }

        public int CompareTo([AllowNull] CPathEdge other)
        {
            var baseCompare = base.CompareTo(other);
            return baseCompare == 0 ? CommTime - other.CommTime : baseCompare;
        }
    }
}
