using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;

namespace Screens
    { 

public enum SpecialKey
{
    None,
    DownArrow,
    UpArrow,
    RightArrow,
    LeftArrow,
    PageDown,
    PageUp,
    Home,
    End,
    Tab,
    Enter,
    Escape,
    Delete,
    Backspace,
    F1,
    F2,
    F3,
    F4,
    F5,
    F6,
    F7,
    F8,
    F9,
    F10,
    F11,
    F12
}


public class KeyInfo
{
    public char KeyChar { get; set; }
    public SpecialKey SpecialKey { get; set; }

    public static KeyInfo Make(SpecialKey spc)
    {
        var key_info = new KeyInfo();
        key_info.SpecialKey = spc;
        return key_info;
    }

    public static KeyInfo Make(char ch)
    {
        var key_info = new KeyInfo();
        key_info.KeyChar = ch;
        key_info.SpecialKey = SpecialKey.None;
        return key_info;
    }
}

}