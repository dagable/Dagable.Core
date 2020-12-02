using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dagifier.Core.Tests
{
    [TestClass]
    public class NodeTests
    {
        [TestMethod]
        public void Equality_TwoNodes_ReturnEqual()
        {
            var firstnode = new Node(2);
            var secondNode = new Node(2);

            Assert.IsTrue(firstnode.Equals(secondNode));
        }

        [TestMethod]
        public void Equality_TwoNodes_ReturnsNotEqual()
        {
            var firstnode = new Node(2);
            var secondNode = new Node(3);

            Assert.IsFalse(firstnode.Equals(secondNode));
        }

        [TestMethod]
        public void HashCodes_TwoNodes_ReturnEqual()
        {
            var firstNode = new Node(2);
            var secondNode = new Node(2);

            Assert.AreEqual(firstNode.GetHashCode(), secondNode.GetHashCode());
        }

        [TestMethod]
        public void HashCodes_TwoNodes_ReturnNotEqual()
        {
            var firstNode = new Node(2);
            var secondNode = new Node(3);

            Assert.AreNotEqual(firstNode.GetHashCode(), secondNode.GetHashCode());
        }
    }
}
