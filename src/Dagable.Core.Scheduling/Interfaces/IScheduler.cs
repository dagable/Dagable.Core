using System.Collections.Generic;

namespace Dagable.Core.Scheduling
{
    public interface IScheduler
    {
        Dictionary<int, List<ScheduledNode>> Schedule();
    }
}