using Dagable.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static Dagable.Core.DAG;

namespace Dagable.Core.Scheduling
{
    public class DSLScheduler : IScheduler
    {
        private readonly int _processorCount;
        private readonly CriticalPath _graph;
        private readonly Dictionary<CPathNode, int> NodeBLevelMappings;
        private readonly Dictionary<CPathNode, int> NodeStaticBLevelMappings;
        private readonly Dictionary<CPathNode, int> NodeTLevelMappings;
        private readonly Dictionary<int, List<ScheduledNode>> processorMapping = new Dictionary<int, List<ScheduledNode>>();


        public DSLScheduler(int processorCount, CriticalPath graph)
        {
            _processorCount = processorCount;
            _graph = graph;
            NodeTLevelMappings = CoreFunctions.ComputerTLevel(Sorting.KhansTopologySort(graph.dagGraph.Nodes, new HashSet<CPathEdge>(graph.dagGraph.Edges)), new HashSet<CPathEdge>(graph.dagGraph.Edges));
            NodeStaticBLevelMappings = CoreFunctions.ComputerStaticBLevel(Sorting.KhansTopologySort(graph.dagGraph.Nodes, new HashSet<CPathEdge>(graph.dagGraph.Edges)), new HashSet<CPathEdge>(graph.dagGraph.Edges));
            NodeBLevelMappings = CoreFunctions.ComputerBLevel(Sorting.KhansTopologySort(graph.dagGraph.Nodes, new HashSet<CPathEdge>(graph.dagGraph.Edges)), new HashSet<CPathEdge>(graph.dagGraph.Edges));
            for (int i = 0; i < _processorCount; i++)
            {
                processorMapping.Add(i, new List<ScheduledNode>());
            }
        }

        public Dictionary<int, List<ScheduledNode>> Schedule()
        {
            var readyNodePool = new HashSet<UnscheduledNode> { new UnscheduledNode(_graph.dagGraph.Nodes.First(x => x.Layer == 0), NodeBLevelMappings[_graph.dagGraph.Nodes.First(x => x.Layer == 0)]) };
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
                Debug.WriteLine($"Node: {maxDlPair.Item1.Node.Id + 1} p: {processor} sl: {NodeStaticBLevelMappings[maxDlPair.Item1.Node]} - {string.Join(',', maxDlPair.Item3)} DL: {maxDl}");
                foreach (var childnode in maxDlPair.Item1.Node.SuccessorNodes)
                {
                    if (!processedNodes.Values.SelectMany(x => x).Any(x => x.Node.Id == childnode.Id) && !readyNodePool.Any(x => x.Node.Id == childnode.Id))
                    {
                        readyNodePool.Add(new UnscheduledNode(childnode, NodeBLevelMappings[childnode]));
                    }
                }
            }

            return processedNodes;
        }
    }
}
