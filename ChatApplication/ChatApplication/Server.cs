using System.Net.Sockets;

namespace ChatServer;

public class Server
{
    private TcpClient _client;
    public Server()
    {
        _client = new TcpClient();
    }

    public void ConnectToServer()
    {
        if (!_client.Connected)
        {
            _client.Connect("127.0.0.1", 7899);
        }
    }
}