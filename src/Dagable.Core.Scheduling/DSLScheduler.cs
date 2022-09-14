using Dagable.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Dagable.Core.DAG;

namespace Dagable.Core.Scheduling
{
    public class DSLScheduler
    {
        private readonly int _processorCount;
        private readonly CriticalPath _graph;
        private readonly Dictionary<CPathNode, int> NodeBLevelMappings;
        private readonly Dictionary<CPathNode, int> NodeTLevelMappings;
        private readonly List<CPathNode> TopologySortedNodes;
        private readonly Dictionary<int, List<ScheduledNode>> processorMapping = new Dictionary<int, List<ScheduledNode>>();


        public DSLScheduler(int processorCount, CriticalPath graph)
        {
            _processorCount = processorCount;
            _graph = graph;
            TopologySortedNodes = Sorting.KhansTopologySort(graph.dagGraph.Nodes, new HashSet<CPathEdge>(graph.dagGraph.Edges));
            NodeBLevelMappings = CoreFunctions.ComputerStaticBLevel(TopologySortedNodes, graph.dagGraph.Edges);
            NodeTLevelMappings = CoreFunctions.ComputerTLevel(TopologySortedNodes, graph.dagGraph.Edges);
            for (int i = 0; i < _processorCount; i++)
            {
                processorMapping.Add(i, new List<ScheduledNode>());
            }
        }

        public Dictionary<int, List<ScheduledNode>> Schedule()
        {
            var readyNodePool = new HashSet<CPathNode> { _graph.dagGraph.Nodes.First(x => x.Layer == 0) };
            var processedNodes = new HashSet<CPathNode>();

            while(readyNodePool.Any())
            {
                var earliestStartTime = 0;
                var node = readyNodePool.First();
                var processorDL = new int[_processorCount];
                for(int i = 0; i < _processorCount; i++)
                {                 
                    var processorWeight = 0;
                    if (processorMapping[i].Any())
                    {
                        processorWeight = processorMapping[i].Max(x => x.EndAt);
                    }
                    // https://www.sciencedirect.com/science/article/pii/S0898122112001915#br000020
                    // http://charm.cs.uiuc.edu/users/arya/docs/6.pdf
                    var dl = NodeBLevelMappings[node] - NodeTLevelMappings[node];
                    processorDL[i] = dl;
                    if(NodeBLevelMappings[node] - dl == 0)
                    {
                        processorMapping[0].Add(new ScheduledNode(node, earliestStartTime + processorWeight, node.ComputationTime + processorWeight));
                        break;
                    }
                    if (dl > earliestStartTime)
                    {
                        earliestStartTime = dl + processorWeight;
                    } else
                    {
                        processorMapping[i].Add(new ScheduledNode(node, earliestStartTime + processorWeight, node.ComputationTime + processorWeight));
                        foreach (var child in node.SuccessorNodes)
                        {
                            if (!processedNodes.Contains(child))
                            {
                                readyNodePool.Add(child);
                            }
                        }
                        readyNodePool.Remove(node);
                        processedNodes.Add(node);
                    }
                }
            }

            return processorMapping;
        }
    }
}
