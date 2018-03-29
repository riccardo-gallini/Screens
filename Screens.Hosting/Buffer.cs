using System;
using System.Drawing;
using System.Text;

namespace Screens
{

    public struct BufferChar : IEquatable<BufferChar>
    {
        public char Ch;
        public ConsoleColor ForeColor;
        public ConsoleColor BackColor;

        public static bool operator ==(BufferChar a, BufferChar b)
        {
            if (a.Ch == b.Ch && a.ForeColor == b.ForeColor && a.BackColor == b.BackColor)
                return true;
            else
                return false;
        }

        public static bool operator !=(BufferChar a, BufferChar b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return (this == (BufferChar)obj);
        }

        public bool Equals(BufferChar other)
        {
            return (this == other);
        }
    }

    public class Buffer : ICloneable
    {
        private Size _size;
        private Rectangle _clipRectangle;
        private BufferChar[][] _internalBuffer;

        public Buffer(Size size)
        {
            _size = size;
            _clipRectangle = new Rectangle(new Point(0, 0), _size);
            _internalBuffer = new BufferChar[_size.Height - 1 + 1][];

            CurrentForeColor = ConsoleColor.White;
            CurrentBackColor = ConsoleColor.Black;


            var x = 0;
            var y = 0;

            for (y = 0; y <= _size.Height - 1; y++)
            {
                _internalBuffer[y] = new BufferChar[_size.Width - 1 + 1];
                for (x = 0; x <= _size.Width - 1; x++)
                    _internalBuffer[y][x] = new BufferChar() { Ch = ' ', BackColor = CurrentBackColor, ForeColor = CurrentForeColor };
            }
        }


