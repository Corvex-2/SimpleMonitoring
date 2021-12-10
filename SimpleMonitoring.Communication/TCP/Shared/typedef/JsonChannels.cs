using Newtonsoft.Json.Linq;
using SimpleMonitoring.Communication.TCP.Shared.Protocols.Json;

namespace SimpleMonitoring.Communication.TCP.Shared.typedef
{
    public class JsonChannel : Channel<JsonProtocol, JObject> { }
    public class JsonClientChannel : ClientChannel<JsonProtocol, JObject> { }
}
