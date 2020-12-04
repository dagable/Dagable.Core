using System;
using System.Linq;

namespace Dagifier.Core
{
    public sealed class GraphCreator
    {
        public int LayerCount { get; private set; }
        public int NodeCount { get; private set; }

        private const double PROBABILITY = 0.50d;

        private static readonly Random random = new Random();

        public GraphCreator Setup(int layers)
        {
            LayerCount = layers;
            NodeCount = random.Next(layers, layers * 2);
            return this;
        }

        public GraphCreator Setup(int layers, int nodeCount)
        {
            LayerCount = layers;
            NodeCount = nodeCount;
            return this;
        }

        public Graph Execute()
        {
            var graph = new Graph(new Node());
            for(int i = 0; i < NodeCount; ++i)
            {
                var layer = random.Next(1, LayerCount);
                if (i < LayerCount) layer = i;
                graph.AddNode(new Node(i, layer));
            }
            
            foreach(Node n in graph.Nodes)
            {
                var nextLayerNodes = graph.Nodes.Where(x => x.Layer == n.Layer + 1);
                // If the current node is root node, then we want to connect to all those in the first layer.
                if(n.Layer == 0)
                {
                    graph.AddEdges(n, nextLayerNodes);
                    continue;
                }

                foreach(Node nextLayernode in nextLayerNodes)
                {
                    if(random.NextDouble() >= PROBABILITY)
                    {
                        graph.AddEdge(n, nextLayernode);
                    }
                }
            }

            return graph;
        }
    }
}
