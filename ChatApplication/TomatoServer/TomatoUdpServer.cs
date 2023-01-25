using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomatoLib.ExtensionMethods;
using TomatoLib.Network;

namespace TomatoServer
{
    //Inspired by: https://stackoverflow.com/questions/19786668/c-sharp-udp-socket-client-and-server
    class TomatoUdpServer
    {
        public static readonly Queue<Action> ClientReceivedQue = new();
        public static UdpListener Server;
        public static IDictionary<string, Func<Received, Action>> CommandDictionary = new Dictionary<string, Func<Received, Action>>();

        private static readonly IList<ClientUserInformation> ConnectedClients = new List<ClientUserInformation>();


        public static void Run()
        {
            //Create a new server.
            Server = new UdpListener();

            ServerCommands.DictionaryInit(CommandDictionary);

            if (Environment.Is64BitOperatingSystem) //MachineName might throw exception if 32 bit.
            {
                Console.WriteLine($"Starting server at {DateTime.Now} on {Environment.MachineName} {NetworkSettingsContainer.ServerAddress}:{NetworkSettingsContainer.ServerPort}.");
            }

            //Start listening for messages and copy the messages back to the client.
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    var received = await Server.ReceiveAsync();

                    //We write the received message.
                    ClientReceivedQue.Enqueue(HandleMessage(received));

                    if (received.Message == "quit")
                        break;
                }
            });

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    if (ClientReceivedQue.TryDequeue(out Action actionToHandle))
                    {
                        await Task.Run(actionToHandle);
                    }
                }
            });

            string read;
            do
            {
                read = Console.ReadLine();

                //TODO: Put quit here.
                switch (read)
                {
                    case "Print":
                        Console.WriteLine("List of Clients:");
                        Console.WriteLine(ConnectedClients.ToElementsString());
                        break;
                    default:
                        Console.WriteLine("Command unclear.");
                        break;
                }
            } while (read != "quit");
        }

        private static Action HandleMessage(Received received)
        {
            Console.WriteLine(received.Message);

            GetSender(received);

            foreach (var (command, func) in CommandDictionary)
            {
                if (received.Message.StartsWith(command))
                {
                    return func.Invoke(received);
                }
            }

            throw new NotImplementedException("Command not implemented yet.");
        }

        public static ClientUserInformation GetSender(Received received)
        {
            foreach (var clientUserInformation in ConnectedClients)
            {
                if (clientUserInformation.IPEndPoint.Equals(received.Sender))
                {
                    return clientUserInformation;
                }
            }


            ConnectedClients.Add(new ClientUserInformation(received.Sender));
            Task.Run(ServerCommands.SendInitClientUserInformation(received));
            return ConnectedClients.Last();
        }
    }
}