using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Dagable.Core.Models
{
    public class CriticalPathNode : StandardNode, INode<CriticalPathNode>, IComparable<CriticalPathNode>
    {
        public int ComputationTime { get; set; }
        public new HashSet<CriticalPathNode> SuccessorNodes { get; }
        public new HashSet<CriticalPathNode> PredecessorNodes { get; }

        public CriticalPathNode() : base() {

            SuccessorNodes = new HashSet<CriticalPathNode>();
            PredecessorNodes = new HashSet<CriticalPathNode>();
        }

        public CriticalPathNode(int id) : this()
        {
            Id = id;
            Layer = 0;
            ComputationTime = 0;
        }

        public CriticalPathNode(int id, int compTime): this(id)
        {
            ComputationTime = compTime;
        }

        public CriticalPathNode(int id, int layer, int compTime) : this(id, compTime)
        {
            Layer = layer;
        }

        public CriticalPathNode AddSuccessor(CriticalPathNode n)
        {
            SuccessorNodes.Add(n);
            return this;
        }

        public CriticalPathNode AddPredecessor(CriticalPathNode n)
        {
            PredecessorNodes.Add(n);
            return this;
        }

        public new CriticalPathNode UpdateLayer(int layer)
        {
            Layer = layer;
            return this;
        }

        public int CompareTo([AllowNull] CriticalPathNode other)
        {
            if(base.CompareTo(other) == 0)
            {
                return ComputationTime - other.ComputationTime;
            }
            return 0;
        }

        int INode<CriticalPathNode>.GetId()
        {
            return Id;
        }

        public int CompareTo<T>(CriticalPathNode prevNode) where T : INode<CriticalPathNode>, new()
        {
            if (base.CompareTo(prevNode) == 0)
            {
                return ComputationTime - prevNode.ComputationTime;
            }
            return 0;
        }
    }
}
