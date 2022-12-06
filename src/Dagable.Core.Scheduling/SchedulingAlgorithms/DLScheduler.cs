using Dagable.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dagable.Core.Scheduling
{
    public class DLScheduler : IScheduler
    {
        private readonly int _processorCount;
        private readonly ICriticalPathTaskGraph _graph;
        private readonly Dictionary<CriticalPathNode, int> NodeBLevelMappings;
        private readonly Dictionary<CriticalPathNode, int> NodeStaticBLevelMappings;
        private readonly Dictionary<CriticalPathNode, int> NodeTLevelMappings;
        private readonly Dictionary<int, List<ScheduledNode>> processorMapping = new Dictionary<int, List<ScheduledNode>>();

        public DLScheduler(int processorCount, ICriticalPathTaskGraph graph)
        {
            _processorCount = processorCount;
            _graph = graph;
            NodeTLevelMappings = CoreFunctions.ComputerTLevel(Sorting.KhansTopologySort(graph.Nodes, new HashSet<CriticalPathEdge>(graph.Edges)), new HashSet<CriticalPathEdge>(graph.Edges));
            NodeStaticBLevelMappings = CoreFunctions.ComputerStaticBLevel(Sorting.KhansTopologySort(graph.Nodes, new HashSet<CriticalPathEdge>(graph.Edges)));
            NodeBLevelMappings = CoreFunctions.ComputerBLevel(Sorting.KhansTopologySort(graph.Nodes, new HashSet<CriticalPathEdge>(graph.Edges)), new HashSet<CriticalPathEdge>(graph.Edges));
            for (int i = 0; i < _processorCount; i++)
            {
                processorMapping.Add(i, new List<ScheduledNode>());
            }
        }

        /// <inheritdoc cref="IScheduler.Schedule"/>
        public Dictionary<int, List<ScheduledNode>> Schedule()
        {
            var readyNodePool = new HashSet<UnscheduledNode> { new UnscheduledNode(_graph.Nodes.First(x => x.Layer == 0), NodeBLevelMappings[_graph.Nodes.First(x => x.Layer == 0)]) };
            var processedNodes = new Dictionary<int, List<ScheduledNode>>();
            for (int i = 0; i < _processorCount; i++)
            {
                processedNodes[i] = new List<ScheduledNode>();
            }

            while (readyNodePool.Any())
            {
                var NodeProcessorPair = new List<Tuple<UnscheduledNode, int[], int[]>>();
                foreach (var node in readyNodePool.OrderByDescending(x => NodeStaticBLevelMappings[x.Node] - NodeTLevelMappings[x.Node]))
                {
                    var processorDL = new int[_processorCount];
                    var processorELS = new int[_processorCount];
                    for (int i = 0; i < _processorCount; i++)
                    {
                        var earliestStartTime = processedNodes[i].Any() ? processedNodes[i].Max(x => x.EndAt) > NodeTLevelMappings[node.Node] ? processedNodes[i].Max(x => x.EndAt) : NodeTLevelMappings[node.Node] : NodeTLevelMappings[node.Node];
                        processorDL[i] = NodeStaticBLevelMappings[node.Node] - NodeTLevelMappings[node.Node];
                        processorELS[i] = earliestStartTime;
                    }
                    NodeProcessorPair.Add(new Tuple<UnscheduledNode, int[], int[]>(node, processorDL, processorELS));
                }

                var maxDl = NodeProcessorPair.SelectMany(x => x.Item2).Max();
                var maxDlPair = NodeProcessorPair.First(x => x.Item2.Contains(maxDl));
                var processor = maxDlPair.Item3.ToList().IndexOf(maxDlPair.Item3.Min());
                processedNodes[processor].Add(new ScheduledNode(maxDlPair.Item1.Node, maxDlPair.Item3[processor], maxDlPair.Item3[processor] + maxDlPair.Item1.Node.ComputationTime));
                readyNodePool.Remove(maxDlPair.Item1);
                foreach (var childnode in maxDlPair.Item1.Node.SuccessorNodes)
                {
                    if (!processedNodes.Values.SelectMany(x => x).Any(x => x.Id == childnode.Id) && !readyNodePool.Any(x => x.Node.Id == childnode.Id))
                    {
                        readyNodePool.Add(new UnscheduledNode(childnode, NodeBLevelMappings[childnode]));
                    }
                }
            }

            return processedNodes;
        }
    }
}
