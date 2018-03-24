using System;
using System.Collections.Generic;
using System.Text;

namespace Screens.Hosting
{
    public class Session
    {
        public TelnetHost Host { get; }
        public ClientConnection Connection { get; }
        public Application Application { get; internal set; }
        public TelnetTerminal Terminal { get; internal set; }
        
        internal Session(TelnetHost h, ClientConnection c)
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
        
        internal void Close()
        {
            Application?.Exit();
        }

        public int Id
        {
            get
            {
                return Connection.Id;
            }
        }

        public void SendCustomMessage(object data)
        {
            Application?.SendAppMessage(data);
        }

        public void Kick()
        {
            Host.Server.KickClient(Connection);
        }

    }
}
