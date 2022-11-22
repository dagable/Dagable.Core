using Dagable.Core.JsonConverters;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Dagable.Core
{
    [JsonConverter(typeof(StandardTaskGraphJsonConverter))]
    public interface IStandardTaskGraph<N, E> 
    {
        int Layers { get; }
        HashSet<N> Nodes { get; }
        HashSet<E> Edges { get; }
    }
}