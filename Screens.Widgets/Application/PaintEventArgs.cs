using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using System.Drawing;

namespace Screens
{

    public class PaintEventArgs : EventArgs
    {
        public bool Handled { get; set; } = false;

        private Rectangle _clipRectangle;
        public Rectangle ClipRectangle
        {
            get
            {
                return _clipRectangle;
            }
        }

        private Buffer _buffer;
        public Buffer Buffer
        {
            get
            {
                return _buffer;
            }
        }

        internal PaintEventArgs(Rectangle clipRectangle, Buffer buffer)
        {
            _clipRectangle = clipRectangle;
            _buffer = buffer;
        }
    }
}
