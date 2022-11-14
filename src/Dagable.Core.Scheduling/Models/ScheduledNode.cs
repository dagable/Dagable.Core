using Dagable.Core.Models;

namespace Dagable.Core.Scheduling
{
    public class ScheduledNode
    {
        public int StartAt { get; set; }
        public int EndAt { get; set; }
        public int Id { get; set; }
        public int ComputationTime { get; set; }
        
        public ScheduledNode(CriticalPathNode node, int startAt, int endAt)
        {
            StartAt = startAt;
            EndAt = endAt;
            Id = node.Id;
            ComputationTime = node.ComputationTime;
        }
    }

    public class UnscheduledNode
    {
        public int StaticBLevel { get; set; }

        public CriticalPathNode Node { get; set; }

        public UnscheduledNode(CriticalPathNode node, int staticBLevel)
        {
            Node = node;
            StaticBLevel = staticBLevel;
        }
    }
}
