using Dagable.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Dagable.Core.Tests
{
    [TestClass]
    public class DagCreatorCriticalPath
    {

        private readonly CriticalPathNode NodeOne = new(0);
        private readonly CriticalPathNode NodeTwo = new(1);
        private readonly CriticalPathNode NodeThree = new(2);
        private readonly CriticalPathNode NodeFour = new(3);
        private readonly CriticalPathNode NodeFive = new(4);
        private readonly CriticalPathNode NodeSix = new(5);
        private readonly CriticalPathNode NodeSeven = new(6);
        private readonly CriticalPathNode NodeEight = new(7);


        [TestMethod]
        public void CriticalPathIsCorrect()
        {
            TaskGraph.CriticalPath creator = new()
            {
                dagGraph = new Graph<CriticalPathNode, CriticalPathEdge>()
            };
            creator.dagGraph.AddNode(NodeOne);
            creator.dagGraph.AddNode(NodeTwo);
            creator.dagGraph.AddNode(NodeThree);
            creator.dagGraph.AddNode(NodeFour);
            creator.dagGraph.AddNode(NodeFive);
            creator.dagGraph.AddNode(NodeSix);
            creator.dagGraph.AddNode(NodeSeven);
            creator.dagGraph.AddNode(NodeEight);
            creator.dagGraph.AddEdge(new CriticalPathEdge(NodeOne, NodeSeven, 2));
            creator.dagGraph.AddEdge(new CriticalPathEdge(NodeTwo, NodeThree, -4));
            creator.dagGraph.AddEdge(new CriticalPathEdge(NodeTwo, NodeFive, 1));
            creator.dagGraph.AddEdge(new CriticalPathEdge(NodeTwo, NodeSeven, 8));
            creator.dagGraph.AddEdge(new CriticalPathEdge(NodeFour, NodeOne, 3));
            creator.dagGraph.AddEdge(new CriticalPathEdge(NodeFour, NodeFive, 5));
            creator.dagGraph.AddEdge(new CriticalPathEdge(NodeSix, NodeTwo, 2));
            creator.dagGraph.AddEdge(new CriticalPathEdge(NodeEight, NodeOne, 6));
            creator.dagGraph.AddEdge(new CriticalPathEdge(NodeEight, NodeTwo, -1));
            creator.dagGraph.AddEdge(new CriticalPathEdge(NodeEight, NodeFour, 4));
            creator.dagGraph.AddEdge(new CriticalPathEdge(NodeEight, NodeSix, -4));

            Assert.AreEqual(creator.FindCriticalPath(NodeEight, NodeOne).Sum(x => x.CommTime), 7);
            Assert.AreEqual(creator.FindCriticalPath(NodeEight, NodeTwo).Sum(x => x.CommTime), -1);
            Assert.AreEqual(creator.FindCriticalPath(NodeEight, NodeThree).Sum(x => x.CommTime), -5);
            Assert.AreEqual(creator.FindCriticalPath(NodeEight, NodeFour).Sum(x => x.CommTime), 4);
            Assert.AreEqual(creator.FindCriticalPath(NodeEight, NodeFive).Sum(x => x.CommTime), 9);
            Assert.AreEqual(creator.FindCriticalPath(NodeEight, NodeSix).Sum(x => x.CommTime), -4);
            Assert.AreEqual(creator.FindCriticalPath(NodeEight, NodeSeven).Sum(x => x.CommTime), 9);
            Assert.AreEqual(creator.FindCriticalPath(NodeEight, NodeEight).Sum(x => x.CommTime), 0);
        }
    }
}
