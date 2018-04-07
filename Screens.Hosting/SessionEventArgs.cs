using System;
using System.Collections.Generic;
using System.Text;

namespace Screens.Hosting
{
    public class SessionEventArgs : EventArgs
    {
        public bool RefuseConnection { get; set; } = false;
        public ISession Session { get; }

        internal SessionEventArgs(ISession session)
        {
            Session = session;
        }
    }
}
