using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            Session.Client.SendAsync("Beep", null);
        }

        public override void Clear()
        {
            Session.Client.SendAsync("Clear", null);
        }

        public override void HideCursor()
        {
            Session.Client.SendAsync("HideCursor", null);
        }

        public override void SetScreenSize(int width, int height)
        {
            Session.Client.SendAsync("SetScreenSize", new object[] { width, height});
        }

        public override void ShowCursor()
        {
            Session.Client.SendAsync("ShowCursor", null);
        }

        public override void SubmitChanges(TerminalChanges changes)
        {
            Session.Client.SendAsync("SubmitChanges", new object[] { changes });
        }

        protected override void SetCursorPositionImpl()
        {
            Session.Client.SendAsync("SubmitChanges", new object[] { this.CursorX, this.CursorY });
        }
    }
}
