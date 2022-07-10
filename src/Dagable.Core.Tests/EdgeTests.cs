﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dagable.Core.Tests
{
    [TestClass]
    public class EdgeTests
    {
        [TestMethod]
        public void Equality_TwoEdges_ReturnEqual()
        {
            var firstNode = new Node(0);
            var secondNode = new Node(1);
            var firstEdge = new Edge<Node>(firstNode, secondNode);
            var secondEdge = new Edge<Node>(firstNode, secondNode);

            Assert.IsTrue(firstEdge.Equals(secondEdge));
        }

        [TestMethod]
        public void Equality_TwoEdges_ReturnNotEqual()
        {
            var firstNode = new Node(0);
            var secondNode = new Node(1);
            var thirdNode = new Node(2);
            var firstEdge = new Edge<Node>(firstNode, secondNode);
            var secondEdge = new Edge<Node>(firstNode, thirdNode);

            Assert.IsFalse(firstEdge.Equals(secondEdge));
        }

        [TestMethod]
        public void HashCode_TwoEdges_ReturnEqual()
        {
            var firstNode = new Node(0);
            var secondNode = new Node(1);
            var firstEdge = new Edge<Node>(firstNode, secondNode);
            var secondEdge = new Edge<Node>(firstNode, secondNode);

            Assert.AreEqual(firstEdge.GetHashCode(), secondEdge.GetHashCode());
        }

        [TestMethod]
        public void HashCode_TwoEdges_ReturnNotEqual()
        {
            var firstNode = new Node(0);
            var secondNode = new Node(1);
            var thirdNode = new Node(2);
            var firstEdge = new Edge<Node>(firstNode, secondNode);
            var secondEdge = new Edge<Node>(firstNode, thirdNode);

            Assert.AreNotEqual(firstEdge.GetHashCode(), secondEdge.GetHashCode());
        }

        [TestMethod]
        public void Add_NewEdge_ShouldBeCorrectPredecessorAndSuccessorNodes()
        {
            var firstNode = new Node(0);
            var secondNode = new Node(1);
            _ = new Edge<Node>(firstNode, secondNode);

            Assert.IsTrue(firstNode.SuccessorNodes.Contains(secondNode));
            Assert.IsTrue(secondNode.PredecessorNodes.Contains(firstNode));
            Assert.AreEqual(firstNode.PredecessorNodes.Count, 0);
            Assert.AreEqual(secondNode.SuccessorNodes.Count, 0);
        }
    }
}
