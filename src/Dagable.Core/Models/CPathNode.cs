using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Dagable.Core.Models
{
    public class CPathNode : Node, INode<CPathNode>, IComparable<CPathNode>
    {
        public int ComputationTime { get; set; }
        public new HashSet<CPathNode> SuccessorNodes { get; }
        public new HashSet<CPathNode> PredecessorNodes { get; }

        public CPathNode() : base() { }

        public CPathNode(int id) : this()
        {
            Id = id;
            Layer = 0;
        }

        public CPathNode(int id, int layer, int compTime) : this(id)
        {
            Layer = layer;
            ComputationTime = compTime;
        }

        public CPathNode AddSuccessor(CPathNode n)
        {
            SuccessorNodes.Add(n);
            return this;
        }

        public CPathNode AddPredecessor(CPathNode n)
        {
            PredecessorNodes.Add(n);
            return this;
        }

        public new CPathNode UpdateLayer(int layer)
        {
            Layer = layer;
            return this;
        }

        public int CompareTo([AllowNull] CPathNode other)
        {
            if(base.CompareTo(other) == 0)
            {
                return ComputationTime - other.ComputationTime;
            }
            return 0;
        }

        int INode<CPathNode>.GetId()
        {
            return Id;
        }
    }
}
