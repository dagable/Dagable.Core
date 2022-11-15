using Dagable.Core.Scheduling.Models.DTO;

namespace Dagable.Core.Scheduling
{
    public interface ITaskGraphSchedulingService
    {
        /// <summary>
        /// Method used to schedule a task graph using the DLS scheduling algorithm
        /// </summary>
        /// <param name="processorCount">The number of processors to use when scheduling</param>
        /// <param name="graph">The task graph that needs scheduling</param>
        /// <returns>A Scheduled task graph optimised using the number of processors.</returns>
        IScheduledGraph DLSchedule(int processorCount, ICriticalPathTaskGraph graph);
    }
}