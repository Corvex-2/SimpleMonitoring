using SimpleMonitoring.Communication.TCP.Serializers;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SimpleMonitoring.Communication.TCP.Shared.Protocols.Xml
{
    public class XmlProtocol : Protocol<XDocument>
    {
        protected override XDocument Deserialize(byte[] Data)
        {
            var xmlData = Encoding.UTF8.GetString(Data);
            var xmlReader = XmlReader.Create(new StringReader(xmlData), new XmlReaderSettings() { DtdProcessing = DtdProcessing.Ignore });
            return XDocument.Load(xmlReader);
        }

        protected override byte[] Serialize<T>(T Message)
        {
            if (Message is XDocument)
                return Encoding.UTF8.GetBytes(Message.ToString());
            else
                return Encoding.UTF8.GetBytes(XmlSerialization.Serialize(Message).ToString());
        }
    }
}
