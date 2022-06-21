using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dagable.Core.Extensions
{
    public static class HashSetExtensions
    {
        public static HashSet<Node> ReorderNodeLayers(this HashSet<Node> nodes, int layer = 0)
        {
            var dictionary = RecursiveLayerCall(nodes.OrderBy(x => x.Layer).ToList(), nodes.OrderBy(x => x.Layer).ToDictionary(x => x, x => new List<int>()),  0);


            var updateHashset = new HashSet<Node>();

            foreach(var node in nodes)
            {
                updateHashset.Add(node.UpdateLayer(dictionary[node].Max()));
            }
            
            return updateHashset;
        }

        private static Dictionary<Node, List<int>> RecursiveLayerCall(List<Node> nodes, Dictionary<Node, List<int>> mapping, int layer = 0)
        {
            var nodeLayerDictionary = mapping;

            foreach (var node in nodes.ToList())
            {
                if (!nodeLayerDictionary.ContainsKey(node))
                {
                    nodeLayerDictionary[node] = new List<int> { layer };
                }
                nodeLayerDictionary[node].Add(layer);
                nodeLayerDictionary.Union(RecursiveLayerCall(node.SuccessorNodes.ToList(), nodeLayerDictionary, layer + 1))
                    .ToDictionary(d => d.Key, d => d.Value);
            }

            return nodeLayerDictionary;
        }
    }
}
