using System.Security.Cryptography;
using System.Text;


namespace GabrielJadderson.Network.Cryptography
{
    public class ECDHRSAProvider
    {
        private ECDHCryptor _ecdhCryptor;
        private RSACryptor _rsaCryptor;


        public ECDHRSAProvider()
        {
            _ecdhCryptor = new ECDHCryptor();
            _rsaCryptor = new RSACryptor();
        }

        public byte[] GetECDHPublicKey()
        {
            return _ecdhCryptor.PublicKey;
        }

        /// <summary>
        /// Hashes and signs the ECDH public key and returns the signed hash.
        /// </summary>
        /// <returns>A byte array of the signed hashed public key</returns>
        public byte[] GetSignedHash()
        {
            byte[] signedData = _rsaCryptor.HashAndSignBytes(_ecdhCryptor.PublicKey);
            return signedData;
        }

        /// <summary>
        /// Checks if the public key is verified.
        /// </summary>
        /// <param name="DataToVerify"> ECDH public key in bytes</param>
        /// <param name="SignedHashedData"> A Signed Hash of the ECDH public key in bytes</param>
        /// <returns> true if verified, false otherwise.</returns>
        public bool IsVerified(byte[] DataToVerify, byte[] SignedHashedData, RSAParameters publicKey)
        {
            return _rsaCryptor.VerifySignedHash(DataToVerify, SignedHashedData, publicKey);
        }

        /// <summary>
        /// Checks if the public key is verified.
        /// </summary>
        /// <param name="DataToVerify"> ECDH public key in bytes</param>
        /// <param name="SignedHashedData"> A Signed Hash of the ECDH public key in bytes</param>
        /// <returns> true if verified, false otherwise.</returns>
        public bool IsVerified(byte[] DataToVerify, byte[] SignedHashedData, byte[] publicKey)
        {
            return _rsaCryptor.VerifySignedHash(DataToVerify, SignedHashedData, DeserializeRSAPublicKey(publicKey));
        }

        public RSAParameters DeserializeRSAPublicKey(byte[] key)
        {
            return _rsaCryptor.DeserializeKey(Encoding.ASCII.GetString(key));
        }

        public byte[] GetRSAPublicKey()
        {
            string d = _rsaCryptor.SerializeKey(_rsaCryptor.PublicKey);
            /*
            Console.WriteLine(d);
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            byte[] dd = Encoding.ASCII.GetBytes(d);
            Console.WriteLine(Encoding.ASCII.GetString(dd));
            Console.WriteLine("");
            Console.WriteLine("");
            if (d.Equals(Encoding.ASCII.GetString(dd)))
                Console.WriteLine("YES");
            else
                Console.WriteLine("NO");
            */
            return Encoding.ASCII.GetBytes(d);
        }

    }
}