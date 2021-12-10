using System.Net;
using System.Threading.Tasks;
using System;
using SimpleMonitoring.Communication.TCP.Shared.Protocols.Json;
using SimpleMonitoring.Communication.TCP.Shared.typedef;
using SimpleMonitoring.Communication.TCP.Messages;

namespace SimpleMonitoring.Communication.TCP.Client
{
    public class Client
    {
        public delegate void ResponseEventHandler(Message response);
        public static event ResponseEventHandler OnResponseHandled;
        internal static void InvokeResponseHandled(Message response) => OnResponseHandled?.BeginInvoke(response, OnResponseHandled.EndInvoke, null);

        public string ClientId { get; private set; }
        public JsonMessageDispatcher MessageDispatcher { get; } = new JsonMessageDispatcher();
        public JsonClientChannel Channel { get; } = new JsonClientChannel();

        //internal XmlMessageDispatcher MessageDispatcher { get; } = new XmlMessageDispatcher();
        //internal XmlClientChannel Channel { get; } = new XmlClientChannel();

        public string IpAdress { get; private set; }
        public int Port { get; private set; }

        public Client(string clientId, string IpAdress, int Port)
        {
            ClientId = clientId;
            this.IpAdress = IpAdress;
            this.Port = Port;
        }

        internal async Task HeartBeat(int interval)
        {
            while(true)
            {
                var Request = new HeartBeatRequestMessage
                {
                    Id = $"{ClientId}",
                    MessageData = new MessageData() { Id = $"DATA001" },
                };
                await Channel.SendAsync(Request);
                await Task.Delay(interval * 1000);
            }
        }

        public async void Run()
        {
            var EndPoint = new IPEndPoint(Network.ToIPAddress(IpAdress), Port);

            MessageDispatcher.Bind(Channel);
            MessageDispatcher.Bind<MessageHandler>();

            await Channel.ConnectAsync(EndPoint).ConfigureAwait(false);

            _ = Task.Run(() => HeartBeat(10));

            var Request = new AuthenticationRequestMessage
            {
                Id = $"AU_001",
                MessageData = new MessageData() { Id = $"DATA001" },
            };
            await Channel.SendAsync(Request);

            Console.ReadLine();
        }
    }
}
