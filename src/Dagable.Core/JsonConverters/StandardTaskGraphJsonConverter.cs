using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dagable.Core.JsonConverters
{
    public class StandardTaskGraphJsonConverter : JsonConverter<IStandardTaskGraph<StandardNode, StandardEdge<StandardNode>>>
    {
        public override IStandardTaskGraph<StandardNode, StandardEdge<StandardNode>> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, IStandardTaskGraph<StandardNode, StandardEdge<StandardNode>> value, JsonSerializerOptions options)
        {
            var nodes = value.Nodes.Where(x => x.Layer != value.Layers);
            var edges = value.Edges.Where(x => x.NextNode.Layer != value.Layers).ToArray();
            writer.WriteStartObject();
            writer.WriteStartArray("nodes");
            foreach (var node in nodes)
            {
                writer.WriteStartObject();
                writer.WriteNumber("id", node.Id);
                writer.WriteNumber("level", node.Layer);
                writer.WriteString("label", $"n{node.Id}");
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.WriteStartArray("edges");
            for (int i = 0; i < edges.Length; i++)
            {
                writer.WriteStartObject();
                writer.WriteString("id", $"edge_{i}");
                writer.WriteNumber("from", edges[i].PrevNode.Id);
                writer.WriteNumber("to", edges[i].NextNode.Id);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}
