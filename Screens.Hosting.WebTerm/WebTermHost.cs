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

            BuildWebHost().RunAsync();
        }
        
        public IWebHost BuildWebHost() =>
            WebHost.CreateDefaultBuilder()
                   .UseStartup<Startup>()
                   .Build();


        public void StopHost()
        {
        }


        internal void ClientConnected(string connectionId, IClientProxy client)
        {
            var sess = new WebTermSession(this, connectionId, client);
            _sessions.Add(sess.ConnectionId, sess);

            var e = new SessionEventArgs(sess);
            SessionConnected(this, e);

            if (!e.RefuseConnection)
            {
                sess.RunAsync();
            }
            else
            {
                sess.Kick();
            }

        }

        internal void ClientDisconnected(string connectionId)
        {
            var sess = _sessions[connectionId];
            sess.Terminal.SendCloseRequest();

            var e = new SessionEventArgs(sess);
            SessionDisconnected(this, e);

            _sessions.Remove(connectionId);

        }


        internal void ProcessKey(string connectionID, string key)
        {
            _sessions[connectionID].Terminal.ProcessKey(keyInfo(key));
        }

        private KeyInfo keyInfo(string browser_key)
        {
            var k = new KeyInfo();

            if (browser_key.Length==1)
            {
                k.KeyChar = browser_key[0];
                k.SpecialKey = SpecialKey.None;
            }
            else
            {
                k.KeyChar = ' ';
                k.SpecialKey = specialKey(browser_key);
            }
            return k;
        }

        private SpecialKey specialKey(string k)
        {
            switch (k)
            {
                case "ArrowDown": return SpecialKey.DownArrow;
                case "ArrowUp": return SpecialKey.UpArrow;
                case "ArrowRight": return SpecialKey.RightArrow;
                case "ArrowLeft": return SpecialKey.LeftArrow;
                case "PageDown": return SpecialKey.PageDown;
                case "PageUp": return SpecialKey.PageUp;
                case "Home": return SpecialKey.Home;
                case "End": return SpecialKey.End;
                case "Tab": return SpecialKey.Tab;
                case "Enter": return SpecialKey.Enter;
                case "Escape": return SpecialKey.Escape;
                case "Delete": return SpecialKey.Delete;
                case "Backspace": return SpecialKey.Backspace;
                //case ConsoleKey.F1: return SpecialKey.F1;
                //case ConsoleKey.F2: return SpecialKey.F2;
                //case ConsoleKey.F3: return SpecialKey.F3;
                //case ConsoleKey.F4: return SpecialKey.F4;
                //case ConsoleKey.F5: return SpecialKey.F5;
                //case ConsoleKey.F6: return SpecialKey.F6;
                //case ConsoleKey.F7: return SpecialKey.F7;
                //case ConsoleKey.F8: return SpecialKey.F8;
                //case ConsoleKey.F9: return SpecialKey.F9;
                //case ConsoleKey.F10: return SpecialKey.F10;
                //case ConsoleKey.F11: return SpecialKey.F11;
                //case ConsoleKey.F12: return SpecialKey.F12;
                default: return SpecialKey.None;




                    
            }
        }

    }
}
