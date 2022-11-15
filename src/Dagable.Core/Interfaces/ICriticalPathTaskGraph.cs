using Dagable.Core.JsonConverters;
using Dagable.Core.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Dagable.Core
{
    [JsonConverter(typeof(CriticalPathTaskGraphJsonConverter))]
    public interface ICriticalPathTaskGraph : IStandardTaskGraph<CriticalPathNode, CriticalPathEdge>
    {
        List<CriticalPathEdge> GetCriticalPathEdges { get; }
        int GetCriticalPathLength { get; }
    }
}
