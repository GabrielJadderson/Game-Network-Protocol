using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Elliptic;

namespace GabrielJadderson.Network.Cryptography
{
    /// <summary>
    /// This Class encapsulates the Diffie-Hellman key exchange algorithm.
    /// </summary>
    public sealed class ECDHCryptor
    {
        public byte[] PublicKey { get; }

        private readonly byte[] _privateKey; //DO NOT SHARE THIS
        private byte[] _sharedKey; //AND THIS
        private bool _allKeysGenerated;

        public ECDHCryptor()
        {
            //first we generate random bytes as starting sequence
            byte[] aliceRandomBytes = new byte[32]; //must be 32 bytes long
            RNGCryptoServiceProvider.Create().GetBytes(aliceRandomBytes); //generate random bytes

            _privateKey = Curve25519.ClampPrivateKey(aliceRandomBytes);
            PublicKey = Curve25519.GetPublicKey(_privateKey);
        }

        // call this method when the public key has been received from the endpoint.
        public void GenerateSharedSecret(byte[] OtherPublicKey)
        {
            _sharedKey = Curve25519.GetSharedSecret(_privateKey, OtherPublicKey);
            _allKeysGenerated = true;
        }

        public void Encrypt(byte[] secretMessage, out byte[] encryptedMessage, out byte[] iv)
        {
            if (_allKeysGenerated)
            {
                try
                {
                    using (Aes aes = new AesCryptoServiceProvider())
                    {
                        aes.Key = _sharedKey;
                        iv = aes.IV;
                        aes.KeySize = 128;
                        // Encrypt the message
                        using (MemoryStream ciphertext = new MemoryStream())
                        using (CryptoStream cs = new CryptoStream(ciphertext, aes.CreateEncryptor(),
                            CryptoStreamMode.Write))
                        {
                            byte[] plaintextMessage = secretMessage;
                            cs.Write(plaintextMessage, 0, plaintextMessage.Length);
                            cs.Close();
                            encryptedMessage = ciphertext.ToArray();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    encryptedMessage = null;
                    iv = null;
                }
            }
            encryptedMessage = null;
            iv = null;
        }

        public byte[] Decrypt(byte[] encryptedMessage, byte[] iv)
        {
            if (_allKeysGenerated)
            {
                try
                {
                    using (Aes aes = new AesCryptoServiceProvider())
                    {
                        aes.Key = _sharedKey;
                        aes.IV = iv;
                        aes.KeySize = 128;
                        // Decrypt the message
                        using (MemoryStream plaintext = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(plaintext, aes.CreateDecryptor(),
                                CryptoStreamMode.Write))
                            {
                                cs.Write(encryptedMessage, 0, encryptedMessage.Length);
                                cs.Close();
                                return plaintext.ToArray();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    return null;
                }
            }
            return null;
        }


    }
}