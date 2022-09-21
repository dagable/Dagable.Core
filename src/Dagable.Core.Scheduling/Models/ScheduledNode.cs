using Dagable.Core.Models;

namespace Dagable.Core.Scheduling
{
    public class ScheduledNode
    {
        public int StartAt { get; set; }
        public int EndAt { get; set; }
        public CPathNode Node { get; set; }

        public ScheduledNode(CPathNode node, int startAt, int endAt)
        {
            Node = node;
            StartAt = startAt;
            EndAt = endAt;
        }
    }

    public class UnscheduledNode
    {
        public int StaticBLevel { get; set; }

        public CPathNode Node { get; set; }

        public UnscheduledNode(CPathNode node, int staticBLevel)
        {
            Node = node;
            StaticBLevel = staticBLevel;
        }
    }
}
