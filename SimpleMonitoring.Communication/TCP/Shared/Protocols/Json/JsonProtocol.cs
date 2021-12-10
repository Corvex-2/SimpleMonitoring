using System.Text;
using Newtonsoft.Json.Linq;
using SimpleMonitoring.Communication.TCP.Serializers;

namespace SimpleMonitoring.Communication.TCP.Shared.Protocols.Json
{
    public class JsonProtocol : Protocol<JObject>
    {
        protected override JObject Deserialize(byte[] Data) => JsonSerialization.Deserialize(Encoding.UTF8.GetString(Data));
        protected override byte[] Serialize<T>(T Message) => Encoding.UTF8.GetBytes(JsonSerialization.Serialize(Message).ToString());
    }
}
