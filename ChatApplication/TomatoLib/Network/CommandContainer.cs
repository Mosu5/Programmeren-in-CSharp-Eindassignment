namespace TomatoLib.Network
{
    public static class CommandContainer
    {

        //Message sent by the server, requesting the client to GiVe (GV) information.
        public static readonly string GiveUsername = "GV_USERNAME";
        public static readonly string GiveConnectionDate = "GV_DATE_CONNECT";
        public static readonly string GivePingDate = "GV_DATE_PING";

        //Message sent by the server, giving the client information (ReCeiving (RC)).
        public static readonly string ReceiveChatMessage = "RC_CHAT";


        //Message sent by the client, sending the server information (SEND).
        public static readonly string SendUsername = "SEND_USERNAME";
        public static readonly string SendConnectionDate = "SEND_DATE_CONNECT";
        public static readonly string SendPingDate = "SEND_DATE_PING";
        public static readonly string SendChatMessage = "SEND_CHAT";
    }
}