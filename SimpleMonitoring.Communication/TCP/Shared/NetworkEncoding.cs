using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace SimpleMonitoring.Communication.TCP.Shared
{
    internal static class NetworkEncoding
    {
        public static (byte[] header, byte[] body) Serialize<T>(T Message)
        {
            var xs = new XmlSerializer(typeof(T));
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            xs.Serialize(sw, Message);

            var body = Encoding.UTF8.GetBytes(sb.ToString());
            var header = BitConverter.GetBytes(FromHostOrder(body.Length));
            return (header, body);
        }
        public static T Deserialize<T>(byte[] body)
        {
            var str = Encoding.UTF8.GetString(body);
            var xs = new XmlSerializer(typeof(T));
            var sr = new StringReader(str);
            return (T)xs.Deserialize(sr);
        }
        public static int FromNetworkOrder(int Value)
        {
            return IPAddress.NetworkToHostOrder(Value);
        }
        public static int FromHostOrder(int Value)
        {
            return IPAddress.HostToNetworkOrder(Value);
        }


    }
}
