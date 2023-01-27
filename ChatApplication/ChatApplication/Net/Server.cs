using System.Net.Sockets;

namespace ChatApplication.Net;

public class Server
{
    private TcpClient _client;
    public Server()
    {
        _client = new TcpClient();
    }

    public void ConnectToServer(string username)
    {
        if (!_client.Connected)
        {
            _client.Connect("127.0.0.1", 7899);
            var connectPacket = new PacketBuilder();
            connectPacket.WriteOpCode(0);
            connectPacket.WriteMessage(username);
            _client.Client.Send(connectPacket.GetPacketBytes());
        }
    }
}