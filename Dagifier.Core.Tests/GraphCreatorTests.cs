using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dagifier.Core.Tests
{
    [TestClass]
    public class GraphCreatorTests
    {
        private GraphCreator creator;

        [TestInitialize]
        public void Setup()
        {
            creator = new GraphCreator();
        }

        [TestMethod]
        public void Setup_GraphCreation_LayerCountIsCorrect()
        {
            creator.Setup(10);
            Assert.AreEqual(creator.LayerCount, 10);
        }

        [TestMethod]
        public void Setup_GraphCreation_NodeCountIsCorrect()
        {
            creator.Setup(10, 20);
            Assert.AreEqual(creator.NodeCount, 20);
            Assert.AreEqual(creator.LayerCount, 10);
        }

        [TestMethod]
        public void Setup_GraphCreation_IsValid()
        {
            var graph = creator.Setup(10, 30).Execute();

            Assert.IsTrue(graph.KhansTopologySort() != null);
        }

        [TestMethod]
        public void Setup_GraphCreationSecond_IsValidRandom()
        {
            var graph = creator.Setup(10, 10).Execute();

            var order = graph.KhansTopologySort();

            Assert.IsTrue(order != null);
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

            Assert.IsTrue(graph.KhansTopologySort() == null);
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

            Assert.IsTrue(graph.KhansTopologySort() != null);
        }
    }
}
