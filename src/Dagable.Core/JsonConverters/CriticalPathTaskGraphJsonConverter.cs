using Dagable.Core.Models;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Dagable.Core.JsonConverters
{
    internal class CriticalPathTaskGraphJsonConverter : JsonConverter<ICriticalPathTaskGraph>
    {
        public override ICriticalPathTaskGraph Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, ICriticalPathTaskGraph value, JsonSerializerOptions options)
        {
            var nodes = value.Nodes.Where(x => x.Layer != value.Layers);
            var edges = value.Edges.Where(x => x.NextNode.Layer != value.Layers).ToArray();
            var criticalEdges = value.GetCriticalPathEdges;
            writer.WriteStartObject();
            writer.WriteStartObject("graph");
            writer.WriteStartArray("nodes");
            foreach (var node in nodes)
            {
                writer.WriteStartObject();
                writer.WriteNumber("id", node.Id);
                writer.WriteNumber("level", node.Layer);
                writer.WriteString("label", $"{node.Id}_{node.ComputationTime}");
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
                writer.WriteString("label", $"{edges[i].CommTime}");
                writer.WriteString("color", criticalEdges.Any(e => e.PrevNode.Id == edges[i].PrevNode.Id && e.NextNode.Id == edges[i].NextNode.Id) ? "#f16f4e" : "black");
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.WriteEndObject();
        }
    }
}
