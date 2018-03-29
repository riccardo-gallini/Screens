using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;

namespace Screens
{

    public class KeyPressEventArgs : EventArgs
    {
        public bool Handled { get; set; } = false;

        public char KeyChar
        {
            get
            {
                return KeyInfo.KeyChar;
            }
        }

        public SpecialKey SpecialKey
        {
            get
            {
                return KeyInfo.SpecialKey;
            }
        }

        public KeyInfo KeyInfo { get; }

        public KeyPressEventArgs(KeyInfo info)
        {
            KeyInfo = info;
        }
    }

}