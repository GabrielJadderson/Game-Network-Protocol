using System;
using System.Collections.Generic;
using Lidgren.Network;


namespace GabrielJadderson.Network.Cryptography
{
    /// <summary>
    /// Used to encode SecureMessages TO SEND to the network.
    /// </summary>
    public class SecureMessageEncoder
    {
        private NetOutgoingMessage _netOutgoingMessage;
        private readonly NetConnection _to;

        private List<byte[]> _mBytes;
        private bool LOCK = false;

        public SecureMessageEncoder(NetPeer from, NetConnection to)
        {
            _to = to;
            _netOutgoingMessage = from.CreateMessage();
            _netOutgoingMessage.Write((byte)SecureMessageTypes.MESSAGE_START);
            _mBytes = new List<byte[]>();
        }

        public SecureMessageEncoder(NetOutgoingMessage msg)
        {
            _netOutgoingMessage = msg;
            _netOutgoingMessage.Write((byte)SecureMessageTypes.MESSAGE_START);
            _mBytes = new List<byte[]>();
        }

        public void WriteBytes(byte[] bytes)
        {
            if (!LOCK)
                _mBytes.Add(bytes);
        }


        public void Pack()
        {
            _netOutgoingMessage.Write(_mBytes.Count);
            foreach (var b in _mBytes)
            {
                _netOutgoingMessage.Write(b.Length);
                _netOutgoingMessage.Write(b);
            }
            _netOutgoingMessage.Write((byte)SecureMessageTypes.MESSAGE_END);
            LOCK = true;
        }

        public void PackAndSend()
        {
            if (_to != null)
            {
                Pack();
                _to.SendMessage(_netOutgoingMessage, NetDeliveryMethod.ReliableOrdered, 0);
            }
            else
            {
                Console.WriteLine("SecureMessage Recipient = null");
            }
        }

        public NetOutgoingMessage PackAndGet()
        {
            Pack();
            return _netOutgoingMessage;
        }

    }
}