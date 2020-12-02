using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Dagifier.Core
{
    internal sealed class Node : INode, IComparable<Node>
    {
        public int Id { get; private set; }
        public HashSet<Node> SuccessorNodes { get; }
        public HashSet<Node> PredecessorNodes { get; }

        public Node() {
            SuccessorNodes = new HashSet<Node>();
            PredecessorNodes = new HashSet<Node>();
        }

        /// <inheritdoc cref="INode.AddSuccessor(Node)"/>
        public bool AddSuccessor(Node n)
        {
            return SuccessorNodes.Add(n);
        }

        /// <inheritdoc cref="INode.AddPredecessor(Node)"/>
        public bool AddPredecessor(Node n)
        {
            return PredecessorNodes.Add(n);
        }

        public Node(int id) : this()
        {
            Id = id;
        }

        public override bool Equals(object obj)
        {
            if(obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Node n = (Node)obj;
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
        public int CompareTo([AllowNull] Node other)
        {
            return Id - other.Id;
        }
    }
}
