using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dagable.Core.Tests
{
    [TestClass]
    public class GraphCreatorTests
    {
        private DAG.StandardTaskGraph creator;

        [TestMethod]
        public void Setup_GraphCreation_IsValid()
        {
            creator = new DAG.StandardTaskGraph(10, 30);
            Assert.IsTrue(creator.Generate().TopologySortedGraph() != null);
        }

        [TestMethod]
        public void Setup_GraphCreationSecond_IsValidRandom()
        {
            creator = new DAG.StandardTaskGraph(10, 10);
            var sorted = creator.Generate().TopologySortedGraph();
            Assert.IsTrue(sorted != null);
        }


        [TestMethod]
        public void Setup_GraphCreated_IsInvalid()
        {
            var graph = new Graph<Node, Edge<Node>>();

            var a = new Node(0);
            var b = new Node(1);
            var c = new Node(2);

            graph.AddEdge(new Edge<Node>(a ,b));
            graph.AddEdge(new Edge<Node>(b, c));
            graph.AddEdge(new Edge<Node>(c, a));

            Assert.IsTrue(Sorting.KhansTopologySort(graph.Nodes, graph.Edges) == null);
        }

        [TestMethod]
        public void Setup_GraphCreated_IsValid()
        {
            var graph = new Graph<Node, Edge<Node>>();

            var a = new Node(0);
            var b = new Node(1);
            var c = new Node(2);


            graph.AddEdge(new Edge<Node>(a, b));
            graph.AddEdge(new Edge<Node>(b, c));
            graph.AddEdge(new Edge<Node>(a, c));

            Assert.IsTrue(Sorting.KhansTopologySort(graph.Nodes, graph.Edges) != null);
        }

        [TestMethod]
        public void Setup_RootNodeConnectedToAllFirstLayer_IsValid()
        {
            var graph = new Graph<Node, Edge<Node>>();
            var a = new Node(0, 0);
            var b = new Node(1, 1);
            var c = new Node(2, 1);

            graph.AddEdges(a, new[] { b, c });

            Assert.IsTrue(a.SuccessorNodes.Count == 2);
            Assert.IsTrue(graph.Edges.Count == 2);
        }
    }
}
