using SimpleMonitoring.Communication.TCP.Attributes;
using SimpleMonitoring.Communication.TCP.Serializers;
using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace SimpleMonitoring.Communication.TCP.Shared.Protocols.Xml
{
    public class XmlMessageDispatcher : MessageDispatcher<XDocument>
    {
        protected override bool MatchRouting(RoutingAttribute routing, XDocument message) => (bool)(message.XPathEvaluate($"boolean({routing.Routing})") ?? false);
        protected override XDocument Serialize<TResult>(TResult result) => XmlSerialization.Serialize<TResult>(result);
        protected override TParam Deserialize<TParam>(XDocument Message) => XmlSerialization.Deserialize<TParam>(Message);

        protected override bool GetRouting(MethodInfo methodInfo, out RoutingAttribute route)
        {
            route = methodInfo.GetCustomAttribute<XmlRoutingAttribute>();
            if (route != null)
                return true;
            return false;
        }

        protected override object Deserialize(Type paramType, XDocument message) => new XmlSerializer(paramType).Deserialize(new StringReader(message.ToString()));
    }
}
