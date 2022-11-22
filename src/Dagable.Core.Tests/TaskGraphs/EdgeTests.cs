using Dagable.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dagable.Core.Tests
{
    [TestClass]
    public class EdgeTests
    {
        [TestMethod]
        public void Equality_Standard_TwoEdges_ReturnEqual()
        {
            var firstNode = new StandardNode(0);
            var secondNode = new StandardNode(1);
            var firstEdge = new StandardEdge<StandardNode>(firstNode, secondNode);
            var secondEdge = new StandardEdge<StandardNode>(firstNode, secondNode);

            Assert.IsTrue(firstEdge.Equals(secondEdge));
        }

        [TestMethod]
        public void Equality_CriticalPath_TwoEdges_ReturnEqual()
        {
            var firstNode = new CriticalPathNode(0, 333);
            var secondNode = new CriticalPathNode(1, 234);
            var firstEdge = new CriticalPathEdge(firstNode, secondNode, 342);
            var secondEdge = new CriticalPathEdge(firstNode, secondNode, 324);

            Assert.IsTrue(firstEdge.Equals(secondEdge));
        }

        [TestMethod]
        public void Equality_Standard_TwoEdges_ReturnNotEqual()
        {
            var firstNode = new StandardNode(0);
            var secondNode = new StandardNode(1);
            var thirdNode = new StandardNode(2);
            var firstEdge = new StandardEdge<StandardNode>(firstNode, secondNode);
            var secondEdge = new StandardEdge<StandardNode>(firstNode, thirdNode);

            Assert.IsFalse(firstEdge.Equals(secondEdge));
        }

        [TestMethod]
        public void HashCode_Standard_TwoEdges_ReturnEqual()
        {
            var firstNode = new StandardNode(0);
            var secondNode = new StandardNode(1);
            var firstEdge = new StandardEdge<StandardNode>(firstNode, secondNode);
            var secondEdge = new StandardEdge<StandardNode>(firstNode, secondNode);

            Assert.AreEqual(firstEdge.GetHashCode(), secondEdge.GetHashCode());
        }

        [TestMethod]
        public void HashCode_Standard_TwoEdges_ReturnNotEqual()
        {
            var firstNode = new StandardNode(0);
            var secondNode = new StandardNode(1);
            var thirdNode = new StandardNode(2);
            var firstEdge = new StandardEdge<StandardNode>(firstNode, secondNode);
            var secondEdge = new StandardEdge<StandardNode>(firstNode, thirdNode);

            Assert.AreNotEqual(firstEdge.GetHashCode(), secondEdge.GetHashCode());
        }
    }
}
