namespace Dagable.Core
{
    public interface IEdge<N> where N : INode<N>
    {
        N NextNode { get; set; }
        N PrevNode { get; set; }

        /// <summary>
        /// Checks to see if two graph Edges are equal.
        /// </summary>
        /// <param name="obj">Some object, expected to be an Edge to compare</param>
        /// <returns>True if two edges are the same false otherwise</returns>
        bool Equals(object obj);

        /// <summary>
        /// Determine a hashcode for this edge
        /// </summary>
        /// <returns></returns>
        int GetHashCode();
    }
}