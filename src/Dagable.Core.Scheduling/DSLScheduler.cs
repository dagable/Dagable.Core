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
            NodeTLevelMappings = CoreFunctions.ComputerTLevel(TopologySortedNodes, graph.dagGraph.Edges);
            NodeBLevelMappings = CoreFunctions.ComputerStaticBLevel(TopologySortedNodes, graph.dagGraph.Edges);
            for (int i = 0; i < _processorCount; i++)
            {
                processorMapping.Add(i, new List<ScheduledNode>());
            }
        }

        public Dictionary<int, List<ScheduledNode>> Schedule()
        {
            var readyNodePool = new HashSet<UnscheduledNode> { new UnscheduledNode(_graph.dagGraph.Nodes.First(x => x.Layer == 0), NodeBLevelMappings[_graph.dagGraph.Nodes.First(x => x.Layer == 0)]) };
            var processedNodes = new Dictionary<int, List<ScheduledNode>>();
            var processorNodeMappings = new Dictionary<UnscheduledNode, Tuple<int, int>>();
            for (int i = 0; i < _processorCount; i++)
            {
                processedNodes[i] = new List<ScheduledNode>();
            }

            while (readyNodePool.Any())
            {
                processorNodeMappings = new Dictionary<UnscheduledNode, Tuple<int, int>>();

                var node = readyNodePool.OrderByDescending(x => x.StaticBLevel).ThenByDescending(x => NodeBLevelMappings[x.Node] - NodeTLevelMappings[x.Node]).First();
                var processorDL = new int[_processorCount];
                Array.Fill(processorDL, int.MaxValue);
                var dl = NodeBLevelMappings[node.Node] - NodeTLevelMappings[node.Node];
                for (int i = 0; i < processorDL.Length; i++)
                {
                    processorDL[i] = processedNodes[i].Any() ? processedNodes[i].Max(x => x.EndAt) : (NodeBLevelMappings[node.Node] - dl);
                    if (i != 0 && processorDL[i] >= processorDL[i - 1])
                    {
                        break;
                    }
                }
                processorNodeMappings.Add(node, Tuple.Create(processorDL.ToList().IndexOf(processorDL.Min()), processorDL.Min()));

                var minDl = processorNodeMappings.Min(x => x.Value.Item2);
                var maxPairing = processorNodeMappings.First(x => x.Value.Item2 == minDl);
                var startTime = (processedNodes[maxPairing.Value.Item1].Any() ? processedNodes[maxPairing.Value.Item1].Max(x => x.EndAt) : NodeTLevelMappings[maxPairing.Key.Node]);
                processedNodes[maxPairing.Value.Item1].Add(new ScheduledNode(maxPairing.Key.Node, startTime, startTime + maxPairing.Key.Node.ComputationTime));
                readyNodePool.Remove(maxPairing.Key);
                foreach (var childnode in maxPairing.Key.Node.SuccessorNodes)
                {
                    if (!processedNodes.Values.SelectMany(x => x).Any(x => x.Node.Id == childnode.Id) && !readyNodePool.Any(x => x.Node.Id == childnode.Id))
                    {
                        readyNodePool.Add(new UnscheduledNode(childnode, NodeBLevelMappings[childnode]));
                    }
                }
            }
            var a = processedNodes[0].Select(x => x.Node.Id);
            var b = processedNodes[1].Select(x => x.Node.Id);
            var c = processedNodes[2].Select(x => x.Node.Id);
            return processedNodes;
        }
    }
}
