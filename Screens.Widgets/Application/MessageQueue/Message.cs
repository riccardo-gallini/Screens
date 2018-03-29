using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using System.Drawing;

namespace Screens
{
    
    public enum WM_MessageType
    {
        WM_TIMER,
        WM_KEY,
        WM_SHOW_FORM,
        WM_RESIZE,
        WM_QUIT
    }
    
    public class Message
    {
        public WM_MessageType MessageType { get; }
        public object Parameter { get; }

        private Message(WM_MessageType message_type)
        {
            MessageType = message_type;
        }

        public Message(WM_MessageType message_type, object par)
        {
            MessageType = message_type;
            Parameter = par;
        }
               
        public static Message WM_TIMER(Timer timer) => new Message(WM_MessageType.WM_TIMER, timer);
        public static Message WM_KEY(KeyInfo key_info) => new Message(WM_MessageType.WM_KEY, key_info);
        public static Message WM_SHOW_FORM(Form form) => new Message(WM_MessageType.WM_SHOW_FORM, form);
        public static Message WM_RESIZE(Size size) => new Message(WM_MessageType.WM_RESIZE, size);
        public static Message WM_EXCEPTION(Exception ex) => new Message(WM_MessageType.WM_EXCEPTION, ex);
        public static Message WM_QUIT() => new Message(WM_MessageType.WM_QUIT);
    }

}