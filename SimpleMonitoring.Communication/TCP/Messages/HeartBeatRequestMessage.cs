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
    public class HeartBeatRequestMessage : Message
    {
        [XmlElement("MessageData")]
        [JsonProperty("MessageData")]
        public MessageData MessageData { get; set; }

        public HeartBeatRequestMessage()
        {
            Type = MessageType.Request;
            Action = "HeartBeat";
        }
    }
}
