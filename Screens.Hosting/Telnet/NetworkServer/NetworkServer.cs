using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Screens.Hosting
{
    public class NetworkServer
    {
        private Socket serverSocket;
        public IPAddress ListeningOnAddress { get; }
        public int ListeningOnPort { get; }
                
        private readonly int dataSize;
        private byte[] data;
        
        private Dictionary<int, ClientConnection> connections;

        public Action<ClientConnection> OnClientConnected;
        public Action<ClientConnection> OnClientDisconnected;
        public Action<ClientConnection, byte[]> OnMessageReceived;

        internal NetworkServer(IPAddress IP, int port, int buffer = 1024)
        {
            this.ListeningOnAddress = IP;
            this.ListeningOnPort = port;

            this.dataSize = buffer;
            this.data = new byte[buffer];

            this.connections = new Dictionary<int, ClientConnection>();

            this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
                
        public void Start()
        {
            serverSocket.Bind(new IPEndPoint(ListeningOnAddress, ListeningOnPort));
            serverSocket.Listen(0);
            serverSocket.BeginAccept(new AsyncCallback(handleIncomingConnection), serverSocket);
        }

        public void Stop()
        {
            serverSocket.Close();
        }
        
        public void Send(ClientConnection conn, byte[] message)
        {
            conn.Socket.BeginSend(message, 0, message.Length, SocketFlags.None, new AsyncCallback(sendData), conn);
        }
                
        public void KickClient(ClientConnection connection)
        {
            closeSocket(connection);
            OnClientDisconnected(connection);
        }

        private void closeSocket(ClientConnection connection)
        {
            connection.Socket.Close();
            connections.Remove(connection.Id);
        }

        private void handleIncomingConnection(IAsyncResult result)
        {
            try
            {
                Socket oldSocket = (Socket)result.AsyncState;

                Socket newSocket = oldSocket.EndAccept(result);

                int clientID = connections.Count + 1;
                ClientConnection conn = new ClientConnection(clientID, (IPEndPoint)newSocket.RemoteEndPoint, newSocket);
                connections.Add(clientID, conn);

                // Do Echo
                // Do Remote Flow Control
                // Will Echo
                // Will Suppress Go Ahead
                Send(
                    conn,
                    new byte[] { 0xff, 0xfd, 0x01,
                                     0xff, 0xfd, 0x21,
                                     0xff, 0xfb, 0x01,
                                     0xff, 0xfb, 0x03 }
                );

                OnClientConnected(conn);

                    serverSocket.BeginAccept(new AsyncCallback(handleIncomingConnection), serverSocket);
                
            }

            catch { }
        }
              
        private void sendData(IAsyncResult result)
        {
            try
            {
                ClientConnection conn = (ClientConnection)result.AsyncState;

                conn.Socket.EndSend(result);

                conn.Socket.BeginReceive(data, 0, dataSize, SocketFlags.None, new AsyncCallback(receiveData), conn);
            }

            catch { }
        }

        private void receiveData(IAsyncResult result)
        {
            try
            {
                ClientConnection connection = (ClientConnection)result.AsyncState;
                                
                int bytesReceived = connection.Socket.EndReceive(result);

                if (bytesReceived == 0)
                {
                    closeSocket(connection);
                    serverSocket.BeginAccept(new AsyncCallback(handleIncomingConnection), serverSocket);
                }

                else 
                {
                    byte[] message = new byte[bytesReceived];
                    Array.Copy(data, message, bytesReceived);
                    OnMessageReceived(connection, message);
                    connection.Socket.BeginReceive(data, 0, dataSize, SocketFlags.None, new AsyncCallback(receiveData), connection);
                }
                
                    
            }

            catch { }
        }
    }
}
