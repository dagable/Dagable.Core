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
        private readonly CPathNode NodeOne = new CPathNode(0, 0, 2);
        private readonly CPathNode NodeTwo = new CPathNode(1,1, 3);
        private readonly CPathNode NodeThree = new CPathNode(2,1, 3);
        private readonly CPathNode NodeFour = new CPathNode(3,1, 4);
        private readonly CPathNode NodeFive = new CPathNode(4,1, 5);
        private readonly CPathNode NodeSix = new CPathNode(5,2, 4);
        private readonly CPathNode NodeSeven = new CPathNode(6,2, 4);
        private readonly CPathNode NodeEight = new CPathNode(7,2, 4);
        private readonly CPathNode NodeNine = new CPathNode(8, 3, 1);

        private List<CPathNode> TopologySortedNodes;
        private HashSet<CPathEdge> graphEdges;
        private DAG.CriticalPathTaskGraph creator;

        [TestInitialize]
        public void Setup()
        {
            creator = new DAG.CriticalPathTaskGraph(3);
            creator.dagGraph.AddNode(NodeOne);
            creator.dagGraph.AddNode(NodeTwo);
            creator.dagGraph.AddNode(NodeThree);
            creator.dagGraph.AddNode(NodeFour);
            creator.dagGraph.AddNode(NodeFive);
            creator.dagGraph.AddNode(NodeSix);
            creator.dagGraph.AddNode(NodeSeven);
            creator.dagGraph.AddNode(NodeEight);
            creator.dagGraph.AddNode(NodeNine);
            creator.dagGraph.AddEdge(new CPathEdge(NodeOne, NodeTwo, 4));
            creator.dagGraph.AddEdge(new CPathEdge(NodeOne, NodeSeven, 10));
            creator.dagGraph.AddEdge(new CPathEdge(NodeOne, NodeThree, 1));
            creator.dagGraph.AddEdge(new CPathEdge(NodeOne, NodeFour, 1));
            creator.dagGraph.AddEdge(new CPathEdge(NodeOne, NodeFive, 1));
            creator.dagGraph.AddEdge(new CPathEdge(NodeTwo, NodeSix, 1));
            creator.dagGraph.AddEdge(new CPathEdge(NodeTwo, NodeSeven, 1));
            creator.dagGraph.AddEdge(new CPathEdge(NodeThree, NodeEight, 1));
            creator.dagGraph.AddEdge(new CPathEdge(NodeFour, NodeEight, 1));
            creator.dagGraph.AddEdge(new CPathEdge(NodeSix, NodeNine, 5));
            creator.dagGraph.AddEdge(new CPathEdge(NodeSeven, NodeNine, 6));
            creator.dagGraph.AddEdge(new CPathEdge(NodeEight, NodeNine, 5));

            TopologySortedNodes =  Sorting.KhansTopologySort(creator.dagGraph.Nodes, new HashSet<CPathEdge>(creator.dagGraph.Edges));
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
            var results = CoreFunctions.ComputerStaticBLevel(TopologySortedNodes, graphEdges);
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
            var scheduler = new DSLScheduler(3, creator);

            var results = scheduler.Schedule();
#if DEBUG
            foreach (var res in results)
            {
                Debug.WriteLine($"processor: {res.Key + 1}");
                foreach (var item in res.Value)
                {
                    Debug.WriteLine($"    item: {item.Node.Id + 1}, start: {item.StartAt} end: {item.EndAt}");
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