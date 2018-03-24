using System;
using System.Collections.Generic;
using System.Text;

namespace Screens.Hosting
{
    public static class ANSI_Encoder
    {
        const char ESC = '\x1B';
        
        public static string Query_Device_Code() => ESC + "[c";
        public static string Report_Device_Code(string code) => ESC + string.Format("[{0}0c", code);
        public static string Query_Device_Status() => ESC + "[5n";
        public static string Report_Device_OK() => ESC + "[0n";
        public static string Report_Device_Failure() => ESC + "[3n";
        public static string Query_Cursor_Position() => ESC + "[6n";
        public static string Report_Cursor_Position(int row, int column) => ESC + string.Format("[{0};{1}R", row+1, column+1);
        public static string Reset_Device() => ESC + "c";
        public static string Enable_Line_Wrap() => ESC + "[7h";
        public static string Disable_Line_Wrap() => ESC + "[7l";
        public static string Font_Set_G0() => ESC + "(";
        public static string Font_Set_G1() => ESC + ")";
        public static string Cursor_Home() => ESC + "[H";
        public static string Cursor_Home(int row, int column) => ESC + string.Format("[{0};{1}H", row, column);
        public static string Cursor_Up(int count) => ESC + string.Format("[{0}A", count);
        public static string Cursor_Down(int count) => ESC + string.Format("[{0}B", count);
        public static string Cursor_Forward(int count) => ESC + string.Format("[{0}C", count);
        public static string Cursor_Backward(int count) => ESC + string.Format("[{0}D", count);
        public static string Force_Cursor_Position(int row, int column) => ESC + string.Format("[{0};{1}f", row, column);
        public static string Save_Cursor() => ESC + "[s";
        public static string Unsave_Cursor() => ESC + "[u";
        public static string Save_Cursor_And_Attrs() => ESC + "7";
        public static string Restore_Cursor_And_Attrs() => ESC + "8";
        public static string Scroll_Screen() => ESC + "[r";
        public static string Scroll_Screen(int start, int end) => ESC + string.Format("[{0};{1}r", start, end);
        public static string Scroll_Down() => ESC + "D";
        public static string Scroll_Up() => ESC + "M";
        public static string Set_Tab() => ESC + "H";
        public static string Clear_Tab() => ESC + "[g";
        public static string Clear_All_Tabs() => ESC + "[3g";
        public static string Erase_End_of_Line() => ESC + "[K";
        public static string Erase_Start_of_Line() => ESC + "[1K";
        public static string Erase_Line() => ESC + "[2K";
        public static string Erase_Down() => ESC + "[J";
        public static string Erase_Up() => ESC + "[1J";
        public static string Erase_Screen() => ESC + "[2J";
        public static string Print_Screen() => ESC + "[i";
        public static string Print_Line() => ESC + "[1i";
        public static string Stop_Print_Log() => ESC + "[4i";
        public static string Start_Print_Log() => ESC + "[5i";
        public static string Set_Key_Definition(string key, string str) => ESC + string.Format("[{0};\"{1}\"p", key, str);
        public static string Set_Attribute_Mode(params ANSI_Attr[] attrs) => ESC + "[" + string.Join(";", Array.ConvertAll(attrs, value => (int)value)) + "m";
        public static string Hide_Cursor() => ESC + "[" + "? 25 l";

        public static string Beep() => "\x07";

