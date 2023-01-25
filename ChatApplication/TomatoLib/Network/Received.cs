using System.Net;

namespace TomatoLib.Network
{
    public struct Received
    {
        public IPEndPoint Sender;
        public string Message;
    }
}