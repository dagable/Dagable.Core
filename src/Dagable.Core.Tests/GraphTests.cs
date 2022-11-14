using Dagable.Core.Exceptions;
using Dagable.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Dagable.Core.Tests
{
    [TestClass]
    public class GraphTests
    {
        private StandardNode firstTestNode;
        private StandardNode secondTestNode;
        private Graph<CriticalPathNode, CriticalPathEdge> testCriticalPathGraph;
        private Graph<StandardNode, StandardEdge<StandardNode>> testStandardGraph;
        private const int EDGE_COMM_TIME = 9;


        [TestInitialize]
        public void Setup()
        {
            firstTestNode = new StandardNode(0);
            secondTestNode = new StandardNode(1);
            testStandardGraph = new Graph<StandardNode, StandardEdge<StandardNode>>();
            testCriticalPathGraph = new Graph<CriticalPathNode, CriticalPathEdge>();
        }

        [TestMethod]
        public void Add_EdgeToGraphCriticalPath_ShouldBeASuccess()
        {
            var graph = new Graph<CriticalPathNode, CriticalPathEdge>();
            var newNodeOne = new CriticalPathNode();
            var newNodeTwo = new CriticalPathNode();
            graph.AddEdge(new CriticalPathEdge(newNodeOne, newNodeTwo, EDGE_COMM_TIME));
            Assert.IsTrue(newNodeOne.SuccessorNodes.Count == 1);
            Assert.IsTrue(!newNodeOne.PredecessorNodes.Any());
            Assert.IsTrue(newNodeTwo.PredecessorNodes.Count == 1);
            Assert.IsTrue(!newNodeTwo.SuccessorNodes.Any());
            Assert.IsTrue(graph.Edges.First().CommTime == EDGE_COMM_TIME);
        }

        [TestMethod]
        public void Add_DuplicateEdgeToGraph_ShouldNotAddDuplicate()
        {
            testStandardGraph.AddEdge(new StandardEdge<StandardNode>(firstTestNode, secondTestNode));
            testStandardGraph.AddEdge(new StandardEdge<StandardNode>(firstTestNode, secondTestNode));
            Assert.AreEqual(1, firstTestNode.SuccessorNodes.Count);
            Assert.AreEqual(1, testStandardGraph.Edges.Count);
        }

        [TestMethod]
        public void Add_NodeToGraph_ShouldBeSuccessful()
        {
            Assert.IsTrue(testStandardGraph.AddNode(firstTestNode));
            Assert.AreEqual(1, testStandardGraph.Nodes.Count);
        }

        [TestMethod]
        public void Add_DuplicateNodeToGraph_ShouldBeSuccessful()
        {
            Assert.IsTrue(testStandardGraph.AddNode(firstTestNode));
            Assert.IsFalse(testStandardGraph.AddNode(firstTestNode));
            Assert.AreEqual(1, testStandardGraph.Nodes.Count);
        }

        [TestMethod]
        public void Given_AnEmptyCriticalTaskGraph_When_NodesAreAdded_OnlyDistinctNodesAreAdded()
        {
            var firstNode = new CriticalPathNode(1, 400);
            var secondNode = new CriticalPathNode(1, 400);
            var thirdNode = new CriticalPathNode(1, 500);
            var fourthNode = new CriticalPathNode(1, 500, 400);
            var fifthNode = new CriticalPathNode(2, 500, 400);
            // first node is added
            Assert.IsTrue(testCriticalPathGraph.AddNode(firstNode));
            Assert.AreEqual(1, testCriticalPathGraph.Nodes.Count);
            //2nd node not added same id
            Assert.IsFalse(testCriticalPathGraph.AddNode(secondNode));
            Assert.AreEqual(1, testCriticalPathGraph.Nodes.Count);
            //3rd node not added same id
            Assert.IsFalse(testCriticalPathGraph.AddNode(thirdNode));
            Assert.AreEqual(1, testCriticalPathGraph.Nodes.Count);
            //4th node not added same id
            Assert.IsFalse(testCriticalPathGraph.AddNode(fourthNode));
            Assert.AreEqual(1, testCriticalPathGraph.Nodes.Count);
            //5th node added different id
            Assert.IsTrue(testCriticalPathGraph.AddNode(fifthNode));
            Assert.AreEqual(2, testCriticalPathGraph.Nodes.Count);
        }

        // this is failing
        [TestMethod]
        public void Given_AnEmptyCriticalTaskGraph_When_EdgesAreAdded_OnlyDistinctEdgesAreAdded()
        {
            var firstNode = new CriticalPathNode(1, 400);
            var fourthNode = new CriticalPathNode(1, 500, 400);
            var fifthNode = new CriticalPathNode(2, 500, 400);
            var addValidEdge = testCriticalPathGraph.AddEdge(new CriticalPathEdge(firstNode, fifthNode, EDGE_COMM_TIME));
            Assert.IsTrue(addValidEdge);
            var addDuplicateEdge = testCriticalPathGraph.AddEdge(new CriticalPathEdge(firstNode, fifthNode, EDGE_COMM_TIME));
            var nodeCountPrior = testCriticalPathGraph.Nodes.Count;
            Assert.IsFalse(addDuplicateEdge);
            Assert.IsFalse(testCriticalPathGraph.AddEdge(new CriticalPathEdge(fourthNode, fifthNode, EDGE_COMM_TIME)));
            Assert.AreEqual(nodeCountPrior, testCriticalPathGraph.Nodes.Count);
        }
    }
}
