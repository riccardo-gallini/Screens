using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Screens.Hosting.WebTerm
{
    public class WebTermSession : ISession
    {
        public IHost Host
        {
            get
            {
                return webTermHost;
            }
        }

        public Terminal Terminal
        {
            get
            {
                return webTerminal;
            }
        }

        public IPAddress RemoteAddress => null;
        public int? RemotePort => null;
        public DateTime ConnectionTime { get; }
        
        public IClientProxy Client { get; }
        public string ConnectionId { get; }
        

        private WebTermHost webTermHost;
        private WebTerminal webTerminal;


        internal WebTermSession(WebTermHost h, string c, IClientProxy client)
        {
            ConnectionTime = DateTime.Now;
            webTermHost = h;
            ConnectionId = c;
            Client = client;
        }

        internal void RunAsync()
        {
            var term = new WebTerminal(this);
            this.webTerminal = term;

            var t = Task.Run(() => Host.Main(term));

        }

        public void Kick()
        {

        }

    }
}
