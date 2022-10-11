using Dagable.Core.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Dagable.Core
{
    public partial class DAG
    {
        public class CriticalPathTaskGraph : StandardTaskGraph, ICriticalPathTaskGraph
        {
            private int MinComp { get; set; }
            private int MaxComp { get; set; }
            private int MinComm { get; set; }
            private int MaxComm { get; set; }

            public new Graph<CPathNode, CPathEdge> dagGraph;

            protected new readonly Dictionary<int, List<CPathNode>> _layeredNodes = new Dictionary<int, List<CPathNode>>();

            private List<CPathEdge> CriticalPathEdges;

            HashSet<CPathEdge> ICriticalPathTaskGraph.Edges => dagGraph.Edges;
            HashSet<CPathNode> ICriticalPathTaskGraph.Nodes => dagGraph.Nodes;

            public CriticalPathTaskGraph() : base() {
                MinComp = random.Next(1, 10);
                MaxComp = random.Next(10, 20);
                MinComm = random.Next(1, 5);
                MaxComm = random.Next(5, 10);
            }

            public CriticalPathTaskGraph(int layers) : base(layers)
            {
            }

            public CriticalPathTaskGraph(int layers, int nodeCount, double probability) : base(layers, nodeCount, probability)
            {
                MinComp = random.Next(1,10);
                MaxComp = random.Next(10,20);
                MinComm = random.Next(1,5);
                MaxComm = random.Next(5,10);
            }

            public CriticalPathTaskGraph(int minComp, int maxComp, int minComm, int maxComm, int layers, int nodeCount, double probability) : base(layers, nodeCount, probability)
            {
                MinComp = minComp;
                MaxComp = maxComp;
                MinComm = minComm;
                MaxComm = maxComm;
            }

            public new CriticalPathTaskGraph Generate()
            {
                dagGraph = new Graph<CPathNode, CPathEdge>(new CPathNode());
                for (int i = 0; i < NodeCount; ++i)
                {
                    var layer = random.Next(1, LayerCount);
                    var compTime = random.Next(MinComp, MaxComp);
                    if (i < LayerCount) layer = i;
                    dagGraph.AddNode(new CPathNode(i, layer, compTime));
                }

                foreach (CPathNode n in dagGraph.Nodes)
                {
                    var nextLayerNodes = dagGraph.Nodes.Where(x => x.Layer == n.Layer + 1);
                    // If the current node is root node, then we want to connect to all those in the first layer.
                    if (n.Layer == 0)
                    {
                        dagGraph.AddEdges(n, nextLayerNodes);
                        continue;
                    }

                    foreach (CPathNode nextLayernode in nextLayerNodes)
                    {
                        var probability = random.NextDouble();
                        if (probability <= _propbability)
                        {
                            dagGraph.AddEdge(new CPathEdge(n, nextLayernode, random.Next(MinComm, MaxComm)));
                        }
                    }
                }

                var nodesWithNoPredecessorNodes = dagGraph.Nodes.Where(x => !x.PredecessorNodes.Any() && x.Layer != 0);

                foreach (CPathNode n in nodesWithNoPredecessorNodes.ToList())
                {
                    var layer = n.Layer - 1;
                    if (!_layeredNodes.ContainsKey(layer))
                    {
                        _layeredNodes.Add(layer, dagGraph.Nodes.Where(x => x.Layer == layer).ToList());
                    }
                    var prevLayerNode = _layeredNodes[layer][random.Next(_layeredNodes[layer].Count)];
                    dagGraph.AddEdge(new CPathEdge(prevLayerNode, n, random.Next(MinComm, MaxComm)));
                }

                //add node on last layer to create critical path
                dagGraph.AddNode(new CPathNode(dagGraph.Nodes.Count, LayerCount, 0));

                var node = dagGraph.Nodes.First(x => x.Layer == LayerCount);

                var nodesOnLastLayer = dagGraph.Nodes.Where(x => x.Layer == LayerCount - 1);
                foreach(var n in nodesOnLastLayer)
                {
                    dagGraph.AddEdge(new CPathEdge(n, node,  0));
                }
                return this;
            }

            private int DFS(CPathNode n, bool[] discoveredNodes, int[] departure, int time)
            {
                discoveredNodes[n.Id] = true;

                foreach(CPathEdge edge in dagGraph.Edges.Where(x => x.PrevNode.Id == n.Id))
                {
                    var u  = edge.NextNode;
                    if (!discoveredNodes[u.Id])
                    {
                        time = DFS(u, discoveredNodes, departure, time);
                    }
                }

                departure[time] = n.Id;
                time++;

                return time;
            }

            internal List<CPathEdge> FindCriticalPath(CPathNode source, CPathNode destination)
            {
                int[] departure = new int[dagGraph.Nodes.Count];
                departure = departure.Select(i => -1).ToArray();

                bool[] discovered = new bool[dagGraph.Nodes.Count];
                int time = 0;

                for(int i = 0; i < dagGraph.Nodes.Count; i++)
                {
                    if (!discovered[i])
                    {
                        time = DFS(dagGraph.Nodes.ElementAt(i), discovered, departure, time);
                    }
                }

                int[] cost = new int[dagGraph.Nodes.Count];
                List<CPathEdge>[] edgeCost = new List<CPathEdge>[dagGraph.Nodes.Count];
                edgeCost = edgeCost.Select(x => new List<CPathEdge>()).ToArray();
                cost = cost.Select(i => int.MaxValue).ToArray();

                cost[source.Id] = source.ComputationTime * -1;

                for (int i = dagGraph.Nodes.Count - 1; i >= 0; i--)
                {
                    // for each vertex in topological order,
                    // relax the cost of its adjacent vertices
                    int v = departure[i];
                    foreach(var edge in dagGraph.Edges.Where(x => x.PrevNode.Id == v))
                    {
                        // edge `e` from `v` to `u` having weight `w`
                        int u = edge.NextNode.Id;
                        int w = (edge.CommTime + edge.NextNode.ComputationTime) * -1; // make edge weight negative

                        // if the distance to destination `u` can be shortened by
                        // taking edge (v, u), then update cost to the new lower value
                        if (cost[v] != int.MaxValue && cost[v] + w < cost[u])
                        {
                            edgeCost[u] = new List<CPathEdge> { edge };
                            edgeCost[u].AddRange(edgeCost[v]);
                            cost[u] = cost[v] + w;
                        }
                    }
                }
                CriticalPathEdges = edgeCost[destination.Id];
                return edgeCost[destination.Id];
            }

            public int DetermineCriticalPathLength()
            {
                if (CriticalPathEdges == null)
                {
                    FindCriticalPath(dagGraph.Nodes.First(x => x.Layer == 0), dagGraph.Nodes.First(x => x.Layer == LayerCount));
                }

                return 
                    CriticalPathEdges.Select(x => x.NextNode).Union(CriticalPathEdges.Select(x => x.PrevNode)).Sum(x => x.ComputationTime) +
                    CriticalPathEdges.Sum(x => x.CommTime);
            }

            public override string ToString()
            {
                var criticalPathEdges = FindCriticalPath(dagGraph.Nodes.First(x => x.Layer == 0), dagGraph.Nodes.First(x => x.Layer == LayerCount));
                return JsonConvert.SerializeObject(new
                {
                    Nodes = dagGraph.Nodes.Where(x => x.Layer != LayerCount).Select(x => new { id = x.Id, label = $"{x.ComputationTime}", level = x.Layer }),
                    Edges = dagGraph.Edges.Where(x => x.NextNode.Layer != LayerCount).Select((x, i) => new { label = $"{x.CommTime}", id = $"edge_{i}", from = x.PrevNode.Id, to = x.NextNode.Id, color = criticalPathEdges.Any(e => e.PrevNode.Id == x.PrevNode.Id && e.NextNode.Id == x.NextNode.Id) ? "#f16f4e" : "black"})
                });
            }                      
        }
    }
}
