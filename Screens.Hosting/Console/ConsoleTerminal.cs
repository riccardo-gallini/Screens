using System;
using System.Collections.Generic;
using System.Text;

namespace Screens.Hosting
{
    internal class ConsoleTerminal : ITerminal
    {
        public bool BlackAndWhite { get; set; } = false;

        public void Beep()
        {
            Console.Beep();
        }

        public void Clear()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
        }

        public void HideCursor()
        {
            Console.CursorVisible = false;
        }

        public void ResetBuffer()
        {
            throw new NotImplementedException();
        }

        public void SendBuffer(Buffer current_buffer)
        {
            throw new NotImplementedException();
        }

        public void SetBackGroundColor(ConsoleColor back)
        {
            if (!BlackAndWhite) Console.BackgroundColor = back;
        }

        public void SetCursorPosition(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }

        public void SetForeGroundColor(ConsoleColor fore)
        {
            if (!BlackAndWhite) Console.ForegroundColor = fore;
        }

        public void SetScreenSize(int width, int height)
        {
            Console.SetWindowSize(width, height);
        }

        public void ShowCursor()
        {
            Console.CursorVisible = true;
        }
               
        public void Write(string s)
        {
            Console.Write(s);
        }

        public void Write(string s, ConsoleColor fore, ConsoleColor back, int x, int y)
        {
            SetCursorPosition(x, y);
            SetForeGroundColor(fore);
            SetBackGroundColor(back);
            Write(s);
        }
    }
}
