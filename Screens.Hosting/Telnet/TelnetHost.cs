using Screens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Screens.Hosting.Telnet
{

    public class TelnetHost : IHost
    {
        private TcpListener server;
        public IPAddress ListeningOnAddress { get; }
        public int ListeningOnPort { get; }

        public Action<Terminal> Main { get; set; }
        
        public event SessionConnectionEventHandler SessionConnected = null;
        public event SessionConnectionEventHandler SessionDisconnected = null;

        private Dictionary<string, TelnetSession> _sessions = new Dictionary<string, TelnetSession>();
        
        public IReadOnlyCollection<ISession> Sessions
        {
            get
            {
                return new ReadOnlyCollection<TelnetSession>(_sessions.Values.ToList());
            }
        }

        public TelnetHost(IPAddress IP, int port = 23)
        {
            this.ListeningOnAddress = IP;
            this.ListeningOnPort = port;

            this.server = new TcpListener(IP, port);
        }

        public TelnetHost() : this(IPAddress.Any) {}

        public void StopHost()
        {
            server.Stop();
        }


        internal void ClientDisconnected(TelnetSession session)
        {
            var e = new SessionEventArgs(session);
            SessionDisconnected(this, e);

            _sessions.Remove(session.ConnectionId);
        }

        public async void StartHost()
        {
            if (Main == null) throw new InvalidOperationException(" 'Main' was null!");

            server.Start();

            while(true)
            {
                var socket = await server.AcceptSocketAsync();

                var session = new TelnetSession(this, socket);
                _sessions.Add(session.ConnectionId, session);

                var e = new SessionEventArgs(session);  //todo: this client event can block the whole server
                SessionConnected?.Invoke(this, e);

                if (!e.RefuseConnection)
                {
                    session.Connect();
                }
                else
                {
                    session.Kick();
                }
            }

        }

    }
}
