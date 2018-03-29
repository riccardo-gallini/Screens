using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Screens.Hosting
{
    public class BufferManager
    {

        public Terminal Terminal { get; }

        public Buffer CurrentBuffer { get; private set; }
        public Buffer LastBuffer { get; private set; }

        public BufferManager(Terminal terminal)
        {
            Terminal = terminal;
        }

        public void FlushBuffer()
        {

            // send changed lines in buffer to the context (console or terminal)
            // in an optimized way

            var xs = 0;
            var ys = 0;


            // scrittura ottimizzata del buffer al terminale
            // a) manda una riga solo se è cambiata
            // b) manda la riga a 'pezzi' scrivendo stringhe intere a pari colore

            while (ys < CurrentBuffer.Height)
            {
                if (IsLineChanged(CurrentBuffer, LastBuffer, ys))
                {
                    xs = 0;
                    var str = new System.Text.StringBuilder();
                    var cur_fore = CurrentBuffer[xs, ys].ForeColor;
                    var cur_back = CurrentBuffer[xs, ys].BackColor;
                    var cur_x = 0;

                    while (xs < CurrentBuffer.Width)
                    {
                        var buf_char = CurrentBuffer[xs, ys];
                        if (buf_char.ForeColor != cur_fore || buf_char.BackColor != cur_back)
                        {
                            Terminal.Write(str.ToString(), cur_fore, cur_back, cur_x, ys);
                            str = new System.Text.StringBuilder();
                            cur_fore = buf_char.ForeColor;
                            cur_back = buf_char.BackColor;
                            cur_x = xs;
                        }
                        str.Append(buf_char.Ch);

                        xs += 1;
                    }
                    Terminal.Write(str.ToString(), cur_fore, cur_back, cur_x, ys);
                }
                ys += 1;
            }

            LastBuffer = (Buffer)CurrentBuffer.Clone();
        }

        private static bool IsLineChanged(Buffer a, Buffer b, int y)
        {
            if (b == null || a == null)
                return true;

            var x = 0;
            while (x < a.Width)
            {
                if (a[x, y] != b[x, y])
                    return true;
                x += 1;
            }
            return false;
        }

        public void ResetBuffer(Size size)
        {
            if (CurrentBuffer == null)
            {
                CurrentBuffer = new Buffer(size);
                LastBuffer = new Buffer(size);
            }
            else
            {
                LastBuffer.Clear();
                CurrentBuffer.Clear();
                Terminal.Clear();
            }
        }


    }
}
