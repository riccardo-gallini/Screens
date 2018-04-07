﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Screens.Hosting.WebTerm
{
    public class WebTermHost : IHost
    {
        public delegate void SessionConnectedEventHandler(WebTermHost h, SessionEventArgs e);
        public event SessionConnectedEventHandler SessionConnected = null;

        public delegate void SessionDisconnectedEventHandler(WebTermHost h, SessionEventArgs e);
        public event SessionDisconnectedEventHandler SessionDisconnected = null;

        private Dictionary<string, TelnetSession> _sessions = new Dictionary<string, TelnetSession>();

        public IReadOnlyCollection<TelnetSession> Sessions
        {
            get
            {
                return new ReadOnlyCollection<TelnetSession>(_sessions.Values.ToList());
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

        public void StopHost()
        {
            throw new NotImplementedException();
        }
        
        public IWebHost BuildWebHost() =>
            WebHost.CreateDefaultBuilder()
                   .UseStartup<Startup>()
                   .Build();


        internal void _clientConnected(string connectionID)
        {
            var sess = new TelnetSession(this, connectionID);
            _sessions.Add(connectionID, sess);

            var e = new SessionEventArgs(sess);
            SessionConnected(this, e);

            if (!e.RefuseConnection)
            {

                var term = new WebTerminal(sess);
                sess.Terminal = term;

                Task.Factory.StartNew(() => Main(term));

            }
            else
            {
                sess.Kick();
            }

        }

        internal void _clientDisconnected(string c)
        {
            var sess = _sessions[c];
            sess.Terminal.SendCloseRequest();

            var e = new SessionEventArgs(sess);
            SessionDisconnected(this, e);

            _sessions.Remove(c);
        }

        internal void SendToClient(string connectionID, string message)
        {
            //byte[] data = Encoding.ASCII.GetBytes(message);
            //this.Server.Send(conn, data);
        }

        private void _messageReceived(string connectionID, byte[] data)
        {
            _sessions[connectionID].DataFromClient(data);
        }

    }
}
