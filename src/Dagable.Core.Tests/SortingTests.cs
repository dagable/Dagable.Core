using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dagable.Core.Tests
{
    [TestClass]
    public class SortingTests
    {
        private Node NodeOne = new Node(1);
        private Node NodeTwo = new Node(2);
        private Node NodeThree = new Node(3);
        private Node NodeFour = new Node(4);

        [TestMethod]
        public void Graph_HasNoLoops_ReturnsValidTopology()
        {
            Graph<Node, Edge<Node>> graph = new Graph<Node, Edge<Node>>();
            graph.AddEdge(new Edge<Node>(NodeOne, NodeTwo));
            graph.AddEdge(new Edge<Node>(NodeOne, NodeThree));
            graph.AddEdge(new Edge<Node>(NodeThree, NodeFour));
            
            var result = Sorting.KhansTopologySort(graph.Nodes, graph.Edges);
            Assert.IsTrue(result.Any());

        }

        [TestMethod]
        public void Graph_HasNoLoops_ReturnsNull()
        {
            Graph<Node, Edge<Node>> graph = new Graph<Node, Edge<Node>>();
            graph.AddEdge(new Edge<Node>(NodeOne, NodeTwo));
            graph.AddEdge(new Edge<Node>(NodeOne, NodeThree));
            graph.AddEdge(new Edge<Node>(NodeThree, NodeOne)); //add loop here

            var result = Sorting.KhansTopologySort(graph.Nodes, graph.Edges);
            Assert.AreEqual(result, null);
        }
    }
}
