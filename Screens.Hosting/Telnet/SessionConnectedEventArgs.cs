using System;
using System.Collections.Generic;
using System.Text;

namespace Screens.Hosting
{
    public class SessionConnectedEventArgs : EventArgs
    {
        public bool Refuse { get; set; } = false;
        public Session Session { get; }

        internal SessionConnectedEventArgs(Session sess)
        {
            Session = sess;
        }
    }
}
