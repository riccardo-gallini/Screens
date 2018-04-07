using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Screens.Hosting.WebTerm
{
    public class WebTermSession : ISession
    {
        public WebTermHost Host { get; }
        public string ConnectionId { get; }
        public WebTerminal Terminal { get; internal set; }




        internal WebTermSession(WebTermHost h, string c)
        {
            Host = h;
            ConnectionId = c;
        }

        internal void SendToClient(string msg)
        {
            Host.SendToClient(ConnectionId, msg);
        }

        internal void DataFromClient(byte[] data)
        {
            Terminal.ProcessData(data);
        }

        public int Id
        {
            get
            {
                return Connection.Id;
            }
        }

        //TODO: removed CustomAppMessage

        public void Kick()
        {
            Host.Server.KickClient(Connection);
        }

    }
}
