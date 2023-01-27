using System.Net;
using ChatServer.Udp;

namespace ChatServer
{
    internal class Server : IServer
    {
        private readonly UdpListener _udpListener;

        public Server() : this(new IPEndPoint(IPAddress.Any, 32123))
        {
        }

        public Server(IPEndPoint endpoint)
        {
            _udpListener = new UdpListener(endpoint);
        }

        public void Reply(string message, IPEndPoint endpoint)
        {
            _udpListener.Reply(message, endpoint);
        }

        public async Task<Received> Receive()
        {
            return await _udpListener.Receive();
        }
    }
}