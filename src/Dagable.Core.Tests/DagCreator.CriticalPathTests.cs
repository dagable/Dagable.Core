using Dagable.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dagable.Core.Tests
{
    [TestClass]
    public class DagCreatorCriticalPath
    {

        private CPathNode NodeOne = new CPathNode(0);
        private CPathNode NodeTwo = new CPathNode(1);
        private CPathNode NodeThree = new CPathNode(2);
        private CPathNode NodeFour = new CPathNode(3);
        private CPathNode NodeFive = new CPathNode(4);
        private CPathNode NodeSix = new CPathNode(5);
        private CPathNode NodeSeven = new CPathNode(6);
        private CPathNode NodeEight = new CPathNode(7);


        /*
         *  new Edge(0, 6, 2), new Edge(1, 2, -4), new Edge(1, 4, 1),
                new Edge(1, 6, 8), new Edge(3, 0, 3), new Edge(3, 4, 5),
                new Edge(5, 1, 2), new Edge(7, 0, 6), new Edge(7, 1, -1),
                new Edge(7, 3, 4), new Edge(7, 5, -4)
        */

        [TestMethod]
        public void CriticalPathIsCorrect()
        {
            DagCreator.CriticalPath creator = new DagCreator.CriticalPath();
            creator.dagGraph = new Graph<CPathNode, CPathEdge>();
            creator.dagGraph.AddNode(NodeOne);
            creator.dagGraph.AddNode(NodeTwo);
            creator.dagGraph.AddNode(NodeThree);
            creator.dagGraph.AddNode(NodeFour);
            creator.dagGraph.AddNode(NodeFive);
            creator.dagGraph.AddNode(NodeSix);
            creator.dagGraph.AddNode(NodeSeven);
            creator.dagGraph.AddNode(NodeEight);
            creator.dagGraph.AddEdge(new CPathEdge(NodeOne, NodeSeven, 2));
            creator.dagGraph.AddEdge(new CPathEdge(NodeTwo, NodeThree, -4));
            creator.dagGraph.AddEdge(new CPathEdge(NodeTwo, NodeFive, 1));
            creator.dagGraph.AddEdge(new CPathEdge(NodeTwo, NodeSeven, 8));
            creator.dagGraph.AddEdge(new CPathEdge(NodeFour, NodeOne, 3));
            creator.dagGraph.AddEdge(new CPathEdge(NodeFour, NodeFive, 5));
            creator.dagGraph.AddEdge(new CPathEdge(NodeSix, NodeTwo, 2));
            creator.dagGraph.AddEdge(new CPathEdge(NodeEight, NodeOne, 6));
            creator.dagGraph.AddEdge(new CPathEdge(NodeEight, NodeTwo, -1));
            creator.dagGraph.AddEdge(new CPathEdge(NodeEight, NodeFour, 4));
            creator.dagGraph.AddEdge(new CPathEdge(NodeEight, NodeSix, -4));

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
