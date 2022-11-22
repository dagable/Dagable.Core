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
        private readonly StandardNode NodeOne = new(1);
        private readonly StandardNode NodeTwo = new(2);
        private readonly StandardNode NodeThree = new(3);
        private readonly StandardNode NodeFour = new(4);

        [TestMethod]
        public void Graph_HasNoLoops_ReturnsValidTopology()
        {
            Graph<StandardNode, StandardEdge<StandardNode>> graph = new();
            graph.AddEdge(new StandardEdge<StandardNode>(NodeOne, NodeTwo));
            graph.AddEdge(new StandardEdge<StandardNode>(NodeOne, NodeThree));
            graph.AddEdge(new StandardEdge<StandardNode>(NodeThree, NodeFour));
            
            var result = Sorting.KhansTopologySort(graph.Nodes, graph.Edges);
            Assert.IsTrue(result.Any());

        }

        [TestMethod]
        public void Graph_HasNoLoops_ReturnsNull()
        {
            Graph<StandardNode, StandardEdge<StandardNode>> graph = new();
            graph.AddEdge(new StandardEdge<StandardNode>(NodeOne, NodeTwo));
            graph.AddEdge(new StandardEdge<StandardNode>(NodeOne, NodeThree));
            graph.AddEdge(new StandardEdge<StandardNode>(NodeThree, NodeOne)); //add loop here

            var result = Sorting.KhansTopologySort(graph.Nodes, graph.Edges);
            Assert.AreEqual(result, null);
        }
    }
}
