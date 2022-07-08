using System;
using System.Collections.Generic;
using System.Text;

namespace Dagable.Core
{
    public sealed partial class DagCreator
    {
        public class CriticalPath : Standard, IDagCreation<CriticalPath>
        {
            private int MinComp { get; set; }
            private int MaxComp { get; set; }
            private int MinComm { get; set; }
            private int MaxComm { get; set; }

            public CriticalPath() : base() { }

            public CriticalPath(int layers) : base(layers)
            {
            }

            public CriticalPath(int layers, int nodeCount) : base(layers, nodeCount)
            {
            }

            public CriticalPath(int layers, int nodeCount, double probability) : base(layers, nodeCount, probability)
            {

            }

            CriticalPath IDagCreation<CriticalPath>.Generate()
            {
                throw new NotImplementedException();
            }
        }
    }
}
