using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Screens.Hosting
{

    public abstract class Terminal
    {
        public Action<KeyInfo> KeyPressed;
        public Action Closed;

        public bool BlackAndWhite { get; set; }
        public Size ScreenSize { get; set; }


        public int CursorX { get; set; }
        public int CursorY { get; set; }
        

        public abstract void Clear();
        
        public abstract void SetScreenSize(int width, int height);

        public void SetCursorPosition(int x, int y)
        {
            CursorX = x;
            CursorY = y;
            SetCursorPositionImpl();
        }

        protected abstract void SetCursorPositionImpl();

        public abstract void HideCursor();
        public abstract void ShowCursor();
                
        public abstract void SubmitChanges(TerminalChanges changes);
        public abstract void Beep();

        public BufferManager BufferManager { get; }

        public Terminal()
        {
            BufferManager = new BufferManager(this);
        }

        public void ResetBuffer()
        {
            BufferManager.ResetBuffer(ScreenSize);
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
            //get changes in buffer and consolidate them
            var changes = BufferManager.GetChanges();
            BufferManager.AcceptChanges();

            //call Terminal concrete implementation that sends changes to client
            this.SubmitChanges(changes);
        }

        public void ProcessKey(KeyInfo key)
        {
            KeyPressed?.Invoke(key);
        }

        public void SendCloseRequest()
        {
            Closed?.Invoke();
        }
 
    }

}