using Screens;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Screens.Hosting
{
    public class TelnetTerminal : Terminal
    {
        private ANSI_Decoder decoder;
        public Session Session { get; internal set; }
        
        internal TelnetTerminal()
        {
            decoder = new ANSI_Decoder();
            decoder.KeyReady = (key) => this.SendKey(key);
        }

        private void SendToClient(string msg)
        {
            Session.SendToClient(msg);
        }

        public void ProcessData(byte[] data)
        {
            decoder.Decode(data);
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
        
        public override void Write(string s)
        {
            SendToClient(s);
        }

        public override void Write(string s, ConsoleColor fore, ConsoleColor back, int x, int y)
        {
            SetForeGroundColor(fore);
            SetBackGroundColor(back);
            SetCursorPosition(x, y);
            SendToClient(s);
        }


    }
}
