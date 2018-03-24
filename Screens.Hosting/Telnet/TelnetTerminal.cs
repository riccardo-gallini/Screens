using Screens;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Screens.Hosting
{
    public class TelnetTerminal : ITerminal
    {
        private ANSI_Decoder decoder;
        public Session Session { get; internal set; }
        public Application Application { get; internal set; }
        public bool BlackAndWhite { get; set; }

        internal TelnetTerminal()
        {
            decoder = new ANSI_Decoder();
            decoder.KeyReady = (key) => Application?.SendKey(key);
        }
        

        private void SendToClient(string msg)
        {
            Session.SendToClient(msg);
        }

        public void ProcessData(byte[] data)
        {
            decoder.Decode(data);
        }



        public void Beep() 
        {
            SendToClient(ANSI_Encoder.Beep());
        }

        public void Clear()
        {
            SendToClient(ANSI_Encoder.Erase_Screen());
        }

        public void HideCursor()
        {
            //
        }

        public void SetBackGroundColor(ConsoleColor back)
        {
            if (!BlackAndWhite) SendToClient(ANSI_Encoder.Set_Attribute_Mode(ANSI_Encoder.From_BackColor(back)));
        }

        public void SetCursorPosition(int x, int y)
        {
            SendToClient(ANSI_Encoder.Cursor_Home(y, x));
        }

        public void SetForeGroundColor(ConsoleColor fore)
        {
            if (!BlackAndWhite) SendToClient(ANSI_Encoder.Set_Attribute_Mode(ANSI_Encoder.From_ForeColor(fore)));
        }

        public void SetScreenSize(int width, int height)
        {
            //
        }

        public void ShowCursor()
        {
            //
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
