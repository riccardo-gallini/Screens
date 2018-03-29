using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Screens.Hosting
{
    public class Session
    {
        public TelnetHost Host { get; }
        public NetworkConnection Connection { get; }
        public TelnetTerminal Terminal { get; internal set; }

        internal Session(TelnetHost h, NetworkConnection c)
        {
            Host = h;
            Connection = c;
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
