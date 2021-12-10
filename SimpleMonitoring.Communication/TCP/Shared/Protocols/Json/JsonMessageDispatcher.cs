using Newtonsoft.Json.Linq;
using SimpleMonitoring.Communication.TCP.Attributes;
using SimpleMonitoring.Communication.TCP.Serializers;
using System;
using System.Reflection;

namespace SimpleMonitoring.Communication.TCP.Shared.Protocols.Json
{
    public class JsonMessageDispatcher : MessageDispatcher<JObject>
    {
        protected override bool MatchRouting(RoutingAttribute routing, JObject message) => (message.SelectToken(routing.Routing)?.ToString() == (routing as JsonRoutingAttribute).Value);
        protected override JObject Serialize<TResult>(TResult result) => JsonSerialization.Serialize(result);
        protected override TParam Deserialize<TParam>(JObject Message) => JsonSerialization.Deserialize<TParam>(Message);
        protected override bool GetRouting(MethodInfo methodInfo, out RoutingAttribute route)
        {
            route = methodInfo.GetCustomAttribute<JsonRoutingAttribute>();
            if (route != null)
                return true;
            return false;
        }

        protected override object Deserialize(Type paramType, JObject message) => JsonSerialization.ToObject(paramType, message);
    }
}
