using Screens;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Screens.Hosting
{
    public class TelnetTerminal : Terminal
    {
        public ANSI_Decoder ANSI_Decoder { get; }
        public Session Session { get; }
        
        internal TelnetTerminal(Session session)
        {
            Session = session;

            ANSI_Decoder = new ANSI_Decoder();
            ANSI_Decoder.KeyReady = (key) => this.SendKey(key);
        }

        private void SendToClient(string msg)
        {
            Session.SendToClient(msg);
        }

        internal void ProcessData(byte[] data)
        {
            ANSI_Decoder.Decode(data);
        }
        
        public override void Beep() 
        {
            SendToClient(ANSI_Encoder.Beep());
        }

        public override void Clear()
        {
            SendToClient(ANSI_Encoder.Erase_Screen());
        }

        public override void HideCursor()
        {
            //
        }

        public override void SetBackGroundColor(ConsoleColor back)
        {
            if (!BlackAndWhite) SendToClient(ANSI_Encoder.Set_Attribute_Mode(ANSI_Encoder.From_BackColor(back)));
        }

        protected override void SetCursorPositionImpl()
        {
            SendToClient(ANSI_Encoder.Cursor_Home(CursorY, CursorX));
        }

        public override void SetForeGroundColor(ConsoleColor fore)
        {
            if (!BlackAndWhite) SendToClient(ANSI_Encoder.Set_Attribute_Mode(ANSI_Encoder.From_ForeColor(fore)));
        }
                
        public override void SetScreenSize(int width, int height)
        {
            ScreenSize = new Size(width, height);

            //
        }

        public override void ShowCursor()
        {
            //
        }

        public override void SubmitChanges(TerminalChanges changes)
        {
            foreach(var line in changes.Lines)
                foreach(var span in line.Spans)
                    Write(span.Text, span.ForeColor, span.BackColor, span.X, line.Y);
        }

        public void Write(string s)
        {
            SendToClient(s);
        }

        public void Write(string s, ConsoleColor fore, ConsoleColor back, int x, int y)
        {
            SetForeGroundColor(fore);
            SetBackGroundColor(back);
            SetCursorPosition(x, y);
            SendToClient(s);
        }


    }
}
