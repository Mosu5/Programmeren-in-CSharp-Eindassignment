using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TomatoLib.Network;

namespace TomatoServer
{
    internal class ServerCommands
    {
        public static void DictionaryInit(IDictionary<string, Func<Received, Action>> dictionary)
        {
            dictionary.Add(CommandContainer.SendUsername, HandleUsername);
            dictionary.Add(CommandContainer.SendConnectionDate, HandleConnectionDate);
            dictionary.Add(CommandContainer.SendPingDate, HandlePingDate);
            dictionary.Add(CommandContainer.SendChatMessage, Echo);
        }

        private static Action HandleUsername(Received received)
        {
            return () =>
                TomatoUdpServer.GetSender(received).Username = received.Message[CommandContainer.SendUsername.Length..];
        }

        private static Action HandlePingDate(Received received)
        {
            return () =>
                TomatoUdpServer.GetSender(received).TimeOfLastPing =
                    DateTime.Parse(received.Message[CommandContainer.SendPingDate.Length..]);
        }

        private static Action HandleConnectionDate(Received received)
        {
            return () =>
                TomatoUdpServer.GetSender(received).TimeOfConnection =
                    DateTime.Parse(received.Message[CommandContainer.SendConnectionDate.Length..]);
        }

        public static Action SendInitClientUserInformation(Received received)
        {
            return () =>
            {
                Task.Run(async () =>
                {
                    await TomatoUdpServer.Server.ReplyAsync(CommandContainer.GiveUsername, received.Sender);
                    await TomatoUdpServer.Server.ReplyAsync(CommandContainer.GiveConnectionDate, received.Sender);
                    await TomatoUdpServer.Server.ReplyAsync(CommandContainer.GivePingDate, received.Sender);
                });
            };
        }

        private static Action Echo(Received received)
        {
            return () =>
            {
                Task.Run((() =>
                    TomatoUdpServer.Server.ReplyAsync(
                        CommandContainer.ReceiveChatMessage +
                        received.Message[CommandContainer.SendChatMessage.Length..], received.Sender)));
            };
        }
    }
}