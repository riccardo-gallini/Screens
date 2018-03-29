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

        public Action<Terminal> Main;
                
        public delegate void SessionConnectedEventHandler(TelnetHost h, SessionEventArgs e);
        public event SessionConnectedEventHandler SessionConnected = null;

        public delegate void SessionDisconnectedEventHandler(TelnetHost h, SessionEventArgs e);
        public event SessionDisconnectedEventHandler SessionDisconnected = null;

        private Dictionary<int, Session> _sessions = new Dictionary<int, Session>();
        
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

        public void Stop()
        {
            Server.Stop();
        }


        private void _clientConnected(NetworkConnection c)
        {
            var sess = new Session(this, c);
            _sessions.Add(c.Id, sess);

            var e = new SessionEventArgs(sess);
            SessionConnected(this, e);

            if (!e.RefuseConnection)
            {
                
                var term = new TelnetTerminal(sess);
                sess.Terminal = term;
                
                Task.Factory.StartNew(() => Main(term)); 

            }
            else
            {
                sess.Kick();
            }
            
        }

        internal void SendToClient(NetworkConnection conn, string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            this.Server.Send(conn, data);
        }

        private void _messageReceived(NetworkConnection c, byte[] data)
        {
            _sessions[c.Id].DataFromClient(data);          
        }

        private void _clientDisconnected(NetworkConnection c)
        {
            var sess = _sessions[c.Id];
            sess.Terminal.Close();

            var e = new SessionEventArgs(sess);
            SessionDisconnected(this, e);

            _sessions.Remove(c.Id);
        }

        public void StartHost()
        {
            if (Main == null) throw new InvalidOperationException(" 'Main' was null!");

            Server.Start();
        }

        public void KickAll()
        {
            foreach (var sess in _sessions.Values.ToList())  //make a copy??
            {
                sess.Kick();
            }
        }

    }
}
