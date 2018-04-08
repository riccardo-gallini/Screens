using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Screens.Hosting.WebTerm
{

    public class WebTermHost : IHost
    {
        
        public event SessionConnectionEventHandler SessionConnected = null;
        public event SessionConnectionEventHandler SessionDisconnected = null;

        public IPAddress ListeningOnAddress => throw new NotImplementedException();
        public int ListeningOnPort => throw new NotImplementedException();

        private Dictionary<string, WebTermSession> _sessions = new Dictionary<string, WebTermSession>();

        public IReadOnlyCollection<ISession> Sessions
        {
            get
            {
                return new ReadOnlyCollection<WebTermSession>(_sessions.Values.ToList());
            }
        }


        public Action<Terminal> Main { get; set; }
                

        public static readonly WebTermHost Instance = new WebTermHost();
        private WebTermHost() {}

        public void StartHost()
        {
            if (Main == null) throw new InvalidOperationException(" 'Main' was null!");

            BuildWebHost().Run();
        }
        
        public IWebHost BuildWebHost() =>
            WebHost.CreateDefaultBuilder()
                   .UseStartup<Startup>()
                   .Build();


        public void StopHost()
        {
        }


        internal async Task ClientConnectedAsync(HubConnectionContext connection, IClientProxy client)
        {
            var sess = new WebTermSession(this, connection, client);
            _sessions.Add(sess.ConnectionId, sess);

            var e = new SessionEventArgs(sess);
            SessionConnected(this, e);

            if (!e.RefuseConnection)
            {
                await sess.RunAsync();
            }
            else
            {
                sess.Kick();
            }

        }

        internal void ClientDisconnectedAsync(HubConnectionContext connection)
        {
            var sess = _sessions[connection.ConnectionId];
            sess.Terminal.SendCloseRequest();

            var e = new SessionEventArgs(sess);
            SessionDisconnected(this, e);

            _sessions.Remove(connection.ConnectionId);

        }


        internal void ProcessKey(string connectionID, KeyInfo key)
        {
            _sessions[connectionID].Terminal.ProcessKey(key);
        }

    }
}
