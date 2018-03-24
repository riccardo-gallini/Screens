using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Screens.Hosting
{

    // Stores a sequence values of type T as tree; SequenceParser can match a stream of values against this kind of sequence to find a match.
    // Each sequence ends with a translated value of type V
    public class Sequence<T, V> 
    {

        // Stores the internal representation of the sequence in form of a tree
        public SequenceNode<T, V> Tree { get; }
        
        public Sequence()
        {
            Tree = new SequenceNode<T, V>(default);
        }

        //Takes an array of values and stores internally as a tree, where common values from different sequences are represented by the same node.
        public void AddSequence(T[] seq, V translation)
        {
            SequenceNode<T, V> _current = this.Tree;

            foreach (var value in seq)
            {
                var node = _current.FindChild(value);
                if (node == null)
                {
                    node = _current.AddChild(value);
                }
                _current = node;
            }

            //the last node has the translated value
            _current.Translation = translation;
        }

    }

    
}
