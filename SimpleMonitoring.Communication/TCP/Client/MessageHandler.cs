using SimpleMonitoring.Communication.TCP.Attributes;
using SimpleMonitoring.Communication.TCP.Messages;
using System;
using System.Threading.Tasks;

namespace SimpleMonitoring.Communication.TCP.Client
{
    internal class MessageHandler
    {
        [XmlRouting("/Message[@type='Response' and @action='HeartBeat']")]
        [JsonRouting("$.action", "HeartBeat")]
        public static Task HandleMessage(HeartBeatResponseMessage response)
        {
            Client.InvokeResponseHandled(response);
            return Task.CompletedTask;
        }
        [XmlRouting("/Message[@type='Response' and @action='AuthenticateUser']")]
        [JsonRouting("$.action", "AuthenticateUser")]
        public static Task HandleMessage(AuthenticationResponseMessage response)
        {
            return Task.CompletedTask;
        }
    }
}
