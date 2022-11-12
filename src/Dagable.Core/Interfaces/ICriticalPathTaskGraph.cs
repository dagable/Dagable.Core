using Dagable.Core.Models;
using System.Collections.Generic;
using static Dagable.Core.DAG;

namespace Dagable.Core
{
    public interface ICriticalPathTaskGraph : IStandardTaskGraph<CriticalPathTaskGraph>
    {
        HashSet<CPathNode> Nodes { get; }
        HashSet<CPathEdge> Edges { get; }
    }
}
