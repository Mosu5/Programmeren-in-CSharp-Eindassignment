using System;
using System.Text;

namespace TomatoLib.Network
{
    //Client.
    public class UdpUser : UdpBase
    {
        private UdpUser()
        {
        }


        public static UdpUser ConnectTo(Tuple<string, int> toConnectTo)
        {
            return ConnectTo(toConnectTo.Item1, toConnectTo.Item2);
        }

        public static UdpUser ConnectTo(string hostname, int port)
        {
            var connection = new UdpUser();
            connection.Client.Connect(hostname, port);
            return connection;
        }

        public void Send(string message)
        {
            var datagram = Encoding.ASCII.GetBytes(message);
            Client.Send(datagram, datagram.Length);
        }
    }
}