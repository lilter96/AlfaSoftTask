using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ConsoleApp3
{
    sealed public class Node<T>
    {
        private readonly T _value;
        private readonly List<Node<T>> _children = new List<Node<T>>();
        public Node<T> Parent { get; private set; }
        public T Value { get => _value; }
        public Node(T value)
        {
            _value = value;
        }
        public ReadOnlyCollection<Node<T>> Children => _children.AsReadOnly();
        public Node<T> AddChild(T value)
        {
            var child = new Node<T>(value) { Parent = this };
            _children.Add(child);
            return child;
        }
        public bool RemoveChild(Node<T> node)
        {
            return _children.Remove(node);
        }
        public IEnumerable<T> Flatten()
        {
            return new[] { Value }.Concat(_children.SelectMany(x => x.Flatten()));
        }
    }
    
    sealed public class Tree<V>
    {
        private Node<V> _root;
        private int _size;
        public Node<V> Root { get => _root; }
        public int Size { get => _size; }
        public Node<V> AddNode(Node<V> parent, V value)
        {
            _size++;
            if (_root == null)
            {
                _root = new Node<V>(value);
                return _root;
            }
            return parent?.AddChild(value);
        }
        
        public Node<V> GetNodeByValueOrDefault(V value)
        {
            var queue = new Queue<Node<V>>();
            queue.Enqueue(_root);
            while (queue.Count != 0)
            {
                var current = queue.Dequeue();
                foreach (var child in current.Children)
                {
                    if (child.Value.Equals(value))
                    {
                        return child;
                    }
                    queue.Enqueue(child);
                }
            }
            return null;
        }

        public bool RemoveNode(Node<V> node)
        {
            if (node == null)
            {
                return false;
            }
            return node.Parent.RemoveChild(node);
        }
    }
    class Program
    {
        static void Main()
        {
            Tree<string> A = new Tree<string>();
            var green = A.AddNode(null, "Green");
            var blue = A.AddNode(green, "Blue");
            var red = A.AddNode(green, "Red");
            var purple = A.AddNode(green, "Purple");
            var orange = A.AddNode(blue, "Orange");
            var yellow = A.AddNode(blue, "Yellow");
            var lightBlue = A.AddNode(purple, "LightBlue");
            var node = A.GetNodeByValueOrDefault("Yellow");
            foreach(var x in green.Flatten())
            {
                Console.WriteLine(x);
            }
        }
    }
}
