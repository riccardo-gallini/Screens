using System;
using System.Collections.Generic;
using System.Linq;

namespace Screens
{

    public interface ITerminal
    {
        bool BlackAndWhite { get; set; }
        void Clear();
        void SetScreenSize(int width, int height);
        void SetCursorPosition(int x, int y);
        void SetBackGroundColor(ConsoleColor back);
        void SetForeGroundColor(ConsoleColor back);
        void HideCursor();
        void ShowCursor();

        void Write(string s);
        void Write(string s, ConsoleColor fore, ConsoleColor back, int x, int y);
        void Beep();
    }

}