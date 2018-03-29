using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Screens.Hosting
{
    public class ConsoleHost
    {

        public Action<Terminal> Main;

        public void StartHost()
        {
            if (Main == null) throw new InvalidOperationException(" 'Main' was null!");

            var terminal = new ConsoleTerminal();
                     
            Task.Factory.StartNew(
                () =>
                {
                    while (true)
                    {
                        var win32_console_key = Console.ReadKey();
                        terminal.SendKey(keyInfo(win32_console_key));
                    }

                }
                
            );
                        
            Main(terminal);
        }

        private KeyInfo keyInfo(ConsoleKeyInfo win32_key)
        {
            var k = new KeyInfo();
            k.KeyChar = win32_key.KeyChar;
            k.SpecialKey = specialKey(win32_key.Key);

            return k;
        }

        private SpecialKey specialKey(ConsoleKey k)
        {
            switch (k)
            {
                case ConsoleKey.DownArrow:      return SpecialKey.DownArrow;
                case ConsoleKey.UpArrow:        return SpecialKey.UpArrow;
                case ConsoleKey.RightArrow:     return SpecialKey.RightArrow;
                case ConsoleKey.LeftArrow:      return SpecialKey.LeftArrow;
                case ConsoleKey.PageDown:       return SpecialKey.PageDown;
                case ConsoleKey.PageUp:         return SpecialKey.PageUp;
                case ConsoleKey.Home:           return SpecialKey.Home;
                case ConsoleKey.End:            return SpecialKey.End;
                case ConsoleKey.Tab:            return SpecialKey.Tab;
                case ConsoleKey.Enter:          return SpecialKey.Enter;
                case ConsoleKey.Escape:         return SpecialKey.Escape;
                case ConsoleKey.Delete:         return SpecialKey.Delete;
                case ConsoleKey.Backspace:      return SpecialKey.Backspace;
                case ConsoleKey.F1:             return SpecialKey.F1;
                case ConsoleKey.F2:             return SpecialKey.F2;
                case ConsoleKey.F3:             return SpecialKey.F3;
                case ConsoleKey.F4:             return SpecialKey.F4;
                case ConsoleKey.F5:             return SpecialKey.F5;
                case ConsoleKey.F6:             return SpecialKey.F6;
                case ConsoleKey.F7:             return SpecialKey.F7;
                case ConsoleKey.F8:             return SpecialKey.F8;
                case ConsoleKey.F9:             return SpecialKey.F9;
                case ConsoleKey.F10:            return SpecialKey.F10;
                case ConsoleKey.F11:            return SpecialKey.F11;
                case ConsoleKey.F12:            return SpecialKey.F12;
                default:                        return SpecialKey.None;
            }
        }

    }
}
