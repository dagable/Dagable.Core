using System;
using System.Collections.Generic;
using System.Linq;

namespace Dagable.Core.Scheduling.Models.DTO
{
    public class ScheduledGraph : IScheduledGraph
    {
        public Dictionary<int, List<ScheduledNode>> NodeProcessorMappings { get; }
        public int MaxLengthRoundedUp { get; }
        public int ProcessorCount { get; }

        public ScheduledGraph(Dictionary<int, List<ScheduledNode>> scheduledGraph)
        {
            NodeProcessorMappings = scheduledGraph;
            var maxEndTime = NodeProcessorMappings.SelectMany(x => x.Value).Max(x => x.EndAt);
            MaxLengthRoundedUp = (int)(Math.Ceiling(maxEndTime / 10.0d) * 10);
            ProcessorCount = NodeProcessorMappings.Select(x => x.Value).Count(x => x.Any());
        }
    }
}
