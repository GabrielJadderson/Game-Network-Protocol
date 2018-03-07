using System;
using Lidgren.Network;

namespace GabrielJadderson.Network.Cryptography
{
    /// <summary>
    /// Used to decode SecureMessages RECEIVED from the network.
    /// </summary>
    public class SecureMessageDecoder
    {
        private byte[][] _contents;

        private NetIncomingMessage _netIncomingMessage;

        public SecureMessageDecoder(NetIncomingMessage m)
        {
            _netIncomingMessage = m;
        }

        public void Decode()
        {
            try
            {
                if (_netIncomingMessage.ReadByte().Equals((byte)SecureMessageTypes.MESSAGE_START))
                {
                    int count = _netIncomingMessage.ReadInt32();
                    _contents = new byte[count][];
                    for (int i = 0; i < count; i++)
                    {
                        int arrayLength = _netIncomingMessage.ReadInt32();
                        _contents[i] = new byte[arrayLength];
                        for (int j = 0; j < arrayLength; j++)
                            _contents[i][j] = _netIncomingMessage.ReadByte();
                    }
                    if (_netIncomingMessage.ReadByte().Equals((byte)SecureMessageTypes.MESSAGE_END)) return;
                }
                _contents = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public ref byte[][] ReadAndDecodeContents()
        {
            Decode();
            return ref _contents;
        }

        public void Dispose(NetPeer Peer)
        {
            Peer.Recycle(_netIncomingMessage);
            _contents = null;
        }
    }
}