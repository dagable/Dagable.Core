namespace Dagable.Core
{
    public interface IEdge<N> where N : INode<N>
    {
        N NextNode { get; set; }
        N PrevNode { get; set; }
        bool Equals(object obj);
        int GetHashCode();
    }
}