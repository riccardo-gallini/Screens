using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Screens.Hosting
{

    public abstract class Terminal
    {
        public Action<KeyInfo> KeyPressed;

        public bool BlackAndWhite { get; set; }
        public Size ScreenSize { get; set; }

        public abstract void Clear();
        
        public abstract void SetScreenSize(int width, int height);
        public abstract void SetCursorPosition(int x, int y);
        public abstract void SetBackGroundColor(ConsoleColor back);
        public abstract void SetForeGroundColor(ConsoleColor back);
        public abstract void HideCursor();
        public abstract void ShowCursor();

        public abstract void Write(string s);
        public abstract void Write(string s, ConsoleColor fore, ConsoleColor back, int x, int y);
        public abstract void Beep();

        public BufferManager BufferManager { get; }

        public Terminal()
        {
            BufferManager = new BufferManager(this);
        }

        public void ResetBuffer()
        {
            BufferManager.ResetBuffer();
        }

        public Buffer CurrentBuffer
        {
            get
            {
                return BufferManager.CurrentBuffer;
            }
        }

        public void FlushBuffer()
        {
            BufferManager.FlushBuffer();
        }

        public void SendKey(KeyInfo key)
        {
            KeyPressed?.Invoke(key);
        }
 
    }

}