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

        public StandardNode(int id) : this()
        {
            Id = id;
            Layer = 0;
        }

        public StandardNode(int id, int layer) : this(id)
        {
            Layer = layer;
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

        /// <summary>
        /// Method used to compare two nodes.
        /// </summary>
        /// <param name="other">The second node to compare the first node to</param>
        /// <returns>zero is the same, all other values mean the node is different.</returns>
        public int CompareTo([AllowNull] StandardNode other)
        {
            return Id - other.Id;
        }

        int INode<StandardNode>.GetId()
        {
            return Id;
        }

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
