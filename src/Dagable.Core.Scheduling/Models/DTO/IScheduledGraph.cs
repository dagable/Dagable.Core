using System.Collections.Generic;

namespace Dagable.Core.Scheduling.Models.DTO
{
    public interface IScheduledGraph
    {
        /// <summary>
        /// The critical path length rounded to the nearest 10 for rendering
        /// </summary>
        int MaxLengthRoundedUp { get; }

        /// <summary>
        /// The scheduled task graph mapping processors to a list of nodes
        /// </summary>
        Dictionary<int, List<ScheduledNode>> NodeProcessorMappings { get; }

        /// <summary>
        /// The number of processors that was used in total.
        /// </summary>
        int ProcessorCount { get; }
    }
}