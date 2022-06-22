namespace Dagable.Core
{
    internal interface INode
    {
        /// <summary>
        /// Method used to add a successor or "next node" to this node.
        /// </summary>
        /// <param name="n">The node that is a successor to this node</param>
        /// <returns>True if the node was added to the collection; false if it was already a successor.</returns>
        Node AddSuccessor(Node n);

        /// <summary>
        /// Method used to add a predecessor node or "previous node" to this node
        /// </summary>
        /// <param name="n">The node that is a predessor to this node</param>
        /// <returns>true if the node was added to the collcetion; false if it was already a predecessor.</returns>
        Node AddPredecessor(Node n);

        /// <summary>
        /// Will set the layer to be on the first layer.
        /// This should only be calle when the node has no predecessor nodes. Used for display purposes.
        /// </summary>
        Node UpdateLayer(int layer);
    }
}
