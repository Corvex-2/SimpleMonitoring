using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SimpleMonitoring.Communication.TCP.Shared.Protocols
{
    public abstract class Protocol<TMessageType>
    {
        public const int HEADER_SIZE = 0x04;

        #region Public Implementations
        public async Task<TMessageType> ReceiveAsync(NetworkStream networkStream)
        {
            var m_bodySize = await m_HeaderAsync(networkStream).ConfigureAwait(false);
            AssertBody(m_bodySize);
            return await m_BodyAsync(networkStream, m_bodySize).ConfigureAwait(false);
        }
        public async Task SendAsync<T>(NetworkStream networkStream, T Message)
        {
            var (m_Header, m_Body) = m_Serialize(Message);
            var m_dataBuffer = new byte[m_Header.Length + m_Body.Length];
            Buffer.BlockCopy(src: m_Header, 0, dst: m_dataBuffer, 0, m_Header.Length);
            Buffer.BlockCopy(src: m_Body, 0, dst: m_dataBuffer, m_Header.Length, m_Body.Length);
            await networkStream.WriteAsync(m_dataBuffer, 0, m_dataBuffer.Length).ConfigureAwait(false);
        }
        public static int FromNetworkOrder(int Value)
        {
            return IPAddress.NetworkToHostOrder(Value);
        }
        public static int FromHostOrder(int Value)
        {
            return IPAddress.HostToNetworkOrder(Value);
        }
        #endregion


        #region Internal Implementations
        internal async Task<int> m_HeaderAsync(NetworkStream networkStream)
        {
            var mHeader = await m_ReadFixedSizeFromNetworkStreamAsync(networkStream, HEADER_SIZE).ConfigureAwait(false);
            return FromNetworkOrder(BitConverter.ToInt32(mHeader, 0));
        }
        internal async Task<TMessageType> m_BodyAsync(NetworkStream networkStream, int m_BodySize)
        {
            var mBody = await m_ReadFixedSizeFromNetworkStreamAsync(networkStream, m_BodySize).ConfigureAwait(false);
            return Deserialize(mBody);
        }
        #endregion


        #region Private Implementations
        private async Task<byte[]> m_ReadFixedSizeFromNetworkStreamAsync(NetworkStream ntNetworkStream, int ntFixedReadSize)
        {
            var ntReadBuffer = new byte[ntFixedReadSize];
            var ntSizeOfBytesRead = 0;
            while (ntSizeOfBytesRead < ntFixedReadSize)
            {
                var ntSizeOfSingleRead = await ntNetworkStream.ReadAsync(ntReadBuffer, ntSizeOfBytesRead, (ntFixedReadSize - ntSizeOfBytesRead)).ConfigureAwait(false);
                if (ntSizeOfSingleRead == 0)
                    throw new Exception("Error during m_ReadFixedSizeFromNetworkStreamAsync");
                ntSizeOfBytesRead += ntSizeOfSingleRead;
            }
            return ntReadBuffer;
        }
        #endregion


        #region Protected Implementations
        protected virtual void AssertBody(int Assert)
        {
            if (Assert < 1)
                throw new ArgumentOutOfRangeException("Message Assertion failed.");
        }
        protected (byte[] mHeader, byte[] mBody) m_Serialize<T>(T Message)
        {
            var mBody = Serialize(Message);
            var mHeader = BitConverter.GetBytes(FromHostOrder(mBody.Length));
            return (mHeader, mBody);
        }
        #endregion


        #region Abstract Implementations
        protected abstract TMessageType Deserialize(byte[] Data);
        protected abstract byte[] Serialize<T>(T Message);
        #endregion
    }
}
