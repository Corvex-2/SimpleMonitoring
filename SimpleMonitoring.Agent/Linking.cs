using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleMonitoring.Communication;
using SimpleMonitoring.Communication.TCP.Client;
using SimpleMonitoring.Monitoring.System;
using SimpleMonitoring.Utilites;

namespace SimpleMonitoring.Agent
{
    public static class Linking
    {
        public static List<Client> Connections = new List<Client>();

        public static void Initialize()
        {
            if (Initialized)
                return;

            Client.OnResponseHandled += (resp) => { Logging.Log("[SimpleMonitoring.Agent]", $"Received Message from {resp.Id} {resp.GetType()}"); };
            new Task(() => 
            {
                Logging.Log("[SimpleMonitoring.Agent]", "Setting up and linking the client to Monitoring Systems in your network, checking IP-Adress now!");
                if (Network.LocalIp != "127.0.0.1" && !Network.LocalIp.StartsWith("169.254."))
                {
                    Logging.Log("[SimpleMonitoring.Agent]", "Current IPv4-Adress is eligable for linking, setting up connections now.");

                    foreach (var m in Network.GetReachableIpAdresses())
                    {
                        if(Connect(m, 6653))
                        {
                            Logging.Log("[SimpleMonitoring.Agent]", $"Automatically linked to monitor on {m}:{6653}.");
                        }
                    }
                }
                if (Connections.Count > 0)
                    Logging.Log("[SimpleMonitoring.Agent]", $"Successfully linked to {Connections.Count} monitors in your network.");
                else
                    Logging.Log("[SimpleMonitoring.Agent]", $"Unable to link to any monitors in your network, this could either be due to a missing internet connection or because there are currently no monitors running.");

            }).Start();
            Initialized = true;
        }

        public static bool Disconnect(string IpAdress)
        {
            var client = Connections.Where(x => x.IpAdress == IpAdress).FirstOrDefault();
            if(client == null || client == default(Client))
            {
                Logging.Log("[SimpleMonitoring.Agent]", $"There is currently no monitor connected on {IpAdress}.");
                return false;
            }
            Connections.Remove(client);
            client.Dispose();
            Logging.Log("[SimpleMonitoring.Agent]", $"Disconnected agent from {IpAdress}!");
            return true;
        }
        public static bool Connect(string IpAddress, int Port)
        {
            try
            {
                if (!Network.IsAvailable(IpAddress))
                {
                    Logging.Log("[SimpleMonitoring.Agent]", $"Unable connected to {IpAddress}:{Port}, the device cannot be reached!");
                    return false;
                }
                if (Connections.Any(x => x.ClientId == OS.GetAll().ComputerName))
                {
                    Logging.Log("[SimpleMonitoring.Agent]", $"Already connected to {IpAddress}:{Port}!");
                    return true;
                }

                var client = new Client(OS.GetAll().ComputerName, IpAddress, Port);
                var res = client.Run();
                res.Wait();
                if (res.Result)
                {
                    Connections.Add(client);
                    Logging.Log("[SimpleMonitoring.Agent]", $"Successfully connected to {IpAddress}:{Port}!");
                    return true;
                }
                throw new Exception();
            }
            catch 
            {
                Logging.Log("[SimpleMonitoring.Agent]", $"Unable to connect to {IpAddress}:{Port}, it is possible that there is no monitor currently running on that device!");
                return false;
            }
        }

        internal static bool Initialized { get; private set; } = false;
    }
}
