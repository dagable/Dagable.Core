using Dagable.Core.Models;
using System.Collections.Generic;

namespace Dagable.Core
{
    public interface ICriticalPathTaskGraph : IStandardTaskGraph
    {
        HashSet<CriticalPathNode> Nodes { get; }
        HashSet<CriticalPathEdge> Edges { get; }
    }
}
