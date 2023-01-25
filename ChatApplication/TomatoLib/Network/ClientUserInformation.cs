using System;
using System.Net;

namespace TomatoLib.Network
{
    public class ClientUserInformation
    {
        public EndPoint IPEndPoint { get; }
        public string Username { get; set; }
        public DateTime TimeOfConnection { get; set; }
        public DateTime TimeOfLastPing { get; set; }

        public ClientUserInformation(IPEndPoint endPoint)
        {
            IPEndPoint = endPoint;
        }

        public override string ToString()
        {
            return $"EndPoint: {IPEndPoint}\n" +
                   $"Username: {Username}\n" +
                   $"TimeOfConnection: {TimeOfConnection}\n" +
                   $"TimeOfLastPing: {TimeOfLastPing}\n";
        }
    }
}