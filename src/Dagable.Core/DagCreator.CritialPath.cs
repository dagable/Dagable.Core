using Dagable.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Dagable.Core
{
    public sealed partial class DagCreator
    {
        public class CriticalPath : Standard, IDagCreation<CriticalPath>
        {
            private int MinComp { get; set; }
            private int MaxComp { get; set; }
            private int MinComm { get; set; }
            private int MaxComm { get; set; }

            internal new Graph<CPathNode, CPathEdge> dagGraph;

            protected new readonly Dictionary<int, List<CPathNode>> _layeredNodes = new Dictionary<int, List<CPathNode>>();

            public CriticalPath() : base() {
                MinComp = random.Next(1, 10);
                MaxComp = random.Next(10, 20);
                MinComm = random.Next(1, 5);
                MaxComm = random.Next(5, 10);
            }


            public CriticalPath(int layers, int nodeCount, double probability) : base(layers, nodeCount, probability)
            {
                MinComp = random.Next(1,10);
                MaxComp = random.Next(10,20);
                MinComm = random.Next(1,5);
                MaxComm = random.Next(5,10);
            }
            
            public CriticalPath(int minComp, int maxComp, int minComm, int maxComm,  int layers, int nodeCount, double probability) : this(layers, nodeCount, probability)
            {
                MinComp = minComp;
                MaxComp = maxComp;
                MinComm = minComm;
                MaxComm = maxComm;
            }


            public new CriticalPath Generate()
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
                            dagGraph.AddEdge(n, nextLayernode);
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
                    dagGraph.AddEdge(prevLayerNode, n);
                }

                foreach (var edge in dagGraph.Edges)
                {
                    edge.coomunicationTime = random.Next(MinComm, MaxComm);
                }

                return this;
            }
        }
    }
}
