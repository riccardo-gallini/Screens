using Screens;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Screens.Hosting.Telnet
{
    public class TelnetTerminal : Terminal
    {
        public ANSI_Decoder ANSI_Decoder { get; }
        public TelnetSession Session { get; }
        
        internal TelnetTerminal(TelnetSession session)
        {
            Session = session;

            ANSI_Decoder = new ANSI_Decoder();
            ANSI_Decoder.KeyReady = (key) => this.ProcessKey(key);
        }

        private void SendToClient(string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            Session.Send(data);
        }

        internal void ProcessRawData(byte[] data)
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

        public void SetBackGroundColor(ConsoleColor back)
        {
            if (!BlackAndWhite) SendToClient(ANSI_Encoder.Set_Attribute_Mode(ANSI_Encoder.From_BackColor(back)));
        }

        protected override void SetCursorPositionImpl()
        {
            SendToClient(ANSI_Encoder.Cursor_Home(CursorY, CursorX));
        }

        public void SetForeGroundColor(ConsoleColor fore)
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
