using Dagable.Core.Scheduling.Models.DTO;

namespace Dagable.Core.Scheduling
{
    public interface ITaskGraphSchedulingService
    {
        IScheduledGraph DLSchedule(int processorCount, ICriticalPathTaskGraph graph);
    }
}