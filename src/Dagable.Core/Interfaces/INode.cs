namespace Dagable.Core
{
    internal interface INode
    {
        /// <summary>
        /// Method used to add a successor or "next node" to this node.
        /// </summary>
        /// <param name="n">The node that is a successor to this node</param>
        /// <returns>True if the node was added to the collection; false if it was already a successor.</returns>
        bool AddSuccessor(Node n);

        /// <summary>
        /// Method used to add a predecessor node or "previous node" to this node
        /// </summary>
        /// <param name="n">The node that is a predessor to this node</param>
        /// <returns>true if the node was added to the collcetion; false if it was already a predecessor.</returns>
        bool AddPredecessor(Node n);
    }
}
