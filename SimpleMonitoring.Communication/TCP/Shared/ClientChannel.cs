using SimpleMonitoring.Communication.TCP.Shared.Protocols;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SimpleMonitoring.Communication.TCP.Shared
{
    public class ClientChannel<TProtocol, TMessageType> : Channel<TProtocol, TMessageType>, IDisposable
    where TProtocol : Protocol<TMessageType>, new()
    {
        public async Task ConnectAsync(IPEndPoint endPoint)
        {
            var Socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            await Socket.ConnectAsync(endPoint);

            Attach(Socket);
        }
    }
}
