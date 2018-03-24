using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace Screens.Hosting
{

    //This stateful parser is used to parse a stream of Ts, posted one-by-one, against a given sequence of type Sequence<T,V>.
    //If it finds a match, it launches the FoundMatch function on the corresponding V value (defined by Sequence)
    //If the sequence does not match (e.g. unexpected value), it translates each pending value through the Fallback function
    //  and calls FoundMatch for each translated value.
    public class SequenceParser<T, V>
    {
        private SequenceNode<T, V> _current;
        private SequenceNode<T, V> _start;
        private List<T> _accumulated = new List<T>();

        public Action<V> FoundMatch { get; set; }
        public Func<T, V> FallBack { get; set; }
        

        //Initialize a new parser with sequences
        public SequenceParser(Sequence<T, V> sequences)
        {
            _start = sequences.Tree;
            _current = _start;
        }
        
        //Post a value to the parser
        public void Post(T posted_value)
        {

            //A top level token flushes the accumulated sequence and restarts the parser.
            var top_level = _start.FindChild(posted_value);

            if (top_level != null)
            {
                foreach (var value in _accumulated)
                {
                    FoundMatch(FallBack(value));
                }
                _accumulated.Clear();
            }
            
            var found_node = _current.FindChild(posted_value);
            

            if (found_node == null)
            {
                _accumulated.Add(posted_value);

                foreach(var value in _accumulated)
                {
                    FoundMatch(FallBack(value));
                }

                _current = _start;
                _accumulated.Clear();
            }
            else if (found_node.Translation != null)
            {
                FoundMatch(found_node.Translation);

                _current = _start;
                _accumulated.Clear();
            }
            else
            {
                _accumulated.Add(posted_value);
                _current = found_node;
            }
            
        }

    }

}
