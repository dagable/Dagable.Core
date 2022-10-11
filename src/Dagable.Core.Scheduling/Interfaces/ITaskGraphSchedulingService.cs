using System.Collections.Generic;

namespace Dagable.Core.Scheduling
{
    public interface ITaskGraphSchedulingService
    {
        Dictionary<int, List<ScheduledNode>> DLSchedule(int processorCount, ICriticalPathTaskGraph graph);
    }
}