        public Size Size
        {
            get
            {
                return _size;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Height
        {
            get
            {
                return _size.Height;
            }
        }

        public Rectangle ClipRectangle
        {
            get
            {
                return _clipRectangle;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Width
        {
            get
            {
                return _size.Width;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ConsoleColor CurrentForeColor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ConsoleColor CurrentBackColor { get; set; }



        public BufferChar this[int x, int y]
        {
            get
            {
                return _internalBuffer[y][x];
            }
            set
            {
                if (_internalBuffer[y][x] != value)
                    _internalBuffer[y][x] = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="dest"></param>
        /// <param name="foreColor"></param>
        /// <param name="backColor"></param>
        /// <remarks></remarks>
        public void DrawString(string s, Rectangle dest, ConsoleColor foreColor, ConsoleColor backColor)
        {
            var i = 0;
            var x = dest.X;
            var y = dest.Y;

            var clip_bottom = dest.Y + dest.Height;
            var clip_right = dest.X + dest.Width;

            var last_cr = false;
            BufferChar ch;

            // read the string s, char by char
            while (i < s.Length && y < clip_bottom)
            {
                if (s[i] == '\n')
                {

                    // if vbCr ==> return to left; one more row
                    x = dest.X;
                    y += 1;
                    last_cr = true;
                }
                else if (s[i] == '\r')
                {

                    // if it was vbCrLf do nothing
                    // otherwise ==> return to left; one more row
                    if (last_cr == false)
                    {
                        x = dest.X;
                        y += 1;
                    }

                    last_cr = false;
                }
                else

                // if visibile, normal character to be printed

                if (y >= 0 && y < _size.Height && x >= 0 && x < _size.Width && y < clip_bottom && x < clip_right)
                {
                    ch = new BufferChar() { Ch = s[i], ForeColor = foreColor, BackColor = backColor };
                    this[x, y] = ch;

                    last_cr = false;

                    x += 1;

                    // wrap around
                    if (x >= clip_right)
                    {
                        x = dest.X;
                        y += 1;
                    }
                }

                i += 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="dest"></param>
        /// <remarks></remarks>
        public void DrawString(string s, Rectangle dest)
        {
            DrawString(s, dest, CurrentForeColor, CurrentBackColor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="foreColor"></param>
        /// <param name="backColor"></param>
        /// <remarks></remarks>
        public void DrawString(string s, ConsoleColor foreColor, ConsoleColor backColor)
        {
            DrawString(s, _clipRectangle, foreColor, backColor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <remarks></remarks>
        public void DrawString(string s)
        {
            DrawString(s, CurrentForeColor, CurrentBackColor);
        }

        /// <summary>
        /// Writes the contents of <para>sourceBuffer</para> starting from point <para>dest</para> and clipping size according to <para>clip</para>.
        /// </summary>
        /// <param name="sourceBuffer"></param>
        /// <param name="dest"></param>
        /// <remarks></remarks>
        public void WriteBuffer(Buffer sourceBuffer, Rectangle dest)
        {

            // do the clipping!

            var x_source = 0;
            var y_source = 0;
            var x_dest = dest.X;
            var y_dest = dest.Y;

            var clip_bottom = dest.Y + dest.Height;
            var clip_right = dest.X + dest.Width;

            while (y_source < sourceBuffer.Height)
            {
                x_source = 0;
                x_dest = dest.X;
                while (x_source < sourceBuffer.Width)
                {

                    // if visible both by destRect and buffersize
                    if (y_dest >= 0 && y_dest < _size.Height && x_dest >= 0 && x_dest < _size.Width && y_dest < clip_bottom && x_dest < clip_right)
                        this[x_dest, y_dest] = sourceBuffer[x_source, y_source];

                    x_source += 1;
                    x_dest += 1;
                }

                y_source += 1;
                y_dest += 1;
            }
        }

        /// <summary>
        /// Writes the contents of <para>sourceBuffer</para> starting from point <para>dest</para>.
        /// </summary>
        /// <param name="sourceBuffer"></param>
        /// <param name="dest"></param>
        /// <remarks></remarks>
        public void WriteBuffer(Buffer sourceBuffer, Point dest)
        {
            WriteBuffer(sourceBuffer, new Rectangle(dest, _size));
        }

        /// <summary>
        /// Clears the buffer using current background and foreground colors.
        /// </summary>
        /// <remarks></remarks>
        public void Clear()
        {
            Clear(' ');
        }

        /// <summary>
        /// Clears the buffer using the character specified by <para>fillChar</para> and current background and foreground colors.
        /// </summary>
        /// <param name="fillChar"></param>
        /// <remarks></remarks>
        public void Clear(char fillChar)
        {
            Clear(fillChar, CurrentForeColor, CurrentBackColor);
        }

        /// <summary>
        /// Clears the buffer using the character specified by <para>fillChar</para> and colors specified by <para>fore</para> and <para>back</para>.
        /// </summary>
        /// <param name="fillChar"></param>
        /// <remarks></remarks>
        public void Clear(char fillChar, ConsoleColor foreColor, ConsoleColor backColor)
        {
            Fill(fillChar, ClipRectangle, foreColor, backColor);
        }

        /// <summary>
        /// Fills the rectangle specified by <para>dest</para> and <para>size</para> with the <para>fillChar</para> character and current colors.
        /// </summary>
        /// <param name="fillChar"></param>
        /// <param name="dest"></param>
        /// <remarks></remarks>
        public void Fill(char fillChar, Rectangle dest)
        {
            Fill(fillChar, dest, CurrentForeColor, CurrentBackColor);
        }

        /// <summary>
        /// Fills the rectangle specified by <para>dest</para> and <para>size</para> with the <para>fillChar</para> character and colors specified by <paramref name="fore"/> and <paramref name="back"/>
        /// </summary>
        /// <param name="fillChar"></param>
        /// <param name="dest"></param>
        /// <param name="foreColor"></param>
        /// <param name="backColor"></param>
        /// <remarks></remarks>
        public void Fill(char fillChar, Rectangle dest, ConsoleColor foreColor, ConsoleColor backColor)
        {
            var x = dest.X;
            var y = dest.Y;

            var clip_bottom = dest.Y + dest.Height;
            var clip_right = dest.X + dest.Width;

            BufferChar ch;

            while (y < clip_bottom)
            {
                x = dest.X;

                while (x < clip_right)
                {
                    ch = new BufferChar() { Ch = fillChar, ForeColor = foreColor, BackColor = backColor };
                    this[x, y] = ch;

                    x += 1;
                }
                y += 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            var sb = new StringBuilder();

            var x = 0;
            var y = 0;

            while (y < _size.Height)
            {
                x = 0;
                while (x < _size.Width)
                {
                    sb.Append(_internalBuffer[y][x].Ch);
                    x += 1;
                }
                sb.Append("\n\r");
                y += 1;
            }

            return sb.ToString();
        }

        public object Clone()
        {
            var new_buffer = new Buffer(this.Size);
            new_buffer.WriteBuffer(this, this.ClipRectangle);
            return new_buffer;
        }
    }

}