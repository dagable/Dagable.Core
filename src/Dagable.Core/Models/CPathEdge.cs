using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Dagable.Core.Models
{
    public class CPathEdge : Edge<CPathNode>, IComparable<CPathEdge>, IEdge<CPathNode>
    {
        public int coomunicationTime { get; set; }
        public new CPathNode NextNode { get; set; }
        public new CPathNode PrevNode { get; set; }

        public CPathEdge() { }

        public int CompareTo([AllowNull] CPathEdge other)
        {
            return 0;
        }
    }
}