        public static ANSI_Attr From_ForeColor(ConsoleColor color)
        {
            switch(color)
            {
                case ConsoleColor.Black:            return ANSI_Attr.BLACK_FG;
                case ConsoleColor.DarkBlue:         return ANSI_Attr.BLUE_FG;
                case ConsoleColor.DarkGreen:        return ANSI_Attr.GREEN_FG;
                case ConsoleColor.DarkCyan:         return ANSI_Attr.CYAN_FG;
                case ConsoleColor.DarkRed:          return ANSI_Attr.RED_FG;
                case ConsoleColor.DarkMagenta:      return ANSI_Attr.MAGENTA_FG;
                case ConsoleColor.DarkYellow:       return ANSI_Attr.YELLOW_FG;
                case ConsoleColor.Gray:             return ANSI_Attr.DARK_GRAY_FG;
                case ConsoleColor.DarkGray:         return ANSI_Attr.DARK_GRAY_FG;
                case ConsoleColor.Blue:             return ANSI_Attr.LIGHT_BLUE_FG;
                case ConsoleColor.Green:            return ANSI_Attr.LIGHT_GREEN_FG;
                case ConsoleColor.Cyan:             return ANSI_Attr.LIGHT_CYAN_FG;
                case ConsoleColor.Red:              return ANSI_Attr.LIGHT_RED_FG;
                case ConsoleColor.Magenta:          return ANSI_Attr.LIGHT_MAGENTA_FG;
                case ConsoleColor.Yellow:           return ANSI_Attr.LIGHT_YELLOW_FG;
                case ConsoleColor.White:            return ANSI_Attr.WHITE_FG;
                default:                            return ANSI_Attr.DEFAULT_FG;
            }
        }

        public static ANSI_Attr From_BackColor(ConsoleColor color)
        {
            switch (color)
            {
                case ConsoleColor.Black:            return ANSI_Attr.BLACK_BG;
                case ConsoleColor.DarkBlue:         return ANSI_Attr.BLUE_BG;
                case ConsoleColor.DarkGreen:        return ANSI_Attr.GREEN_BG;
                case ConsoleColor.DarkCyan:         return ANSI_Attr.CYAN_BG;
                case ConsoleColor.DarkRed:          return ANSI_Attr.RED_BG;
                case ConsoleColor.DarkMagenta:      return ANSI_Attr.MAGENTA_BG;
                case ConsoleColor.DarkYellow:       return ANSI_Attr.YELLOW_BG;
                case ConsoleColor.Gray:             return ANSI_Attr.DARK_GRAY_BG;
                case ConsoleColor.DarkGray:         return ANSI_Attr.DARK_GRAY_BG;
                case ConsoleColor.Blue:             return ANSI_Attr.LIGHT_BLUE_BG;
                case ConsoleColor.Green:            return ANSI_Attr.LIGHT_GREEN_BG;
                case ConsoleColor.Cyan:             return ANSI_Attr.LIGHT_CYAN_BG;
                case ConsoleColor.Red:              return ANSI_Attr.LIGHT_RED_BG;
                case ConsoleColor.Magenta:          return ANSI_Attr.LIGHT_MAGENTA_BG;
                case ConsoleColor.Yellow:           return ANSI_Attr.LIGHT_YELLOW_BG;
                case ConsoleColor.White:            return ANSI_Attr.WHITE_BG;
                default:                            return ANSI_Attr.DEFAULT_BG;
            }
        }

        public enum ANSI_Attr
        {
            RESET_ALL_ATTRIBUTES = 0,
            BRIGHT = 1,
            DIM = 2,
            UNDERSCORE = 4,
            BLINK = 5,
            REVERSE = 7,
            HIDDEN = 8,

            DEFAULT_FG = 39,
            BLACK_FG = 30,
            RED_FG = 31,
            GREEN_FG = 32,
            YELLOW_FG = 33,
            BLUE_FG = 34,
            MAGENTA_FG = 35,
            CYAN_FG = 36,
            WHITE_FG = 37,
            DARK_GRAY_FG = 90,
            LIGHT_RED_FG = 91,
            LIGHT_GREEN_FG = 92,
            LIGHT_YELLOW_FG = 93,
            LIGHT_BLUE_FG = 94,
            LIGHT_MAGENTA_FG = 95,
            LIGHT_CYAN_FG = 96,
                        
            DEFAULT_BG = 49,
            BLACK_BG = 40,
            RED_BG = 41,
            GREEN_BG = 42,
            YELLOW_BG = 43,
            BLUE_BG = 44,
            MAGENTA_BG = 45,
            CYAN_BG = 46,
            WHITE_BG = 47,
            DARK_GRAY_BG = 100,
            LIGHT_RED_BG = 101,
            LIGHT_GREEN_BG = 102,
            LIGHT_YELLOW_BG = 103,
            LIGHT_BLUE_BG = 104,
            LIGHT_MAGENTA_BG = 105,
            LIGHT_CYAN_BG = 106
        }

    }
}
