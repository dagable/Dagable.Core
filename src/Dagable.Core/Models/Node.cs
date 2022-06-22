﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Dagable.Core
{
    public sealed class Node : INode, IComparable<Node>
    {
        public int Id { get; private set; }
        public HashSet<Node> SuccessorNodes { get; }
        public HashSet<Node> PredecessorNodes { get; }
        public int Layer { get; set; }

        public Node()
        {
            Id = default;
            Layer = default;
            SuccessorNodes = new HashSet<Node>();
            PredecessorNodes = new HashSet<Node>();
        }

        /// <inheritdoc cref="INode.AddSuccessor(Node)"/>
        public Node AddSuccessor(Node n)
        {
            SuccessorNodes.Add(n);
            return this;
        }

        /// <inheritdoc cref="INode.UpdateLayer(int)"/>
        public Node UpdateLayer(int layer)
        {
            Layer = layer;
            return this;
        }

        /// <inheritdoc cref="INode.AddPredecessor(Node)"/>
        public Node AddPredecessor(Node n)
        {
            PredecessorNodes.Add(n);
            return this;
        }

        public Node(int id) : this()
        {
            Id = id;
            Layer = 0;
        }

        public Node(int id, int layer) : this(id)
        {
            Layer = layer;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
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
