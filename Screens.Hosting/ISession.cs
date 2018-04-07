using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Screens.Hosting
{
    public interface ISession
    {
        string ConnectionId { get; }
        IPAddress RemoteAddress { get; }
        int RemotePort { get; }
        DateTime ConnectionTime { get; }

        IHost Host { get; }
        Terminal Terminal { get; }


        void Kick();
    }
}
