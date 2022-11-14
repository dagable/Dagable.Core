using System.Collections.Generic;

namespace Dagable.Core.Scheduling
{
    internal interface IScheduler
    {
        /// <summary>
        /// Method used to schedule a task graph
        /// </summary>
        /// <returns>A List of nodes mapped to a processor as a dictionary</returns>
        Dictionary<int, List<ScheduledNode>> Schedule();
    }
}