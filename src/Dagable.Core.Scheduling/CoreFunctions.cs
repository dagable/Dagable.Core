using Dagable.Core.Models;
using System.Collections.Generic;
using System.Linq;
using static Dagable.Core.TaskGraph;

namespace Dagable.Core.Scheduling
{
    public static class CoreFunctions
    {
       public static Dictionary<CriticalPathNode, int> ComputerTLevel(List<CriticalPathNode> toplogySortedNodes, HashSet<CriticalPathEdge> edges)
       {
            var results = toplogySortedNodes.ToDictionary(x => x, x => default(int));
            foreach(var node in toplogySortedNodes)
            {
                var max = 0;
                foreach(var parentNode in node.PredecessorNodes)
                {
                    var tLevel = results[parentNode] + parentNode.ComputationTime + edges.First(x => x.NextNode == node && x.PrevNode == parentNode).CommTime;
                    if (tLevel > max){
                        max = tLevel;
                    }
                }
                results[node] = max;
            }
            return results;
       }


        public static Dictionary<CriticalPathNode, int> ComputerBLevel(List<CriticalPathNode> topologySortedNodes, HashSet<CriticalPathEdge> edges)
        {
            topologySortedNodes.Reverse();
            var results = topologySortedNodes.ToDictionary(x => x, x => default(int));
            foreach(var node in topologySortedNodes)
            {
                var max = 0;
                foreach(var childNode in node.SuccessorNodes)
                {
                    var bLevel = results[childNode] + edges.First(x => x.NextNode == childNode && x.PrevNode == node).CommTime;
                    if(bLevel > max)
                    {
                        max = bLevel;
                    }
                }
                results[node] = node.ComputationTime + max;
            }
            return results;
        }

        public static Dictionary<CriticalPathNode, int> ComputerStaticBLevel(List<CriticalPathNode> topologySortedNodes)
        {
            topologySortedNodes.Reverse();
            var results = topologySortedNodes.ToDictionary(x => x, x => default(int));
            foreach (var node in topologySortedNodes)
            {
                var max = 0;
                foreach (var childNode in node.SuccessorNodes)
                {
                    var bLevel = results[childNode];
                    if (bLevel > max)
                    {
                        max = bLevel;
                    }
                }
                results[node] = max + node.ComputationTime;
            }
            return results;
        }

        public static Dictionary<CriticalPathNode, int> ComputeALAP(CriticalPath graph)
        {
            var topologyOrdered = Sorting.KhansTopologySort(graph.dagGraph.Nodes, new HashSet<CriticalPathEdge>(graph.dagGraph.Edges));
            topologyOrdered.Reverse();
            var results = topologyOrdered.ToDictionary(x => x, x => default(int));
            var CPathLength = graph.DetermineCriticalPathLength();

            foreach(var node in topologyOrdered)
            {
                var minFinishTime = CPathLength;
                foreach(var child in node.SuccessorNodes)
                {
                    var childAlap = results[child] - graph.dagGraph.Edges.First(x => x.NextNode == child && x.PrevNode == node).CommTime;
                    if(childAlap < minFinishTime)
                    {
                        minFinishTime = childAlap;
                    }
                }
                results[node] = minFinishTime - node.ComputationTime;
            }

            return results;
        }
    }
}
