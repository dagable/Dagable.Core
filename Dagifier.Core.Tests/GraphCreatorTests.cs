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
    }
}
