using System;
using System.Net;
using System.Net.Sockets;
using ChatServer.NET.IO;

namespace ChatServer;

class Program
{
    static List<Client> _users;
    static TcpListener _listener;

    static void Main(string[] args)
    {
        _users = new List<Client>();
        _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7899);
        _listener.Start();

        while (true)
        {
            var client = new Client(_listener.AcceptTcpClient());
            _users.Add(client);
                
            /* Broadcast the connection to every user on the server */
            BroadcastConnection();
        }
    }

    static void BroadcastConnection()
    {
        foreach (var user in _users)
        {
            foreach (var usr in _users)
            {
                var broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOpCode(1);
                broadcastPacket.WriteMessage(usr.Username);
                broadcastPacket.WriteMessage(usr.UID.ToString());
                user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
            }
        }
    }
    public static void BroadcastMessage(string message)
    {
        foreach (var user in _users)
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteMessage(message);
            user.ClientSocket.Client.Send(messagePacket.GetPacketBytes());
        }
    }
    
    public static void BroadcastDisconnect(string uid)
    {
        var disconnectUser = _users.FirstOrDefault(x => x.UID.ToString() == uid);
        if (disconnectUser != null) _users.Remove(disconnectUser);
        
        foreach (var user in _users)
        {
            var broadcastPacket = new PacketBuilder();
            broadcastPacket.WriteOpCode(10);
            broadcastPacket.WriteMessage(uid);
            user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
        }
        
        BroadcastMessage($"{disconnectUser!.Username} has disconnected");
    }
}