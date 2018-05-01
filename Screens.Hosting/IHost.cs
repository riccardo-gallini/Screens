using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Screens.Hosting
{
    public interface IHost
    {
        event SessionConnectionEventHandler SessionConnected;
        event SessionConnectionEventHandler SessionDisconnected;
        
        IReadOnlyCollection<ISession> Sessions { get; }

        Action<Terminal> Main { get; set; }
        void StartHost();
        void StopHost();

        IPAddress ListeningOnAddress { get; }
        int ListeningOnPort { get; }

    }
}
