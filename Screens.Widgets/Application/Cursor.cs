using System;
using System.Collections.Generic;

namespace Screens
{
    public class Cursor
    {
        public static Cursor Current { get; set; }
    }

    public class Cursors
    {
        private static Cursor _default = new Cursor();
        public static Cursor Default
        {
            get
            {
                return _default;
            }
        }

        private static Cursor _waitCursor = new Cursor();
        public static Cursor WaitCursor
        {
            get
            {
                return _default;
            }
        }
    }

}