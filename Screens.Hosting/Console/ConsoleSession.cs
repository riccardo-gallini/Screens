using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Screens.Hosting.LocalConsole
{
    public class ConsoleSession : ISession
    {
        public string ConnectionId => Guid.Empty.ToString();
        public IPAddress RemoteAddress => null;
        public int? RemotePort => null;
        public DateTime ConnectionTime { get; }
        public IHost Host { get; }
        public Terminal Terminal {get;}

        public ConsoleSession(ConsoleHost h, ConsoleTerminal term)
        {
            Host = h;
            Terminal = term;
        }

        public void Kick()
        {
            Terminal.SendCloseRequest();
        }
    }
}
