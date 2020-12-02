using System;
using System.Diagnostics.CodeAnalysis;

namespace Dagifier.Core
{
    public sealed class Node : IComparable<Node>
    {
        public int Id { get; private set; }

        public Node() { }

        public Node(int id)
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
