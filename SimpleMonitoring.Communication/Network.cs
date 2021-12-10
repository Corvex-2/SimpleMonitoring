using SimpleMonitoring.Utilites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMonitoring.Communication
{
    public static class Network
    {
        private static List<IPHostEntry> HOSTS { get; set; }
        private static bool PROCESSING_HOSTS { get; set; } = false;

        static Network()
        {
            RESOLVE_HOSTS();
        }

        public static string LocalIp
        {
            get
            {
                return INTERNAL_IP();
            }
        }
        public static string PublicIp
        {
            get
            {
                return EXTERNAL_IP();
            }
        }

        public static byte[] ToByteArray(string IP)
        {
            var STRBYTES = IP.Split('.');
            if (STRBYTES.Length < 4)
                return new byte[] { 127, 0, 0, 1 };

            var IPBYTES = new byte[] {
                    (byte.TryParse(STRBYTES[0], out var IP1) == true ? IP1 : (byte)127),
                    (byte.TryParse(STRBYTES[1], out var IP2) == true ? IP2 : (byte)0),
                    (byte.TryParse(STRBYTES[2], out var IP3) == true ? IP3 : (byte)0),
                    (byte.TryParse(STRBYTES[3], out var IP4) == true ? IP4 : (byte)1),
                };
            return IPBYTES;
        }
        public static IPAddress ToIPAddress(string IP)
        {
            return new IPAddress(ToByteArray(IP));
        }
        public static IPAddress ToIPAddress(byte[] IP)
        {
            return new IPAddress(IP);
        }
        public static bool TryParse(EndPoint EndPoint, out string Result)
        {
            var IPEndPoint = (IPEndPoint)EndPoint;
            if (IPEndPoint != null)
            {
                try
                {
                    Result = IPEndPoint.Address.ToString();
                    return true;
                }
                catch
                {
                    Result = "";
                    return false;
                }
            }
            Result = "";
            return false;
        }

        private static void RESOLVE_HOSTS()
        {
            if (PROCESSING_HOSTS)
                return;

            new Task(() =>
            {
                PROCESSING_HOSTS = true;
                HOSTS = new List<IPHostEntry>();
                var IPBYTES = ToByteArray(INTERNAL_IP());
                for (IPBYTES[3] = 1; IPBYTES[3] <= 254; IPBYTES[3]++)
                {
                    IPAddress ip = new IPAddress(IPBYTES);
                    var Ping = _ping.Send(ip, 100);
                    if (Ping.Status == IPStatus.Success)
                    {
                        try
                        {
                            var Entry = Dns.GetHostEntry(ip);
                            HOSTS.Add(Entry);
                            Logging.Log("[SIMPLE-MONITORING-NETWORK]", Entry.HostName + ", " + ip.ToString());
                        }
                        catch(Exception ex)
                        {
                            Logging.Log("[SIMPLE-MONITORING-NETWORK]", ip.ToString() + Environment.NewLine + ex.Message);
                        }
                    }
                }
                PROCESSING_HOSTS = false;
            }).Start();
        }
        private static string INTERNAL_IP()
        {
            UnicastIPAddressInformation _ip = null;
            var _nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var _nic in _nics)
            {
                if (_nic.OperationalStatus != OperationalStatus.Up || _nic.GetIPProperties().GatewayAddresses.Count <= 0)
                    continue;
                foreach (var _address in _nic.GetIPProperties().UnicastAddresses)
                {
                    if (_address.Address.AddressFamily != AddressFamily.InterNetwork || IPAddress.IsLoopback(_address.Address))
                        continue;
                    if (!_address.IsDnsEligible && _ip == null)
                    {
                        _ip = _address;
                        continue;
                    }
                    if (_address.PrefixOrigin != PrefixOrigin.Dhcp)
                    {
                        if (_ip == null || !_ip.IsDnsEligible)
                            _ip = _address;
                        continue;
                    }
                    return _address.Address.ToString();
                }
            }
            return (_ip != null ? _ip.Address.ToString() : "127.0.0.1");
        }
        private static string EXTERNAL_IP()
        {
            try
            {
                return new WebClient().DownloadString("http://checkip.dyndns.org/").Split(':')[1].Replace(" ", "").Split('<')[0];
            }
            catch
            {
                return "DISCONNECTED";
            }
        }

        private static Ping _ping = new Ping();
    }
}
