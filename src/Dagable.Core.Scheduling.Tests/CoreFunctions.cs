using Dagable.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dagable.Core.Scheduling.Tests
{
    [TestClass]
    public class Tests
    {
        private readonly CriticalPathNode NodeOne = new CriticalPathNode(0, 0, 2);
        private readonly CriticalPathNode NodeTwo = new CriticalPathNode(1,1, 3);
        private readonly CriticalPathNode NodeThree = new CriticalPathNode(2,1, 3);
        private readonly CriticalPathNode NodeFour = new CriticalPathNode(3,1, 4);
        private readonly CriticalPathNode NodeFive = new CriticalPathNode(4,1, 5);
        private readonly CriticalPathNode NodeSix = new CriticalPathNode(5,2, 4);
        private readonly CriticalPathNode NodeSeven = new CriticalPathNode(6,2, 4);
        private readonly CriticalPathNode NodeEight = new CriticalPathNode(7,2, 4);
        private readonly CriticalPathNode NodeNine = new CriticalPathNode(8, 3, 1);

        private List<CriticalPathNode> TopologySortedNodes;
        private HashSet<CriticalPathEdge> graphEdges;
        private TaskGraph.CriticalPath creator;

        [TestInitialize]
        public void Setup()
        {
            creator = new TaskGraph.CriticalPath(3)
            {
                dagGraph = new Graph<CriticalPathNode, CriticalPathEdge>()
            };
            creator.dagGraph.AddNode(NodeOne);
            creator.dagGraph.AddNode(NodeTwo);
            creator.dagGraph.AddNode(NodeThree);
            creator.dagGraph.AddNode(NodeFour);
            creator.dagGraph.AddNode(NodeFive);
            creator.dagGraph.AddNode(NodeSix);
            creator.dagGraph.AddNode(NodeSeven);
            creator.dagGraph.AddNode(NodeEight);
            creator.dagGraph.AddNode(NodeNine);
            creator.dagGraph.AddEdge(new CriticalPathEdge(NodeOne, NodeTwo, 4));
            creator.dagGraph.AddEdge(new CriticalPathEdge(NodeOne, NodeSeven, 10));
            creator.dagGraph.AddEdge(new CriticalPathEdge(NodeOne, NodeThree, 1));
            creator.dagGraph.AddEdge(new CriticalPathEdge(NodeOne, NodeFour, 1));
            creator.dagGraph.AddEdge(new CriticalPathEdge(NodeOne, NodeFive, 1));
            creator.dagGraph.AddEdge(new CriticalPathEdge(NodeTwo, NodeSix, 1));
            creator.dagGraph.AddEdge(new CriticalPathEdge(NodeTwo, NodeSeven, 1));
            creator.dagGraph.AddEdge(new CriticalPathEdge(NodeThree, NodeEight, 1));
            creator.dagGraph.AddEdge(new CriticalPathEdge(NodeFour, NodeEight, 1));
            creator.dagGraph.AddEdge(new CriticalPathEdge(NodeSix, NodeNine, 5));
            creator.dagGraph.AddEdge(new CriticalPathEdge(NodeSeven, NodeNine, 6));
            creator.dagGraph.AddEdge(new CriticalPathEdge(NodeEight, NodeNine, 5));

            TopologySortedNodes =  Sorting.KhansTopologySort(creator.dagGraph.Nodes, new HashSet<CriticalPathEdge>(creator.dagGraph.Edges));
            graphEdges = creator.dagGraph.Edges;
        }

        [TestMethod]
        public void TLevelValuesAreCorrect()
        {
            var results = CoreFunctions.ComputerTLevel(TopologySortedNodes, graphEdges);
        #if DEBUG
            foreach (var res in results.OrderBy(x => x.Key.Id))
            {
                Debug.WriteLine($"Node: {res.Key.Id}, tlevel: {res.Value}");
            }
        #endif
            var expected = new List<int> { 0, 6, 3, 3, 3, 10, 12, 8, 22 };
            var actual = results.OrderBy(x => x.Key.Id).Select(x => x.Value).ToList();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StaticBLevelValuesAreCorrect()
        {
            var results = CoreFunctions.ComputerStaticBLevel(TopologySortedNodes);
            #if DEBUG
            foreach (var res in results.OrderBy(x => x.Key.Id))
            {
                Debug.WriteLine($"Node: {res.Key.Id}, blevel: {res.Value}");
            }
            #endif
            var expected = new List<int> {11,8,8,9,5,5,5,5,1 };
            var actual = results.OrderBy(x => x.Key.Id).Select(x => x.Value).ToList();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BLevelValuesAreCorrect()
        {
            var results = CoreFunctions.ComputerBLevel(TopologySortedNodes, graphEdges);
            #if DEBUG
            foreach (var res in results.OrderBy(x => x.Key.Id))
            {
                Debug.WriteLine($"Node: {res.Key.Id}, blevel: {res.Value}");
            }
            #endif
            var expected = new List<int> { 23, 15, 14, 15, 5, 10, 11, 10, 1 };
            var actual = results.OrderBy(x => x.Key.Id).Select(x => x.Value).ToList();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DLSSchedule()
        {
            var scheduler = new DLScheduler(3, creator);

            var results = scheduler.Schedule();

            Assert.IsNotNull(results);
#if DEBUG
            foreach (var res in results)
            {
                Debug.WriteLine($"processor: {res.Key + 1}");
                foreach (var item in res.Value)
                {
                    Debug.WriteLine($"    item: {item.Id + 1}, start: {item.StartAt} end: {item.EndAt}");
                }
            }

#endif
        }


        [TestMethod]
        public void ComputeALapIsCorrect()
        {
            var results = CoreFunctions.ComputeALAP(creator);
#if DEBUG
            foreach (var res in results.OrderBy(x => x.Key.Id))
            {
                Debug.WriteLine($"Node: {res.Key.Id}, ALAP: {res.Value}");
            }
#endif
            var expected = new List<int> {0, 8, 9, 8, 18, 13, 12,13, 22  };
            var actual = results.OrderBy(x => x.Key.Id).Select(x => x.Value).ToList();
            CollectionAssert.AreEqual(expected, actual);
        }

    }
}