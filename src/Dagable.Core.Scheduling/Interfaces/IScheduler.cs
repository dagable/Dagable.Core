using System.Collections.Generic;

namespace Dagable.Core.Scheduling
{
    internal interface IScheduler
    {
        Dictionary<int, List<ScheduledNode>> Schedule();
    }
}