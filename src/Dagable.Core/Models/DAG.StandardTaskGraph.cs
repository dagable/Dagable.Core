using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dagable.Core
{
    public partial class DAG
    {
        static readonly Random random = new Random();

        public class StandardTaskGraph : IStandardTaskGraph<StandardTaskGraph>
        {
            protected int LayerCount { get; private set; }
            protected int NodeCount { get; private set; }

            protected double _propbability;

            public Graph<Node, Edge<Node>> dagGraph;

            protected readonly Dictionary<int, List<Node>> _layeredNodes = new Dictionary<int, List<Node>>();

            public StandardTaskGraph()
            {
                _propbability = 0.5d;
                LayerCount = 10;
                NodeCount = random.Next(10, 20);
            }

            public StandardTaskGraph(int layers)
            {
                LayerCount = layers;
                NodeCount = random.Next(layers, layers * 2);
            }

            public StandardTaskGraph(int layers, int nodeCount) : this (layers)
            {
                if (nodeCount < layers)
                {
                    LayerCount = nodeCount;
                }
                NodeCount = nodeCount;
            }

            public StandardTaskGraph(int layers, int nodeCount, double probability) : this(layers, nodeCount)
            {
                if (probability > 1.0d)
                {
                    probability = 1.0d;
                }
                if (probability < 0.0d)
                {
                    probability = 0;
                }
                _propbability = probability;
            }

            /// <summary>
            /// Method used to randomly generate a random DAG using the settings;
            /// </summary>
            /// <returns></returns>
            public StandardTaskGraph Generate()
            {
                dagGraph = new Graph<Node, Edge<Node>>(new Node());
                for (int i = 0; i < NodeCount; ++i)
                {
                    var layer = random.Next(1, LayerCount);
                    if (i < LayerCount) layer = i;
                    dagGraph.AddNode(new Node(i, layer));
                }

                foreach (Node n in dagGraph.Nodes)
                {
                    var nextLayerNodes = dagGraph.Nodes.Where(x => x.Layer == n.Layer + 1);
                    // If the current node is root node, then we want to connect to all those in the first layer.
                    if (n.Layer == 0)
                    {
                        dagGraph.AddEdges(n, nextLayerNodes);
                        continue;
                    }

                    foreach (Node nextLayernode in nextLayerNodes)
                    {
                        var probability = random.NextDouble();
                        if (probability <= _propbability)
                        {
                            dagGraph.AddEdge(new Edge<Node>(n, nextLayernode));
                        }
                    }
                }

                var nodesWithNoPredecessorNodes = dagGraph.Nodes.Where(x => !x.PredecessorNodes.Any() && x.Layer != 0);

                foreach (Node n in nodesWithNoPredecessorNodes.ToList())
                {
                    var layer = n.Layer - 1;
                    if (!_layeredNodes.ContainsKey(layer))
                    {
                        _layeredNodes.Add(layer, dagGraph.Nodes.Where(x => x.Layer == layer).ToList());
                    }
                    var prevLayerNode = _layeredNodes[layer][random.Next(_layeredNodes[layer].Count)];
                    dagGraph.AddEdge(new Edge<Node>(prevLayerNode, n));
                }

                return this;
            }

            /// <summary>
            /// Method used to get a list of Nodes topology sorted from the graph.
            /// </summary>
            /// <returns>A list of nodes that have been topology sorted using Khan's algorithm.</returns>
            internal List<Node> TopologySortedGraph()
            {
                return Sorting.KhansTopologySort(dagGraph.Nodes, dagGraph.Edges);
            }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(new
                {
                    Nodes = dagGraph.Nodes.Select(x => new { id = x.Id, label = $"{x.Id}", level = x.Layer }),
                    Edges = dagGraph.Edges.Select((x, i) => new { id = $"edge_{i}", from = x.PrevNode.Id, to = x.NextNode.Id })
                });
            }
        }
    }
}
