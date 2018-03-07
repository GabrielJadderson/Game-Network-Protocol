using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace GabrielJadderson.Network.Cryptography
{
    /// <summary>
    ///  RSA encryption decryption.
    ///  Derived from https://stackoverflow.com/questions/17128038/c-sharp-rsa-encryption-decryption-with-transmission
    /// </summary>
    public sealed class RSACryptor
    {
        public const ushort KEY_SIZE = 2048;

        public RSAParameters PublicKey { get; }
        public RSAParameters PrivateKey { get; }
        public byte[] PrivateKeyInBytes { get; }
        public byte[] PublicKeyInBytes { get; }


        public RSACryptor()
        {
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider(KEY_SIZE);
            csp.PersistKeyInCsp = false;

            PublicKey = csp.ExportParameters(false);
            PrivateKey = csp.ExportParameters(true);

            PublicKeyInBytes = csp.ExportCspBlob(false);
            PrivateKeyInBytes = csp.ExportCspBlob(true);
        }

        public string SerializeKey(RSAParameters key)
        {
            try
            {
                //we need some buffer
                var sw = new System.IO.StringWriter();
                //we need a serializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //serialize the key into the stream
                xs.Serialize(sw, key);
                return sw.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

        public RSAParameters DeserializeKey(string serializedRSAParameters)
        {
            try
            {
                if (serializedRSAParameters == null) return new RSAParameters();
                //get a stream from the string
                var sr = new System.IO.StringReader(serializedRSAParameters);
                //we need a deserializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //get the object back from the stream
                return (RSAParameters)xs.Deserialize(sr);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return new RSAParameters();
            }
        }


        public RSAParameters LoadKey(string filePath)
        {
            if (File.Exists(filePath))
            {
                string readText = File.ReadAllText(filePath);
                return DeserializeKey(readText);
            }
            else return new RSAParameters();
        }

        public void SaveKey(RSAParameters key, string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, SerializeKey(key));
            }
            else
            {
                File.Create(filePath).Close();
                SaveKey(key, filePath);
            }
        }


        public void SignData()
        {
            /*
            try
            {
                // Create a UnicodeEncoder to convert between byte array and string.
                ASCIIEncoding ByteConverter = new ASCIIEncoding();

                string dataString = "Data to Sign 42 trump ass wipe";

                // Create byte arrays to hold original, encrypted, and decrypted data.
                byte[] originalData = ByteConverter.GetBytes(dataString);
                byte[] signedData;

                // Create a new instance of the RSACryptoServiceProvider class
                // and automatically create a new key-pair.
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                // Export the key information to an RSAParameters object.
                // You must pass true to export the private key for signing.
                // However, you do not need to export the private key
                // for verification.
                RSAParameters Key = RSAalg.ExportParameters(true);

                // Hash and sign the data.
                signedData = HashAndSignBytes(originalData, Key);

                // Verify the data and display the result to the
                // console.
                if (VerifySignedHash(originalData, signedData, Key))
                {
                    Console.WriteLine("The data was verified.");
                }

                else
                {
                    Console.WriteLine("The data does not match the signature.");
                }

            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("The data was not signed or verified");

            }
            */
        }

        /// <summary>
        /// Hashes the byte array using SHA256 and returns a signed hash. private key is required for signing.
        /// </summary>
        /// <param name="DataToSign"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public byte[] HashAndSignBytes(byte[] DataToSign, RSAParameters Key)
        {
            try
            {
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(KEY_SIZE))
                {
                    RSA.ImportParameters(Key);
                    // Hash and sign the data. Pass a new instance of SHA256CryptoServiceProvider
                    // to specify the use of SHA256 for hashing.
                    return RSA.SignData(DataToSign, new SHA256CryptoServiceProvider());
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public byte[] HashAndSignBytes(byte[] DataToSign, byte[] CSPBlob)
        {
            try
            {
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(KEY_SIZE))
                {
                    RSA.ImportCspBlob(CSPBlob);
                    return RSA.SignData(DataToSign, new SHA256CryptoServiceProvider());
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public byte[] HashAndSignBytes(byte[] DataToSign)
        {
            return HashAndSignBytes(DataToSign, PrivateKey);
        }

        /// <summary>
        /// Verifies the signed data using SHA256, private key is not required for verification. returns a boolean true if verified, false otherwise.
        /// </summary>
        /// <param name="DataToVerify"></param>
        /// <param name="SignedData"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData, byte[] CSPBlob)
        {
            try
            {
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(KEY_SIZE))
                {
                    RSA.ImportCspBlob(CSPBlob);
                    return RSA.VerifyData(DataToVerify, new SHA256CryptoServiceProvider(), SignedData);
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return false;
            }
        }

        /// <summary>
        /// Verifies the signed data using SHA256, private key is not required for verification. returns a boolean true if verified, false otherwise.
        /// </summary>
        /// <param name="DataToVerify"></param>
        /// <param name="SignedData"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData, RSAParameters Key)
        {
            try
            {
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(KEY_SIZE))
                {
                    RSA.ImportParameters(Key);
                    return RSA.VerifyData(DataToVerify, new SHA256CryptoServiceProvider(), SignedData);
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return false;
            }
        }


        public byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(KEY_SIZE))
                {

                    //Import the RSA Key information. This only needs
                    //toinclude the public key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Encrypt the passed byte array and specify OAEP padding.
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.
                    encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
                }
                return encryptedData;
            }
            //Catch and display a CryptographicException
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }

        }

        public byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(KEY_SIZE))
                {
                    //Import the RSA Key information. This needs
                    //to include the private key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Decrypt the passed byte array and specify OAEP padding.
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.
                    decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                }
                return decryptedData;
            }
            //Catch and display a CryptographicException
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }

        }

        public byte[] RSADecrypt(byte[] DataToDecrypt, byte[] CSPBlob)
        {
            try
            {
                byte[] decryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(KEY_SIZE))
                {
                    //Import the RSA Key information. This needs
                    //to include the private key information.
                    RSA.ImportCspBlob(CSPBlob);
                    decryptedData = RSA.Decrypt(DataToDecrypt, false);
                }
                return decryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }
        }

        public byte[] RSAEncrypt(byte[] DataToEncrypt, byte[] CSPBlob)
        {
            try
            {
                byte[] encryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(KEY_SIZE))
                {

                    //Import the RSA Key information. This only needs
                    //to include the public key information.
                    RSA.ImportCspBlob(CSPBlob);
                    encryptedData = RSA.Encrypt(DataToEncrypt, false);
                }
                return encryptedData;
            }
            //Catch and display a CryptographicException
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }


    }
}