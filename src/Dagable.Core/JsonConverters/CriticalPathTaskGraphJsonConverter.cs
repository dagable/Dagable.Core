using Dagable.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using static Dagable.Core.TaskGraph;

namespace Dagable.Core.JsonConverters
{
    internal class CriticalPathTaskGraphJsonConverter : JsonConverter<ICriticalPathTaskGraph>
    {
        private const string EDGES_CONST = "edges";
        private const string NODES_CONST = "nodes";
        private readonly string[] JSON_PROPERTIES = new[] { EDGES_CONST, NODES_CONST };

        public override ICriticalPathTaskGraph Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var edgeCollection = new HashSet<CriticalPathEdge>();
            var nodeCollection = new HashSet<CriticalPathNode>();
            var graph = new Graph<CriticalPathNode, CriticalPathEdge>();
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }
            reader.Read();
            while (reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }
                string propertyName = reader.GetString();
                if (!JSON_PROPERTIES.Contains(propertyName))
                {
                    throw new JsonException("property should be edges or nodes");
                }
                reader.Read();
                if (reader.TokenType != JsonTokenType.StartArray)
                {
                    throw new JsonException("should be the start of the nodes/edges array");
                }

                switch (propertyName)
                {
                    case EDGES_CONST:
                        reader.Read();
                        while (reader.TokenType != JsonTokenType.EndArray)
                        {
                            if (reader.TokenType != JsonTokenType.StartObject)
                            {
                                throw new JsonException("should be the start of and edges object");
                            }
                            int commTime = 0;
                            var idFrom = 0;
                            var idTo = 0;
                            reader.Read();
                            while (reader.TokenType != JsonTokenType.EndObject)
                            {
                                string edgePropName = reader.GetString();
                                switch (edgePropName)
                                {
                                    case "id":
                                    case "color":
                                        reader.Read();
                                        break;
                                    case "from":
                                        reader.Read();
                                        idFrom = reader.GetInt32();
                                        if (!nodeCollection.Any(x => x.Id == idFrom))
                                        {
                                            nodeCollection.Add(new CriticalPathNode(idFrom));
                                        }
                                        break;
                                    case "to":
                                        reader.Read();
                                        idTo = reader.GetInt32();
                                        if (!nodeCollection.Any(x => x.Id == idTo))
                                        {
                                            nodeCollection.Add(new CriticalPathNode(idTo));
                                        }
                                        break;
                                    case "label":
                                        reader.Read();
                                        commTime = Convert.ToInt32(reader.GetString());
                                        break;
                                    default:
                                        throw new JsonException("Invalid edge property");
                                }
                                reader.Read();
                            }
                            edgeCollection.Add(new CriticalPathEdge(nodeCollection.First(x => x.Id == idFrom), nodeCollection.First(x => x.Id == idTo), commTime));
                            reader.Read();
                        }
                        break;
                    case NODES_CONST:
                        reader.Read();
                        while (reader.TokenType != JsonTokenType.EndArray)
                        {
                            if (reader.TokenType != JsonTokenType.StartObject)
                            {
                                throw new JsonException("should be the start of and node object");
                            }
                            var level = 0;
                            var compTime = 0;
                            var id = 0;
                            reader.Read();
                            while (reader.TokenType != JsonTokenType.EndObject)
                            {
                                string edgePropName = reader.GetString();
                                switch (edgePropName)
                                {
                                    case "id":
                                        reader.Read();
                                        id = reader.GetInt32();
                                        break;
                                    case "level":
                                        reader.Read();
                                        level = reader.GetInt32();
                                        break;
                                    case "label":
                                        reader.Read();
                                        var labelString = reader.GetString();
                                        compTime = Convert.ToInt32(Regex.Match(labelString.Split('_')[1], @"\d+").Value);
                                        break;
                                    default:
                                        throw new JsonException("Invalid node property");
                                }
                                reader.Read();
                            }
                            if(nodeCollection.Any(x => x.Id == id))
                            {
                                var node = nodeCollection.First(x => x.Id == id);
                                node.UpdateLayer(level);
                                node.ComputationTime = compTime;
                            } else
                            {
                                nodeCollection.Add(new CriticalPathNode(id, level, compTime));
                            }
                            
                            reader.Read();
                        }
                        break;
                    default:
                        throw new JsonException("property should be edges or nodes");
                }
                reader.Read();
            }

            nodeCollection.ToList().ForEach(x => graph.AddNode(x));
            edgeCollection.ToList().ForEach(x => graph.AddEdge(x));
            var createdGraph = new CriticalPath(nodeCollection.Max(x => x.Layer))
            {
                dagGraph = graph,
            };
            createdGraph.DetermineCriticalPathLength();
            return createdGraph;
        }


        public override void Write(Utf8JsonWriter writer, ICriticalPathTaskGraph value, JsonSerializerOptions options)
        {
            var nodes = value.Nodes.Where(x => x.Layer != value.Layers);
            var edges = value.Edges.Where(x => x.NextNode.Layer != value.Layers).ToArray();
            var criticalEdges = value.GetCriticalPathEdges;
            writer.WriteStartObject();
            writer.WriteStartArray(EDGES_CONST);
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
            writer.WriteStartArray(NODES_CONST);
            foreach (var node in nodes)
            {
                writer.WriteStartObject();
                writer.WriteNumber("id", node.Id);
                writer.WriteNumber("level", node.Layer);
                writer.WriteString("label", $"n{node.Id}_c{node.ComputationTime}");
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}
