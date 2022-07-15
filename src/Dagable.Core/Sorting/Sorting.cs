using System.Collections.Generic;
using System.Linq;

namespace Dagable.Core
{
    public static class Sorting
    {
        /// <summary>
        /// Method that given a set of nodes and edges of a graph will perform Topological Sorting using Kahn's algorithm
        /// </summary>
        /// <remarks>https://en.wikipedia.org/wiki/Topological_sorting</remarks>
        /// <param name="nodes">All of the nodes of the graph</param>
        /// <param name="edges">All of the edges of the graph</param>
        /// <returns>Sorted graph nodes in a topological order</returns>
        public static List<N> KhansTopologySort<N, E>(HashSet<N> nodes, HashSet<E> edges) where N : INode<N> where E : IEdge<N>, new()
        {
            // L ← Empty list that will contain the sorted elements
            var L = new List<N>();

            // S ← Set of all nodes with no incoming edge
            var S = new HashSet<N>(nodes.Where(n => edges.All(x => x.NextNode.Equals(n) == false)));

            // while S is not empty do
            while (S.Any())
            {
                // remove a node n from S
                var n = S.First();
                S.Remove(n);

                // add n to L
                L.Add(n);

                // for each node m with an edge e from n to m do
                foreach (var e in edges.Where(e => e.PrevNode.Equals(n)).ToList())
                {
                    var m = e.NextNode;

                    // remove edge e from the graph
                    edges.Remove(e);

                    // if m has no other incoming edges then
                    if (edges.All(me => me.NextNode.Equals(m) == false))
                    {
                        // insert m into S
                        S.Add(m);
                    }
                }
            }

            // if graph has edges then
            // return error(graph has at least one cycle)          
            // else
            //  return L(a topologically sorted order)
            return edges.Any() ? null : L;
        }
    }
}
