using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SimpleMonitoring.Communication.TCP.Messages
{
    [XmlRoot("Message")]
    public abstract class Message
    {
        [XmlAttribute("id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        [XmlAttribute("type")]
        [JsonProperty("type")]
        public MessageType Type { get; set; }

        [XmlAttribute("action")]
        [JsonProperty("action")]
        public string Action { get; set; }
    }
}
