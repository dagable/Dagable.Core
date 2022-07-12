using Dagable.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dagable.Core.Tests
{
    [TestClass]
    public class GraphTests
    {
        private Node firstTestNode;
        private Node secondTestNode;
        private Graph<Node, Edge<Node>> testGraph;

        [TestInitialize]
        public void Setup()
        {
            firstTestNode = new Node(0);
            secondTestNode = new Node(1);
            testGraph = new Graph<Node, Edge<Node>>();
        }

        [TestMethod]
        public void Add_EdgeToGraphCriticalPath_ShouldBeASuccess()
        {
            var graph = new Graph<CPathNode, CPathEdge>();
            var newNodeOne = new CPathNode();
            var newNodeTwo = new CPathNode();
            graph.AddEdge(new CPathEdge(newNodeOne, newNodeTwo, 9));
            Assert.IsTrue(newNodeOne.SuccessorNodes.Count == 1);
            Assert.IsTrue(newNodeTwo.PredecessorNodes.Count == 1);
        }

        [TestMethod]
        public void Add_EdgeToGraph_ShouldBeASuccess()
        {
            var result = testGraph.AddEdge(new Edge<Node>(firstTestNode, secondTestNode));
            Assert.IsTrue(firstTestNode.SuccessorNodes.Count == 1);
            Assert.IsTrue(secondTestNode.PredecessorNodes.Count == 1);
            Assert.AreEqual(result, true);
            Assert.AreEqual(testGraph.Edges.Count, 1);
        }

        [TestMethod]
        public void Add_DuplicateEdgeToGraph_ShouldNotAddDuplicate()
        {
            testGraph.AddEdge(new Edge<Node>(firstTestNode, secondTestNode));
            testGraph.AddEdge(new Edge<Node>(firstTestNode, secondTestNode));
            Assert.AreEqual(firstTestNode.SuccessorNodes.Count, 1);
        }

        [TestMethod]
        public void Add_nodeToGraph_ShouldBeSuccessful()
        {
            Assert.IsTrue(testGraph.AddNode(firstTestNode));
        }
    }
}
