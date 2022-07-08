using System.Diagnostics.CodeAnalysis;

namespace Dagable.Core
{
    public interface IEdge<N, E> where N : INode<N>
    {
        N NextNode { get; set; }
        N PrevNode { get; set; }

        int CompareTo([AllowNull] E other);
        bool Equals(object obj);
        int GetHashCode();
    }
}