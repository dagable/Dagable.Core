using Dagable.Core.Models;
using System.Collections.Generic;

namespace Dagable.Core
{
    public interface ICriticalPathTaskGraph : IStandardTaskGraph
    {
        HashSet<CPathNode> Nodes { get; }
        HashSet<CPathEdge> Edges { get; }
    }
}
