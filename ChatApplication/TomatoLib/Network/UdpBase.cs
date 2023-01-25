using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TomatoLib.Network
{
    //TODO: Fix disposable issue. -> Has field Client, but is not disposable.
    public abstract class UdpBase
    {
        protected UdpClient Client;

        protected UdpBase()
        {
            Client = new UdpClient();
            Client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        public async Task<Received> ReceiveAsync()
        {
            var result = await Client.ReceiveAsync();

            return new Received()
            {
                Message = Encoding.ASCII.GetString(result.Buffer, 0, result.Buffer.Length),
                Sender = result.RemoteEndPoint
            };
        }
    }
}