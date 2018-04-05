using System;
using System.Collections.Generic;
using System.Text;

namespace Screens.Hosting.WebTerm
{
    public class Session
    {
        public WebTermHost Host { get; }
        public string ConnectionID { get; }
        public WebTerminal Terminal { get; internal set; }

        internal Session(WebTermHost h, string c)
        {
            Host = h;
            ConnectionID = c;
        }

        internal void SendToClient(string msg)
        {
            Host.SendToClient(Connection, msg);
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
