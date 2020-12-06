using System;
using System.Collections.Generic;
using System.Linq;

namespace Dagifier.Core
{
    public sealed class DagCreator
    {
        public int LayerCount { get; private set; }
        public int NodeCount { get; private set; }

        private readonly double _propbability;
        private static readonly Random random = new Random();

        private Graph dagGraph;

        public DagCreator()
        {
            _propbability = 0.5d;
            LayerCount = 10;
            NodeCount = random.Next(10, 20);
        }

        public DagCreator(int layers) : this()
        {
            LayerCount = 10;
            NodeCount = random.Next(layers, layers * 2);
        }

        public DagCreator(int layers, int nodeCount) : this(layers)
        {
            if (nodeCount < layers)
            {
                LayerCount = nodeCount;
            }
            NodeCount = nodeCount;
        }

        public DagCreator(int layers, int nodeCount, double probability) : this(layers, nodeCount)
        {
            if (probability > 1.0d)
            {
                _propbability = 1.0d;
            }
            if (probability < 0.0d)
            {
                _propbability = 0;
            }
            _propbability = probability;
        }

        /// <summary>
        /// Method used to randomly generate a random DAG using the settings;
        /// </summary>
        /// <returns></returns>
        public DagCreator Generate()
        {
            dagGraph = new Graph(new Node());
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
                    if (random.NextDouble() >= _propbability)
                    {
                        dagGraph.AddEdge(n, nextLayernode);
                    }
                }
            }
            return this;
        }

        /// <summary>
        /// Method used to get a list of Nodes topology sorted from the graph.
        /// </summary>
        /// <returns>A list of nodes that have been topology sorted using Khan's algorithm.</returns>
        public List<Node> TopologySortedGraph()
        {
            return Sorting.KhansTopologySort(dagGraph.Nodes, dagGraph.Edges);
        }
    }
}
