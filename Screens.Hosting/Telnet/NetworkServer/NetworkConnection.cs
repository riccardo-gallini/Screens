using System;
using System.Net;
using System.Net.Sockets;

namespace Screens.Hosting
{
    public class NetworkConnection 
    {
        public int Id { get; }
        public IPEndPoint RemoteEndPoint { get; }
        public DateTime ConnectionTime { get;  }

        internal Socket Socket { get; }

        
        public IPAddress RemoteAddress
        {
            get
            {
                return RemoteEndPoint.Address;
            }
        }

        internal NetworkConnection(int clientId, IPEndPoint remoteAddress, Socket socket)
        {
            this.Id = clientId;
            this.RemoteEndPoint = remoteAddress;
            this.Socket = socket;
            this.ConnectionTime = DateTime.Now;
        }
                
    }
}
