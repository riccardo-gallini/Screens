using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Screens.Hosting.Telnet
{
    public class TelnetSession : ISession
    {
        public DateTime ConnectionTime { get; }
        public string ConnectionId { get; }

        public IHost Host => telnetHost;
        public Terminal Terminal => telnetTerminal;
        public IPEndPoint RemoteEndPoint => (IPEndPoint)Socket.RemoteEndPoint;
        public int? RemotePort => RemoteEndPoint.Port;
        public IPAddress RemoteAddress => RemoteEndPoint.Address;
        
        private TelnetHost telnetHost;
        private TelnetTerminal telnetTerminal;
                

        internal Socket Socket { get; }
        private readonly int dataSize;
        private byte[] data;


        internal TelnetSession(TelnetHost h, Socket socket)
        {
            ConnectionId = Guid.NewGuid().ToString();
            ConnectionTime = DateTime.Now;

            telnetHost = h;
            Socket = socket;
            dataSize = socket.ReceiveBufferSize;
            data = new byte[dataSize];
        }

        //TODO: removed CustomAppMessage

        public void Kick()
        {
            Terminal.SendCloseRequest();

            Socket.Close();
            telnetHost.ClientDisconnected(this);
        }

        public void Run()
        {
            var term = new TelnetTerminal(this);
            this.telnetTerminal = term;


            // Do Echo
            // Do Remote Flow Control
            // Will Echo
            // Will Suppress Go Ahead
            Send(
                new byte[] { 0xff, 0xfd, 0x01,
                                     0xff, 0xfd, 0x21,
                                     0xff, 0xfb, 0x01,
                                     0xff, 0xfb, 0x03 }
            );

            Task.Run(() => telnetHost.Main(term));
        }


        public void Send(byte[] message)
        {
            Socket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(sendData), null);
        }
        
        private void sendData(IAsyncResult result)
        {
            try
            {
                Socket.EndSend(result);

                Socket.BeginReceive(data, 0, dataSize, SocketFlags.None, new AsyncCallback(receiveData), null);
            }

            catch { }
        }

        private void receiveData(IAsyncResult result)
        {
            try
            {
                int bytesReceived = Socket.EndReceive(result); //TODO: ObjectDisposedException!

                if (bytesReceived == 0)
                {
                    Socket.Close();
                    //server.BeginAccept(new AsyncCallback(handleIncomingConnection), server);
                }

                else
                {
                    byte[] message = new byte[bytesReceived];
                    Array.Copy(data, message, bytesReceived);
                    
                    telnetTerminal.ProcessRawData(message);

                    Socket.BeginReceive(data, 0, dataSize, SocketFlags.None, new AsyncCallback(receiveData), null);
                }


            }

            catch { }
        }

    }
}
