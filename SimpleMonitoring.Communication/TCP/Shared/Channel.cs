using SimpleMonitoring.Communication.TCP.Shared.Protocols;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleMonitoring.Communication.TCP.Shared
{
    public abstract class Channel<TProtocol, TMessageType> : IDisposable
    where TProtocol : Protocol<TMessageType>, new()
    {
        #region Properties and Field declarations
        internal NetworkStream                      _networkStream;
        internal Task                               _receiver;
        internal Func<TMessageType, Task>           _messageCallback;
        internal EndPoint                         _socketEndPoint;

        internal readonly TProtocol                 _protocol           = new TProtocol();
        internal readonly CancellationTokenSource   _cancellationToken  = new CancellationTokenSource();
        #endregion


        public void Attach(Socket NtSocket)
        {
            _networkStream = new NetworkStream(NtSocket, true);
            _receiver = Task.Run(Receiver, _cancellationToken.Token);
            _socketEndPoint = NtSocket.RemoteEndPoint;
        }

        public void Close()
        {
            _cancellationToken.Cancel();
            _networkStream?.Close();
        }

        public async Task SendAsync<T>(T Message) => await _protocol.SendAsync(_networkStream, Message).ConfigureAwait(false);

        public void OnMessage(Func<TMessageType, Task> m_Callback) => _messageCallback = m_Callback;

        protected virtual async Task Receiver()
        {
            while(!_cancellationToken.IsCancellationRequested)
            {
                //TODO: Pass Token to Protocol
                try
                {
                    var msg = await _protocol.ReceiveAsync(_networkStream).ConfigureAwait(false);
                    await _messageCallback(msg).ConfigureAwait(false);
                }
                catch
                {
                    Close();
                    Dispose();
                }
            }
            //Log.Create($"CLIENT DISCONNECTED ({(Network.TryParse(_socketEndPoint, out var IPAddress) ? IPAddress : "unknown")})", LoggingOptions.AllowConsole | LoggingOptions.AllowGlobal | LoggingOptions.AllowFile);
        }


        #region Disposing and Cleaning
        ~Channel() => Dispose(false);
        public void Dispose() => Dispose(true);
        protected void Dispose(bool isDisposing)
        {
            if(!_isDisposed)
            {
                _isDisposed = true;

                //TODO: Cleaning
                Close();
                _networkStream?.Dispose();

                if (isDisposing)
                    GC.SuppressFinalize(this);
            }
        }
        protected bool _isDisposed { get; private set; } = false;
        #endregion
    }
}
