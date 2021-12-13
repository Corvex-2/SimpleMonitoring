using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using SimpleMonitoring.Monitoring;
using SimpleMonitoring.Monitoring.Monitors;
using SimpleMonitoring.Utilites;

namespace SimpleMonitoring.Agent
{
    public static class Input
    {
        public static void Start()
        {
            Logging.Log("[SimpleMonitoring.Agent]", "Input routine has started, awaiting input.");
            Logging.Log("[SimpleMonitoring.Agent]", "parameters marked with { } are optional.");
            while (true)
            {
                var input = Console.ReadLine().Split(' ');
                #region INPUT: connect -ip xxx.xxx.xxx.xxx {-port xxxx}
                if (input.Length > 0 && input[0].ToLower() == "connect")
                {
                    string ip = string.Empty;
                    int port = 6653;
                    if (input.Length >= 3 && input[1].ToLower() == "-ip")
                        ip = input[2].ToLower();
                    else
                    {
                        Logging.Log("[SimpleMonitoring.Agent]", "Invalid input detected.\r\nSyntax: 'connect -ip 192.168.0.1 {-port 6653}'");
                        continue;
                    }
                    if (ip.ToLower().Count(c => c == '.') != 3)
                    {
                        Logging.Log("[SimpleMonitoring.Agent]", "Invalid IPv4-Adress entered.\r\nSyntax: 'connect -ip 192.168.0.1 {-port 6653}'");
                        continue;
                    }
                    if (input.Length >= 5 && input[3].ToLower() == "-port" && !int.TryParse(input[4], out port))
                    {
                        port = 6653;
                        Logging.Log("[SimpleMonitoring.Agent]", "Port parsing failed due to invalid input.\r\nSyntax: 'connect -ip 192.168.0.1 {-port 6653}'");
                    }
                    Linking.Connect(ip, port);
                }
                #endregion
                #region INPUT: disconnect -ip xxx.xxx.xxx.xxx
                if(input.Length > 0 && input[0].ToLower() == "disconnect")
                {
                    string ip = string.Empty;
                    if (input.Length >= 3 && input[1].ToLower() == "-ip")
                        ip = input[2].ToLower();
                    else
                    {
                        Logging.Log("[SimpleMonitoring.Agent]", "Invalid input detected.\r\nSyntax: 'disconnect -ip 192.168.0.1'");
                        continue;
                    }
                    if (ip.ToLower().Count(c => c == '.') != 3)
                    {
                        Logging.Log("[SimpleMonitoring.Agent]", "Invalid IPv4-Adress entered.\r\nSyntax: 'disconnect -ip 192.168.0.1'");
                        continue;
                    }
                    Linking.Disconnect(ip);
                }
                #endregion
            }
        }
    }
}
