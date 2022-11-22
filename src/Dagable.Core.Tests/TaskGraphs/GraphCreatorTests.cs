using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dagable.Core.Tests
{
    [TestClass]
    public class GraphCreatorTests
    {
        private TaskGraph.Standard standardTaskGraph;

        [TestMethod]
        public void Setup_GraphCreation_IsValid()
        {
            standardTaskGraph = new TaskGraph.Standard(10, 30);
            Assert.IsTrue(standardTaskGraph.Generate().TopologySortedGraph() != null);
        }

        [TestMethod]
        public void Setup_GraphCreationSecond_IsValidRandom()
        {
            standardTaskGraph = new TaskGraph.Standard(10, 10);
            var sorted = standardTaskGraph.Generate().TopologySortedGraph();
            Assert.IsTrue(sorted != null);
        }


        [TestMethod]
        public void Setup_GraphCreated_IsInvalid()
        {
            var graph = new Graph<StandardNode, StandardEdge<StandardNode>>();

            var a = new StandardNode(0);
            var b = new StandardNode(1);
            var c = new StandardNode(2);

            graph.AddEdge(new StandardEdge<StandardNode>(a ,b));
            graph.AddEdge(new StandardEdge<StandardNode>(b, c));
            graph.AddEdge(new StandardEdge<StandardNode>(c, a));

            Assert.IsTrue(Sorting.KhansTopologySort(graph.Nodes, graph.Edges) == null);
        }

        [TestMethod]
        public void Setup_GraphCreated_IsValid()
        {
            var graph = new Graph<StandardNode, StandardEdge<StandardNode>>();

            var a = new StandardNode(0);
            var b = new StandardNode(1);
            var c = new StandardNode(2);


            graph.AddEdge(new StandardEdge<StandardNode>(a, b));
            graph.AddEdge(new StandardEdge<StandardNode>(b, c));
            graph.AddEdge(new StandardEdge<StandardNode>(a, c));

            Assert.IsTrue(Sorting.KhansTopologySort(graph.Nodes, graph.Edges) != null);
        }

        [TestMethod]
        public void Setup_RootNodeConnectedToAllFirstLayer_IsValid()
        {
            var graph = new Graph<StandardNode, StandardEdge<StandardNode>>();
            var a = new StandardNode(0, 0);
            var b = new StandardNode(1, 1);
            var c = new StandardNode(2, 1);

            graph.AddEdges(a, new[] { b, c });

            Assert.IsTrue(a.SuccessorNodes.Count == 2);
            Assert.IsTrue(graph.Edges.Count == 2);
        }
    }
}
