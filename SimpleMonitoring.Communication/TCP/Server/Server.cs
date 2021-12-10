using SimpleMonitoring.Communication.TCP.Shared.Protocols.Json;
using SimpleMonitoring.Communication.TCP.Shared.typedef;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SimpleMonitoring.Communication.TCP.Server
{
    public class Server
    {
        //internal XmlMessageDispatcher MessageDispatcher = new XmlMessageDispatcher();
        public JsonMessageDispatcher MessageDispatcher = new JsonMessageDispatcher();
        public string IpAdress { get; private set; }
        public int Port { get; private set; }

        public Server(string IP, int Port)
        {
            this.IpAdress = IP;
            this.Port = Port;
        }

        public void Run()
        {
            Information.EndPoint = new IPEndPoint(Network.ToIPAddress(IpAdress), Port);
            Information.Socket = new Socket(Information.EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            Information.Socket.Bind(Information.EndPoint);
            Information.Socket.Listen(128);

            Information.Task = new Task(() => Echo());
            Information.Task.Start();
        }

        public async Task Echo()
        {
            MessageDispatcher.Bind<MessageHandler>();
            while (true)
            {
                var clientSocket = await Task.Factory.FromAsync(
                                   new Func<AsyncCallback, object, IAsyncResult>(Information.Socket.BeginAccept),
                                   new Func<IAsyncResult, Socket>(Information.Socket.EndAccept),
                                   null).ConfigureAwait(false);

                //Log.Create($"CLIENT CONNECTED FROM: {(Network.TryParse(clientSocket.RemoteEndPoint, out var IPAddress) ? IPAddress : "unknown")}", LoggingOptions.AllowConsole | LoggingOptions.AllowGlobal | LoggingOptions.AllowFile);

                var client = new JsonChannel();

                MessageDispatcher.Bind(client);
                client.Attach(clientSocket);

            }
        }

        public ServerInformation Information { get; private set; } = new ServerInformation();
    }

    public class ServerInformation
    {
        public Socket Socket { get; internal set; }
        public IPEndPoint EndPoint { get; internal set; }
        public Task Task { get; internal set; }
        public List<Socket> Clients { get; internal set; } = new List<Socket>();
    }
}
