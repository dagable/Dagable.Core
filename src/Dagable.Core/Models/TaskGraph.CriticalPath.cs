using Dagable.Core.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Dagable.Core
{
    public partial class TaskGraph
    {
        public class CriticalPath : Standard, ICriticalPathTaskGraph
        {
            private int MinComp { get; set; }
            private int MaxComp { get; set; }
            private int MinComm { get; set; }
            private int MaxComm { get; set; }

            public new Graph<CriticalPathNode, CriticalPathEdge> dagGraph;

            protected new readonly Dictionary<int, List<CriticalPathNode>> _layeredNodes = new();

            private List<CriticalPathEdge> CriticalPathEdges;

            HashSet<CriticalPathNode> IStandardTaskGraph<CriticalPathNode, CriticalPathEdge>.Nodes => dagGraph.Nodes;

            HashSet<CriticalPathEdge> IStandardTaskGraph<CriticalPathNode, CriticalPathEdge>.Edges => dagGraph.Edges;

            List<CriticalPathEdge> ICriticalPathTaskGraph.GetCriticalPathEdges => CriticalPathEdges;

            public CriticalPath() {
                MinComp = random.Next(1, 10);
                MaxComp = random.Next(10, 20);
                MinComm = random.Next(1, 5);
                MaxComm = random.Next(5, 10);
            }

            public CriticalPath(int layers) : base(layers)
            {
            }

            public CriticalPath(int layers, int nodeCount, double probability) : base(layers, nodeCount, probability)
            {
                MinComp = random.Next(1,10);
                MaxComp = random.Next(10,20);
                MinComm = random.Next(1,5);
                MaxComm = random.Next(5,10);
            }

            public CriticalPath(int minComp, int maxComp, int minComm, int maxComm, int layers, int nodeCount, double probability) : base(layers, nodeCount, probability)
            {
                MinComp = minComp;
                MaxComp = maxComp;
                MinComm = minComm;
                MaxComm = maxComm;
            }

            public new CriticalPath Generate()
            {
                dagGraph = new Graph<CriticalPathNode, CriticalPathEdge>(new CriticalPathNode());
                for (int i = 0; i < NodeCount; ++i)
                {
                    var layer = random.Next(1, LayerCount);
                    var compTime = random.Next(MinComp, MaxComp);
                    if (i < LayerCount) layer = i;
                    dagGraph.AddNode(new CriticalPathNode(i, layer, compTime));
                }

                foreach (CriticalPathNode n in dagGraph.Nodes)
                {
                    var nextLayerNodes = dagGraph.Nodes.Where(x => x.Layer == n.Layer + 1);
                    // If the current node is root node, then we want to connect to all those in the first layer.
                    if (n.Layer == 0)
                    {
                        dagGraph.AddEdges(n, nextLayerNodes);
                        continue;
                    }

                    foreach (CriticalPathNode nextLayernode in nextLayerNodes)
                    {
                        var probability = random.NextDouble();
                        if (probability <= _propbability)
                        {
                            dagGraph.AddEdge(new CriticalPathEdge(n, nextLayernode, random.Next(MinComm, MaxComm)));
                        }
                    }
                }

                var nodesWithNoPredecessorNodes = dagGraph.Nodes.Where(x => !x.PredecessorNodes.Any() && x.Layer != 0);

                foreach (CriticalPathNode n in nodesWithNoPredecessorNodes.ToList())
                {
                    var layer = n.Layer - 1;
                    if (!_layeredNodes.ContainsKey(layer))
                    {
                        _layeredNodes.Add(layer, dagGraph.Nodes.Where(x => x.Layer == layer).ToList());
                    }
                    var prevLayerNode = _layeredNodes[layer][random.Next(_layeredNodes[layer].Count)];
                    dagGraph.AddEdge(new CriticalPathEdge(prevLayerNode, n, random.Next(MinComm, MaxComm)));
                }

                //add node on last layer to create critical path
                dagGraph.AddNode(new CriticalPathNode(dagGraph.Nodes.Count, LayerCount, 0));

                var node = dagGraph.Nodes.First(x => x.Layer == LayerCount);

                var nodesOnLastLayer = dagGraph.Nodes.Where(x => x.Layer == LayerCount - 1);
                foreach(var n in nodesOnLastLayer)
                {
                    dagGraph.AddEdge(new CriticalPathEdge(n, node,  0));
                }
                FindCriticalPath(dagGraph.Nodes.First(x => x.Layer == 0), dagGraph.Nodes.First(x => x.Layer == LayerCount));
                return this;
            }

            /// <summary>
            /// Depth first search.
            /// 
            /// Based on https://en.wikipedia.org/wiki/Depth-first_search#Pseudocode
            /// </summary>
            /// <param name="n">The current Node to find the adjacent nodes for processing</param>
            /// <param name="discoveredNodes">An array of node length with true / false values indiciting if they have been visited.</param>
            /// <param name="departure">0..n of when a node is departed from</param>
            /// <param name="step">The current step of the search.</param>
            /// <returns></returns>
            private int DepthFirstSearch(CriticalPathNode n, bool[] discoveredNodes, int[] departure, int step)
            {
                discoveredNodes[n.Id] = true;

                foreach(CriticalPathEdge edge in dagGraph.Edges.Where(x => x.PrevNode.Id == n.Id))
                {
                    var u  = edge.NextNode;
                    if (!discoveredNodes[u.Id])
                    {
                        step = DepthFirstSearch(u, discoveredNodes, departure, step);
                    }
                }

                departure[step] = n.Id;
                step++;

                return step;
            }

            /// <summary>
            /// Method used to determine the critical path of a graph.
            /// </summary>
            /// <param name="source">The source node to start the search from</param>
            /// <param name="destination">The destination node</param>
            /// <returns>A List of edges that are on the critical path</returns>
            internal List<CriticalPathEdge> FindCriticalPath(CriticalPathNode source, CriticalPathNode destination)
            {
                int[] departure = new int[dagGraph.Nodes.Count];
                departure = departure.Select(i => -1).ToArray();

                bool[] discovered = new bool[dagGraph.Nodes.Count];
                int time = 0;

                for(int i = 0; i < dagGraph.Nodes.Count; i++)
                {
                    if (!discovered[i])
                    {
                        time = DepthFirstSearch(dagGraph.Nodes.ElementAt(i), discovered, departure, time);
                    }
                }

                int[] cost = new int[dagGraph.Nodes.Count];
                List<CriticalPathEdge>[] edgeCost = new List<CriticalPathEdge>[dagGraph.Nodes.Count];
                edgeCost = edgeCost.Select(x => new List<CriticalPathEdge>()).ToArray();
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
                            edgeCost[u] = new List<CriticalPathEdge> { edge };
                            edgeCost[u].AddRange(edgeCost[v]);
                            cost[u] = cost[v] + w;
                        }
                    }
                }
                CriticalPathEdges = edgeCost[destination.Id];
                return edgeCost[destination.Id];
            }

            /// <summary>
            /// Method used to determine the length of the critical Path
            /// </summary>
            /// <returns>The length of the critical path for a task graph.</returns>
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
        }
    }
}
