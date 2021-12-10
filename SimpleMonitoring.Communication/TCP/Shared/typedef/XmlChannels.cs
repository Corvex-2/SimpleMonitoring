using SimpleMonitoring.Communication.TCP.Shared.Protocols.Xml;
using System.Xml.Linq;

namespace SimpleMonitoring.Communication.TCP.Shared.typedef
{
    public class XmlChannel : Channel<XmlProtocol, XDocument> { }
    public class XmlClientChannel : ClientChannel<XmlProtocol, XDocument> { }
}
