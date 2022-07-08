﻿using System.Collections.Generic;

namespace Dagable.Core
{
    internal interface IGraph<N, E> where N : INode<N> where E : IEdge<N, E>, new()
    {
        /// <summary>
        /// A Method that will connect a node to a list of nodes
        /// </summary>
        /// <param name="i">the node that we want to update the successors of</param>
        /// <param name="nextNodes">The successor nodes of this node</param>
        /// <returns>true if all of the edges were added, false if atleast one of the edges already exists.</returns>
        bool AddEdges(N i, IEnumerable<N> nextNodes);

        /// <summary>
        /// A Method that will connect a node to a list of nodes
        /// </summary>
        /// <param name="i">the node that we want to update the successors of</param>
        /// <param name="nextNodes">The successor nodes of this node</param>
        /// <returns>true if all of the edges were added, false if atleast one of the edges already exists.</returns>
        bool AddEdges(IEnumerable<N> nextNodes, N i);

        /// <summary>
        /// Method used to add a new edge to the graph given two nodes
        /// </summary>
        /// <param name="i">the node we want to create the edge from.</param>
        /// <param name="j">the node we want to create the edge to.</param>
        /// <returns>true if the node was added false if the node already existed.</returns>
        bool AddEdge(N i, N j);

        /// <summary>
        /// Method that will add an edge to the graph and return the newly added edge
        /// </summary>
        /// <param name="i">The node that we want to create the edge from</param>
        /// <param name="j">The node that we want to create the edge to</param>
        /// <parma name="e">The edge that we want to add to the graph</parma>
        /// <returns>The mewly created edge</returns>
        E AddEdge(N i, N j, E e);

        /// <summary>
        /// Method used to add a new Node to the graph
        /// </summary>
        /// <param name="n">the node that want to add to the graph.</param>
        /// <returns>True if the node was added to the graph. False if the node already existed</returns>
        bool AddNode(N n);

    }
}
