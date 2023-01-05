using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Dagable.Core.Tests.JsonConverters
{
    [TestClass]
    public class StandardTaskGraphJsonConverterTests
    {

        private readonly Mock<IStandardTaskGraph<StandardNode, StandardEdge<StandardNode>>> _standardTaskGraph;

        private static readonly HashSet<StandardNode> _standardNodes = new()
        {
            new StandardNode(0, 0), //0
            new StandardNode(1, 1), //1
            new StandardNode(2, 1), //2
            new StandardNode(3, 2), //3
            new StandardNode(4, 2), //4
        };

        private static readonly HashSet<StandardEdge<StandardNode>> _standardEdges = new()
        {
            new StandardEdge<StandardNode>(_standardNodes.ElementAt(0), _standardNodes.ElementAt(1)),
            new StandardEdge<StandardNode>(_standardNodes.ElementAt(0), _standardNodes.ElementAt(2)),
            new StandardEdge<StandardNode>(_standardNodes.ElementAt(1), _standardNodes.ElementAt(3)),
            new StandardEdge<StandardNode>(_standardNodes.ElementAt(2), _standardNodes.ElementAt(3)),
            new StandardEdge<StandardNode>(_standardNodes.ElementAt(2), _standardNodes.ElementAt(4)),
        };

        private static readonly string _standardTaskGraphJsonResult = File.ReadAllText($"{ProjectSourcePath.Value}JsonConverters/JsonConverter.StandardTaskGraph-output.json");
        private const int layer = 3;

        public StandardTaskGraphJsonConverterTests()
        {
            _standardTaskGraph = new Mock<IStandardTaskGraph<StandardNode, StandardEdge<StandardNode>>>();
            _standardTaskGraph.Setup(x => x.Nodes).Returns(_standardNodes);
            _standardTaskGraph.Setup(x => x.Edges).Returns(_standardEdges);
            _standardTaskGraph.Setup(x => x.Layers).Returns(layer);
        }

        [TestMethod]
        public void When_ConvertingStandardTaskGraph_ToJson_ValueIsCorrectFormat()
        {
            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var jsonString = JsonSerializer.Serialize(_standardTaskGraph.Object, serializeOptions);
            Assert.AreEqual(_standardTaskGraphJsonResult, jsonString);
        }

    }
}
