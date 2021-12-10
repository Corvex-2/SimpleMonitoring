using SimpleMonitoring.Communication.TCP.Attributes;
using SimpleMonitoring.Communication.TCP.Messages;
using System.Threading.Tasks;

namespace SimpleMonitoring.Communication.TCP.Server
{
    internal class MessageHandler
    {
        [XmlRouting("/Message[@type='Request' and @action='HeartBeat']")]
        [JsonRouting("$.action", "HeartBeat")]
        public static Task<HeartBeatResponseMessage> HandleMessage(HeartBeatRequestMessage request)
        {
            var response = new HeartBeatResponseMessage()
            {
                Id = request.Id,
                MessageData = request.MessageData,
                Result = new Result() { Status = Status.Success }
            };

            //Log.Create($"Received Request: {request.Action}, ID:{request.Id}", LoggingOptions.AllowConsole | LoggingOptions.AllowHandlers);

            return Task.FromResult(response);
        }
        [XmlRouting("/Message[@type='Request' and @action='AuthenticateUser']")]
        [JsonRouting("$.action", "AuthenticateUser")]
        public static Task<AuthenticationResponseMessage> HandleMessage(AuthenticationRequestMessage request)
        {
            var response = new AuthenticationResponseMessage()
            {
                Id = request.Id,
                MessageData = request.MessageData,
                Result = new Result() { Status = Status.Failure }
            };

            //Log.Create($"Received Request: {request.Action}, ID:{request.Id}", LoggingOptions.AllowConsole | LoggingOptions.AllowHandlers);

            return Task.FromResult(response);
        }
    }
}
