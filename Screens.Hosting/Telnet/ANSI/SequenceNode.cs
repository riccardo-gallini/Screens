using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Screens.Hosting
{
    // Tree/node class
    public class SequenceNode<T, V>
    {
        public T value;
        public List<SequenceNode<T, V>> Children;
        public V Translation;

        public SequenceNode<T, V> AddChild(T child_value)
        {
            var child_node = new SequenceNode<T, V>(child_value);
            Children.Add(child_node);

            return child_node;
        }

        public SequenceNode(T _value)
        {
            Children = new List<SequenceNode<T, V>>();
            value = _value;
        }

        public SequenceNode<T, V> FindChild(T v)
        {
            return Children.Where((n) => n.value.Equals(v)).FirstOrDefault();
        }

    }
}
