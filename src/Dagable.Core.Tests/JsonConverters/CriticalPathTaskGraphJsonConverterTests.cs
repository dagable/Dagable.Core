using Dagable.Core.Models;
using Dagable.Core.Scheduling;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Dagable.Core.Tests.JsonConverters
{
    [TestClass]
    public class CriticalPathTaskGraphJsonConverterTests
    {

        private readonly Mock<ICriticalPathTaskGraph> _criticalTaskGraph;

        private static readonly HashSet<CriticalPathNode> _criticalNodes = new()
        {
            new CriticalPathNode(1, 0, 10), //0
            new CriticalPathNode(2, 1, 1), //1
            new CriticalPathNode(3, 1, 10), //2
            new CriticalPathNode(4, 2, 10), //3
            new CriticalPathNode(5, 2, 1), //4
        };

        private static readonly HashSet<CriticalPathEdge> _criticalEdges = new()
        {
            new CriticalPathEdge(_criticalNodes.ElementAt(0), _criticalNodes.ElementAt(1), 2),
            new CriticalPathEdge(_criticalNodes.ElementAt(0), _criticalNodes.ElementAt(2), 2),
            new CriticalPathEdge(_criticalNodes.ElementAt(1), _criticalNodes.ElementAt(3), 2),
            new CriticalPathEdge(_criticalNodes.ElementAt(2), _criticalNodes.ElementAt(3), 2),
            new CriticalPathEdge(_criticalNodes.ElementAt(2), _criticalNodes.ElementAt(4), 2),
        };

        private static readonly List<CriticalPathEdge> _criticalPathEdges = new()
        {
            _criticalEdges.ElementAt(1),
            _criticalEdges.ElementAt(3),
        };

        private static readonly string _criticalPathTaskGraphJsonResult = File.ReadAllText($"{ProjectSourcePath.Value}JsonConverters/JsonConverter.CriticalPathTaskGraph-output.json");
        private const int layer = 3;

        public CriticalPathTaskGraphJsonConverterTests()
        {
            _criticalTaskGraph = new Mock<ICriticalPathTaskGraph>();
            _criticalTaskGraph.Setup(x => x.Nodes).Returns(_criticalNodes);
            _criticalTaskGraph.Setup(x => x.Edges).Returns(_criticalEdges);
            _criticalTaskGraph.Setup(x => x.Layers).Returns(layer);
            _criticalTaskGraph.Setup(x => x.GetCriticalPathEdges).Returns(_criticalPathEdges);
        }

        [TestMethod]
        public void When_ConvertingCrtiicalTaskGraph_ToJson_ValueIsCorrectFormat()
        {
            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var jsonString = JsonSerializer.Serialize(_criticalTaskGraph.Object, serializeOptions);
            Assert.AreEqual(_criticalPathTaskGraphJsonResult, jsonString);
        }

        [TestMethod]
        public void When_ConvertingCriticalTaskGraph_FromJson_ValueIsCorrect()
        {
            var desObject = JsonSerializer.Deserialize<ICriticalPathTaskGraph>(_criticalPathTaskGraphJsonResult);
            Assert.AreEqual(_criticalTaskGraph.Object.Nodes.Count, desObject.Nodes.Count);
            Assert.AreEqual(_criticalTaskGraph.Object.Edges.Count, desObject.Edges.Count);
        }

        [TestMethod]
        public void DLSSchedule()
        {
            var desObject = JsonSerializer.Deserialize<ICriticalPathTaskGraph>(_criticalPathTaskGraphJsonResult);

            var scheduler = new DLScheduler(3, desObject);

            var results = scheduler.Schedule();

            Assert.IsNotNull(results);
#if DEBUG
            foreach (var res in results)
            {
                Debug.WriteLine($"processor: {res.Key + 1}");
                foreach (var item in res.Value)
                {
                    Debug.WriteLine($"    item: {item.Id + 1}, start: {item.StartAt} end: {item.EndAt}");
                }
            }

#endif
        }
    }
}
