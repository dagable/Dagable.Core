using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Dagable.Core
{
    public class StandardNode : INode<StandardNode>
    {
        public int Id { get; protected set; }
        public HashSet<StandardNode> SuccessorNodes { get; }
        public HashSet<StandardNode> PredecessorNodes { get; }
        public int Layer { get; set; }

        public StandardNode()
        {
            Id = default;
            Layer = default;
            SuccessorNodes = new HashSet<StandardNode>();
            PredecessorNodes = new HashSet<StandardNode>();
        }

        public StandardNode(int id) : this()
        {
            Id = id;
            Layer = 0;
        }

        public StandardNode(int id, int layer) : this(id)
        {
            Layer = layer;
        }

        /// <inheritdoc cref="INode.AddSuccessor(StandardNode)"/>
        public StandardNode AddSuccessor(StandardNode n)
        {
            SuccessorNodes.Add(n);
            return this;
        }

        /// <inheritdoc cref="INode.UpdateLayer(int)"/>
        public StandardNode UpdateLayer(int layer)
        {
            Layer = layer;
            return this;
        }

        /// <inheritdoc cref="INode.AddPredecessor(StandardNode)"/>
        public StandardNode AddPredecessor(StandardNode n)
        {
            PredecessorNodes.Add(n);
            return this;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            StandardNode n = (StandardNode)obj;
            return Id.Equals(n.Id);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        /// <inheritdoc cref="INode{N}.CompareTo{T}(N)"/>
        public int CompareTo([AllowNull] StandardNode other)
        {
            return Id - other.Id;
        }

        /// <inheritdoc cref="INode{N}.GetId"/>
        int INode<StandardNode>.GetId()
        {
            return Id;
        }

        /// <inheritdoc cref="INode{N}.CompareTo{T}(N)"/>
        public int CompareTo<N>(N prevNode) where N : INode<N>, new()
        {
            return CompareTo(prevNode);
        }

        public int CompareTo<T>(StandardNode prevNode) where T : INode<StandardNode>, new()
        {
            return CompareTo(prevNode);
        }
    }
}
