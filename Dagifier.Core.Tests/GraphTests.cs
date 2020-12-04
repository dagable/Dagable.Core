using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Dagifier.Core.Tests
{
    [TestClass]
    public class GraphTests
    {
        private Node firstTestNode;
        private Node secondTestNode;
        private Graph testGraph;

        [TestInitialize]
        public void Setup()
        {
            firstTestNode = new Node(0);
            secondTestNode = new Node(1);
            testGraph = new Graph();
        }

        [TestMethod]
        public void Add_EdgeToGraph_ShouldBeASuccess()
        {
            var result = testGraph.AddEdge(firstTestNode, secondTestNode);

            Assert.AreEqual(result, true);
            Assert.AreEqual(testGraph.Edges.Count, 1);
        }

        [TestMethod]
        public void Add_DuplicateEdgeToGraph_ShouldNotAddDuplicate()
        {
            testGraph.AddEdge(firstTestNode, secondTestNode);
            testGraph.AddEdge(firstTestNode, secondTestNode);

            Assert.AreEqual(testGraph.Edges.Count, 1);
        }

        [TestMethod]
        public void Add_nodeToGraph_ShouldBeSuccessful()
        {
            Assert.IsTrue(testGraph.AddNode(firstTestNode));
        }
    }
}
