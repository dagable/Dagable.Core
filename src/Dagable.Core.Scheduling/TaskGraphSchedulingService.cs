using System.Collections.Generic;

namespace Dagable.Core.Scheduling
{
    public sealed class TaskGraphSchedulingService : ITaskGraphSchedulingService
    {
        public Dictionary<int, List<ScheduledNode>> DLSchedule(int processorCount, ICriticalPathTaskGraph graph)
        {
            var scheduler = new DLScheduler(processorCount, graph);
            return scheduler.Schedule();
        }
    }
}
