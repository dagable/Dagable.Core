﻿namespace Dagable.Core
{
    public interface INode<N> where N : INode<N>
    {
        /// <summary>
        /// Gets the ID of this node.
        /// </summary>
        /// <returns>The ID of the node.</returns>
        int GetId();

        /// <summary>
        /// Method used to add a successor or "next node" to this node.
        /// </summary>
        /// <param name="n">The node that is a successor to this node</param>
        /// <returns>True if the node was added to the collection; false if it was already a successor.</returns>
        N AddSuccessor(N n);

        /// <summary>
        /// Method used to add a predecessor node or "previous node" to this node
        /// </summary>
        /// <param name="n">The node that is a predessor to this node</param>
        /// <returns>true if the node was added to the collcetion; false if it was already a predecessor.</returns>
        N AddPredecessor(N n);

        /// <summary>
        /// Will set the layer to be on the first layer.
        /// This should only be calle when the node has no predecessor nodes. Used for display purposes.
        /// </summary>
        N UpdateLayer(int layer);

        /// <summary>
        /// Method that compares one node to another.
        /// </summary>
        /// <typeparam name="T">Either a StandardNode or CriticalPathNode</typeparam>
        /// <param name="prevNode">Another node to compare to</param>
        /// <returns>0 if they are the same./returns>
        int CompareTo<T>(N prevNode) where T : INode<N>, new();
    }
}
