using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dagifier.Core.Tests
{
    [TestClass]
    public class GraphTests
    {
        [TestMethod]
        public void Add_EdgeToGraph_ShouldBeASuccess()
        {
            var graph = new Graph();
            var firstNode = new Node(0);
            var secondNode = new Node(1);

            var result = graph.AddEdge(firstNode, secondNode);

            Assert.AreEqual(result, true);
            Assert.AreEqual(graph.Edges.Count, 1);
        }

        [TestMethod]
        public void Add_DuplicateEdgeToGraph_ShouldNotAddDuplicate()
        {
            var graph = new Graph();
            var firstNode = new Node(0);
            var secondNode = new Node(1);

            graph.AddEdge(firstNode, secondNode);
            graph.AddEdge(firstNode, secondNode);

            Assert.AreEqual(graph.Edges.Count, 1);
        }
    }
}
