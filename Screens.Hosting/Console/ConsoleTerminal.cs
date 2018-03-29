using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Screens.Hosting
{
    internal class ConsoleTerminal : Terminal
    {
        public override void Beep()
        {
            Console.Beep();
        }

        public override void Clear()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
        }

        public override void HideCursor()
        {
            Console.CursorVisible = false;
        }
        
        public override void SetBackGroundColor(ConsoleColor back)
        {
            if (!BlackAndWhite) Console.BackgroundColor = back;
        }

        protected override void SetCursorPositionImpl()
        {
            Console.SetCursorPosition(CursorX, CursorY);
        }

        public override void SetForeGroundColor(ConsoleColor fore)
        {
            if (!BlackAndWhite) Console.ForegroundColor = fore;
        }

        public override void SetScreenSize(int width, int height)
        {
            ScreenSize = new Size(width, height);
            Console.SetWindowSize(width, height);
        }

        public override void ShowCursor()
        {
            Console.CursorVisible = true;
        }
               
        public override void Write(string s)
        {
            Console.Write(s);
        }

        public override void Write(string s, ConsoleColor fore, ConsoleColor back, int x, int y)
        {
            SetCursorPosition(x, y);
            SetForeGroundColor(fore);
            SetBackGroundColor(back);
            Write(s);
        }
    }
}
