using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dagable.Core.Tests
{
    [TestClass]
    public class GraphCreatorTests
    {
        private DagCreator creator;


        [TestMethod]
        public void Setup_GraphCreation_LayerCountIsCorrect()
        {
            creator = new DagCreator(10);
            Assert.AreEqual(creator.LayerCount, 10);
        }

        [TestMethod]
        public void Setup_GraphCreations_LayerCountIsCorrect()
        {
            var jsonResult = new DagCreator(10).Generate().AsJson();
            Assert.AreEqual(creator.LayerCount, 10);
        }

        [TestMethod]
        public void Setup_GraphCreation_NodeCountIsCorrect()
        {
            creator = new DagCreator(10, 20);
            Assert.AreEqual(creator.NodeCount, 20);
            Assert.AreEqual(creator.LayerCount, 10);
        }

        [TestMethod]
        public void Setup_GraphCreation_IsValid()
        {
            creator = new DagCreator(10, 30);
            Assert.IsTrue(creator.Generate().TopologySortedGraph() != null);
        }

        [TestMethod]
        public void Setup_GraphCreationSecond_IsValidRandom()
        {
            creator = new DagCreator(10, 10);
            var sorted = creator.Generate().TopologySortedGraph();
            Assert.IsTrue(sorted != null);
        }


        [TestMethod]
        public void Setup_GraphCreated_IsInvalid()
        {
            var graph = new Graph();

            var a = new Node(0);
            var b = new Node(1);
            var c = new Node(2);


            graph.AddEdge(a ,b);
            graph.AddEdge(b, c);
            graph.AddEdge(c, a);

            Assert.IsTrue(Sorting.KhansTopologySort(graph.Nodes, graph.Edges) == null);
        }

        [TestMethod]
        public void Setup_GraphCreated_IsValid()
        {
            var graph = new Graph();

            var a = new Node(0);
            var b = new Node(1);
            var c = new Node(2);


            graph.AddEdge(a, b);
            graph.AddEdge(b, c);
            graph.AddEdge(a, c);

            Assert.IsTrue(Sorting.KhansTopologySort(graph.Nodes, graph.Edges) != null);
        }

        [TestMethod]
        public void Setup_RootNodeConnectedToAllFirstLayer_IsValid()
        {
            var graph = new Graph();
            var a = new Node(0, 0);
            var b = new Node(1, 1);
            var c = new Node(2, 1);

            graph.AddEdges(a, new[] { b, c });

            Assert.IsTrue(a.SuccessorNodes.Count == 2);
            Assert.IsTrue(graph.Edges.Count == 2);
        }
    }
}
