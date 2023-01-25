using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TomatoLib.Network;

namespace TomatoServer
{
    /*
     * This is the server -> Should only have one instance running at all times.
     */
    internal class UdpListener : UdpBase
    {
        private readonly IPEndPoint _listenOn;

        public UdpListener() : this(NetworkSettingsContainer.ServerPort)
        {
        }

        public UdpListener(int serverPort) : this(new IPEndPoint(IPAddress.Any, serverPort))
        {
        }

        public UdpListener(IPEndPoint endpoint)
        {
            _listenOn = endpoint;
            Client = new UdpClient(_listenOn);
        }

        public void Reply(string message, IPEndPoint endPoint)
        {
            var datagram = Encoding.ASCII.GetBytes(message);
            Client.Send(datagram, datagram.Length, endPoint);
        }

        public async Task<int> ReplyAsync(string message, IPEndPoint endPoint)
        {
            var datagram = Encoding.ASCII.GetBytes(message);
            return await Client.SendAsync(datagram, datagram.Length, endPoint);
        }
    }
}