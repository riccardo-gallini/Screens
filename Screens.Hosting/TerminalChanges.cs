using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Screens.Hosting
{
    public class TerminalChanges
    {
        private Dictionary<int, ChangedLine> _inner = new Dictionary<int, ChangedLine>();

        public ChangedLine AddLine(int y)
        {
            var line = new ChangedLine(y);
            _inner.Add(y, line);
            return line;
        }

        public Dictionary<int, ChangedLine>.ValueCollection Lines
        {
            get
            {
                return _inner.Values;
            }
        }
    }

    public class ChangedLine
    {
        public List<Span> Spans { get; } = new List<Span>();

        public int Y { get; }

        public ChangedLine(int y)
        {
            Y = y;
        }

        public Span AddSpan(string text, ConsoleColor fore, ConsoleColor back, int x)
        {
            var span = new Span(text, fore, back, x);
            Spans.Add(span);
            return span;
        }
    }


    public class Span
    {
        public string Text { get; }
        public ConsoleColor ForeColor { get; }
        public ConsoleColor BackColor { get; }
        public int X { get; }

        public Span(string text, ConsoleColor fore, ConsoleColor back, int x)
        {
            Text = text;
            ForeColor = fore;
            BackColor = back;
            X = x;
        }
    }

}
