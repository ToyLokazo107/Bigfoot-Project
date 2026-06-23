using Bigfoot.Collections.Graphs;
using System.Collections.Generic;
using UnityEngine;

namespace Bigfoot.Collections.Graphs
{
    public class NonOrientedGraph<T>
    {
        private List<Node<T>> nodes = new();

        public List<Node<T>> Nodes => nodes;

        public Node<T> AddNode(T value)
        {
            Node<T> newNode = new Node<T>(value);
            nodes.Add(newNode);
            return newNode;
        }

        public void AddEdges(int posA, int posB)
        {
            nodes[posA].Connect(nodes[posB]);
        }

        public void PrintAdjancencyList()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                string nodeList = "Node Nro: " + nodes[i].Value.ToString() + " => ";

                for (int j = 0; j < nodes[i].Neighbors.Count; j++)
                {
                    nodeList += nodes[i].Neighbors[j].Value.ToString() + ", ";
                }

                Debug.Log(nodeList);
            }
        }
    }
}