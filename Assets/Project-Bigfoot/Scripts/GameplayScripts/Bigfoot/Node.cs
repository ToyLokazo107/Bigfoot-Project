using System.Collections.Generic;

namespace Bigfoot.Collections.Graphs
{
    public class Node<T>
    {
        private T value;
        private List<Node<T>> neighbors = new();

        public Node(T value)
        {
            this.value = value;
        }

        public void Connect(Node<T> node)
        {
            if (!neighbors.Contains(node))
            {
                neighbors.Add(node);
            }

            if (!node.neighbors.Contains(this))
            {
                node.neighbors.Add(this);
            }
        }

        public void Disconnect(Node<T> node)
        {
            if (neighbors.Contains(node))
                neighbors.Remove(node);

            if (node.neighbors.Contains(this))
                node.neighbors.Remove(this);
        }

        public T Value => value;
        public List<Node<T>> Neighbors => neighbors;
    }
}