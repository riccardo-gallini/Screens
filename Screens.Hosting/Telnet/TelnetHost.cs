using Screens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Screens.Hosting
{

    public class TelnetHost
    {
        public NetworkServer Server { get; }

        private Dictionary<int, Session> _sessions = new Dictionary<int, Session>();
        private Action<Application> _runner;

        public IReadOnlyCollection<Session> Sessions
        {
            get
            {
                return new ReadOnlyCollection<Session>(_sessions.Values.ToList());
            }
        }

        public TelnetHost(IPAddress IP, int port = 23)
        {
            Server = new NetworkServer(IP, port);
            Server.OnClientConnected = _clientConnected;
            Server.OnClientDisconnected = _clientDisconnected;
            Server.OnMessageReceived = _messageReceived;
        }

        public TelnetHost() : this(IPAddress.Any) {}

        public delegate void SessionConnectedEventHandler(TelnetHost h, SessionConnectedEventArgs e);
        public event SessionConnectedEventHandler SessionConnected = null;


        public void Run(Action<Application> runner)
        {
            _runner = runner;
        }

        public void Stop()
        {
            Server.Stop();
        }


        private void _clientConnected(ClientConnection c)
        {
            var sess = new Session(this, c);
            _sessions.Add(c.Id, sess);

            var e = new SessionConnectedEventArgs(sess);
            SessionConnected(this, e);

            if (!e.Refuse)
            {
                
                var term = new TelnetTerminal();
                var app = new Application(term);
                sess.Terminal = term;
                term.Session = sess;
                term.Application = app;
                sess.Application = app;

                Task.Factory.StartNew(() => _runner(app)); 

            }
            else
            {
                sess.Kick();

            }
            
        }

        internal void SendToClient(ClientConnection conn, string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            this.Server.Send(conn, data);
        }

        private void _messageReceived(ClientConnection c, byte[] data)
        {
            _sessions[c.Id].DataFromClient(data);          
        }

        private void _clientDisconnected(ClientConnection c)
        {
            _sessions[c.Id].Close();
            _sessions.Remove(c.Id);
        }

        public void StartHost()
        {
            Server.Start();
        }


        public void SendCustomMessage_All(object data)
        {
            foreach (var sess in _sessions.Values)
            {
                sess.SendCustomMessage(data);
            }
        }

        public void Kick_All()
        {
            foreach (var sess in _sessions.Values.ToList())  //make a copy??
            {
                sess.Kick();
            }
        }

    }
}
