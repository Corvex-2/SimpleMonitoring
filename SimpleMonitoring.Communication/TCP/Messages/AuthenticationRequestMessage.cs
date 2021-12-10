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
    public class AuthenticationRequestMessage : Message
    {
        [XmlElement("MessageData")]
        [JsonProperty("MessageData")]
        public MessageData MessageData { get; set; }

        [XmlElement("user")]
        [JsonProperty("user")]
        public string User { get; set; }

        [XmlElement("password")]
        [JsonProperty("password")]
        public string Passwort { get; set; }

        public AuthenticationRequestMessage()
        {
            Type = MessageType.Request;
            Action = "AuthenticateUser";
        }
    }
}
