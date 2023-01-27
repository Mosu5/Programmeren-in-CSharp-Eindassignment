using System.Net.Sockets;
using ChatServer.NET;

namespace ChatServer;

public class Client
{
    public string Username { get; set; }
    public Guid UID { get; set; }
    public TcpClient ClientSocket { get; set; }

    private PacketReader _packetReader;
    public Client(TcpClient client)
    {
        ClientSocket = client;
        UID = Guid.NewGuid();
        
        // TODO Read the username from the client
        _packetReader = new PacketReader(ClientSocket.GetStream());

        var opcode =_packetReader.ReadByte();
        Username = _packetReader.ReadMessage();

        Console.WriteLine($"{DateTime.Now} - Client connected with username {Username} and UID {UID}");

        Task.Run(Process);
    }

    private void Process()
    {
        while (true)
        {
            try
            {
                var opcode = _packetReader.ReadByte();
                switch (opcode)
                {
                    case 5:
                        var message = _packetReader.ReadMessage();
                        Console.WriteLine($"{DateTime.Now} - {Username} sent a message: {message}");
                        Program.BroadcastMessage($"[{DateTime.Now}]: [{Username}]: {message}");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{DateTime.Now} - Client {Username} disconnected");
                Program.BroadcastDisconnect(UID.ToString());
                ClientSocket.Close();
                break;
            }
        }
    }
}