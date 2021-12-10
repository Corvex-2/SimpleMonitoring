using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SimpleMonitoring.Communication.TCP.Messages
{
    public class Result
    {
        [XmlAttribute("status")][JsonProperty("status")]
        public Status Status { get; set; }
    }
}
