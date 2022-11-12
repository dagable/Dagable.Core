using Dagable.Core.Scheduling.Models.DTO;

namespace Dagable.Core.Scheduling
{
    public sealed class TaskGraphSchedulingService : ITaskGraphSchedulingService
    {
        public IScheduledGraph DLSchedule(int processorCount, ICriticalPathTaskGraph graph)
        {
            var scheduler = new DLScheduler(processorCount, graph);
            return new ScheduledGraph(scheduler.Schedule());
        }
    }
}
