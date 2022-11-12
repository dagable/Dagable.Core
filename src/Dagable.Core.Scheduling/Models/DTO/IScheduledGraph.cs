using System.Collections.Generic;

namespace Dagable.Core.Scheduling.Models.DTO
{
    public interface IScheduledGraph
    {
        int MaxLengthRoundedUp { get; }
        Dictionary<int, List<ScheduledNode>> NodeProcessorMappings { get; }
        int ProcessorCount { get; }
    }
}