using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using System.Drawing;

namespace Screens
{




    public enum WM_MessageType
    {
        WM_PAINT = 1,
        WM_TIMER = 2,
        WM_KEY = 3,
        WM_SHOW_FORM = 4,
        WM_RESIZE = 5,
        WM_EXCEPTION = 6,
        WM_APP_MSG = 7,
        WM_QUIT = 8
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



        public static Message WM_PAINT()
        {
            return new Message(WM_MessageType.WM_PAINT);
        }
        public static Message WM_TIMER(Timer timer)
        {
            return new Message(WM_MessageType.WM_TIMER, timer);
        }
        public static Message WM_KEY(KeyInfo key_info)
        {
            return new Message(WM_MessageType.WM_KEY, key_info);
        }
        public static Message WM_SHOW_FORM(Form form)
        {
            return new Message(WM_MessageType.WM_SHOW_FORM, form);
        }
        public static Message WM_RESIZE(Size size)
        {
            return new Message(WM_MessageType.WM_RESIZE, size);
        }
        public static Message WM_EXCEPTION(Exception ex)
        {
            return new Message(WM_MessageType.WM_EXCEPTION, ex);
        }
        public static Message WM_APP_MSG(object data)
        {
            return new Message(WM_MessageType.WM_APP_MSG, data);
        }
        public static Message WM_QUIT()
        {
            return new Message(WM_MessageType.WM_QUIT);
        }
    }

}