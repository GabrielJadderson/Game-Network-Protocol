using System;
using Lidgren.Network;

namespace GabrielJadderson.Network.Cryptography
{

    public sealed class KeyExchangeManager
    {
        private ECDHRSAProvider _ecdhrsaProvider;

        public KeyExchangeManager()
        {
            _ecdhrsaProvider = new ECDHRSAProvider();

        }

        /// <summary>
        /// Sends the servers public key and the signed hashed public key to the peer.
        /// first get the public key from the peer before doing this.
        /// </summary>
        /// <param name="con"></param>
        public void SendPublicKeyDSMessage(NetPeer from, NetConnection to)
        {
            byte[] signedHash = _ecdhrsaProvider.GetSignedHash();
            byte[] publickey = _ecdhrsaProvider.GetECDHPublicKey();
            SecureMessageEncoder sem = new SecureMessageEncoder(from, to);
            sem.WriteBytes(publickey);
            sem.WriteBytes(signedHash);
            sem.PackAndSend();
        }

        public bool IsDigitalSignatureVerified(NetIncomingMessage m)
        {
            SecureMessageDecoder sed = new SecureMessageDecoder(m);
            byte[][] contents = sed.ReadAndDecodeContents();
            Console.WriteLine("SIZE OF CONTENTS: " + contents.Length);
            Console.WriteLine("SIZE OF CONTENTS: " + contents[0].Length);
            Console.WriteLine("SIZE OF CONTENTS: " + contents);
            return false;
        }

    }
}