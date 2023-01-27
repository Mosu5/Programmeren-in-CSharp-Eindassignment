using System.Net;

namespace ChatServer
{
    internal interface IServer
    {
        public void Reply(string message, IPEndPoint endpoint);
        public Task<Received> Receive();
    }
}