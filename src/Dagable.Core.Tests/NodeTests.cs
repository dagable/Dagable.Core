using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dagable.Core.Tests
{
    [TestClass]
    public class NodeTests
    {
        private Node firstTestNode, secondTestNode;

        [TestInitialize]
        public void Setup()
        {
            firstTestNode = new Node(0);
            secondTestNode = new Node(1);
        }

        [TestMethod]
        public void Equality_TwoNodes_ReturnEqual()
        {
            Assert.IsTrue(firstTestNode.Equals(firstTestNode));
        }

        [TestMethod]
        public void Equality_TwoNodes_ReturnsNotEqual()
        {
            Assert.IsFalse(firstTestNode.Equals(secondTestNode));
        }

        [TestMethod]
        public void HashCodes_TwoNodes_ReturnEqual()
        {
            Assert.AreEqual(firstTestNode.GetHashCode(), firstTestNode.GetHashCode());
        }

        [TestMethod]
        public void HashCodes_TwoNodes_ReturnNotEqual()
        {
            Assert.AreNotEqual(firstTestNode.GetHashCode(), secondTestNode.GetHashCode());
        }

        [TestMethod]
        public void Add_NewSuccessor_ReturnsCorrectCount()
        {
            firstTestNode.AddSuccessor(secondTestNode);
            Assert.AreEqual(firstTestNode.SuccessorNodes.Count, 1);
        }

        [TestMethod]
        public void Add_NewPredesessor_ReturnsCorrectCount()
        {
            firstTestNode.AddPredecessor(secondTestNode);
            Assert.AreEqual(firstTestNode.PredecessorNodes.Count, 1);
        }
    }
}
