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
    public class HeartBeatResponseMessage : Message
    {
        [XmlElement("Result")]
        [JsonProperty("Result")]
        public Result Result { get; set; }

        [XmlElement("MessageData")]
        [JsonProperty("MessageData")]
        public MessageData MessageData { get; set; }

        public HeartBeatResponseMessage()
        {
            Type = MessageType.Response;
            Action = "HeartBeat";
        }
    }
}
