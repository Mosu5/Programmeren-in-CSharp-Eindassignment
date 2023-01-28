using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using ChatApplication.Net.IO;

namespace ChatApplication.Net;

public class Server
{
    private readonly TcpClient _client;
    public PacketReader PacketReader = null!;

    public event Action ConnectedEvent = null!;
    public event Action MessageReceivedEvent = null!;
    public event Action UserDisconnectedEvent = null!;


    public Server()
    {
        _client = new TcpClient();
    }

    public void ConnectToServer(string username)
    {
        if (!_client.Connected)
        {
            try
            {
                _client.Connect("127.0.0.1", 7899);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
                throw;
            }

            PacketReader = new PacketReader(_client.GetStream());

            if (!string.IsNullOrEmpty(username))
            {
                var connectPacket = new PacketBuilder();
                connectPacket.WriteOpCode(0);
                connectPacket.WriteMessage(username);
                _client.Client.Send(connectPacket.GetPacketBytes());
            }

            ReadPackets();
        }
    }

    private void ReadPackets()
    {
        Task.Run(() =>
        {
            while (true)
            {
                var opcode = PacketReader.ReadByte();
                switch (opcode)
                {
                    case 1:
                        ConnectedEvent?.Invoke();
                        break;
                    case 5:
                        MessageReceivedEvent?.Invoke();
                        break;
                    case 10:
                        UserDisconnectedEvent?.Invoke();
                        break;
                    default:
                        Console.WriteLine("welp...");
                        break;
                }
            }
            // ReSharper disable once FunctionNeverReturns
        });
    }

    public void SendMessage(string message)
    {
        // Check if the client is connected
        // if the client is not connected, show an error message and return
        if (!_client.Connected)
        {
            throw new Exception("You are not connected to the server");
            return;
        }

        var messagePacket = new PacketBuilder();
        messagePacket.WriteOpCode(5);
        messagePacket.WriteMessage(message);
        _client.Client.Send(messagePacket.GetPacketBytes());
    }

    public void Disconnect()
    {
        // If the client is connected, disconnect it
        if (_client.Connected)
        {
            _client.Close();
        }
    }
}