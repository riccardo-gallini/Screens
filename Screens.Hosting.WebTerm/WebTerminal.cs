using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Screens.Hosting.WebTerm
{
    public class WebTerminal : Terminal
    {
        public WebTermSession Session { get; }

        //TODO: understand if SetBackColor / SetForeColor is needed on the Terminal interface // used for simple term (no app)

        public WebTerminal(WebTermSession sess)
        {
            Session = sess;
        }

        public override void Beep()
        {
            Session.Client.SendAsync("Beep");
        }

        public override void Clear()
        {
            Session.Client.SendAsync("Clear");
        }

        public override void HideCursor()
        {
            Session.Client.SendAsync("HideCursor");
        }

        public override void SetScreenSize(int width, int height)
        {
            Session.Client.SendAsync("SetScreenSize", width, height);
        }

        public override void ShowCursor()
        {
            Session.Client.SendAsync("ShowCursor");
        }

        public override void SubmitChanges(TerminalChanges changes)
        {
            Session.Client.SendAsync("SubmitChanges", changes);
        }

        protected override void SetCursorPositionImpl()
        {
            Session.Client.SendAsync("SetCursorPosition", this.CursorX, this.CursorY);
        }
    }
}
