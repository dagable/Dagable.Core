using System;
using System.Collections.Generic;
using System.Text;

namespace Dagable.Core.Models
{
    public class CPathNode : Node, INode<CPathNode>
    {
        public int ComputationTime { get; set; }

        public CPathNode() : base() { }

        public CPathNode AddSuccessor(CPathNode n)
        {
            throw new NotImplementedException();
        }

        public CPathNode AddPredecessor(CPathNode n)
        {
            throw new NotImplementedException();
        }

        CPathNode INode<CPathNode>.UpdateLayer(int layer)
        {
            throw new NotImplementedException();
        }
    }
}
