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
        public string ConnectionId
        {
            get
            {
                return Connection.ConnectionId;
            }
        }

        public Terminal Terminal { get; internal set; }

        public IPAddress RemoteAddress => Connection.RemoteIpAddress;
        public int? RemotePort => Connection.RemotePort;
        public DateTime ConnectionTime { get; }
        
        public IClientProxy Client { get; }
        public HubConnectionContext Connection { get; }
        

        private WebTermHost webTermHost;
        private WebTerminal webTerminal;


        internal WebTermSession(WebTermHost h, HubConnectionContext c, IClientProxy client)
        {
            ConnectionTime = DateTime.Now;
            webTermHost = h;
            Connection = c;
            Client = client;
        }

        async internal Task RunAsync()
        {
            var term = new WebTerminal(this);
            this.webTerminal = term;

            await Task.Run(() => Host.Main(term));
        }

        public void Kick()
        {

        }

    }
}
