using System;

namespace TomatoLib.Network
{
    /*
     * Contains values of we use for networking.
     */
    public static class NetworkSettingsContainer
    {
        #region Server

        public static Tuple<string, int> ServerTarget => new(ServerAddress, ServerPort);
        // public static string ServerAddress { get; set; } = "127.0.0.1"; //Standard address for IPv4.
        public static string ServerAddress { get; set; } = "192.168.1.10";
        public static int ServerPort { get; set; } = 32123;

        #endregion
    }
}