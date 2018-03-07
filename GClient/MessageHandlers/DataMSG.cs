using System;
using GabrielJadderson.Network.Cryptography;
using Lidgren.Network;

namespace GClient.MessageHandlers
{
    public class DataMSG
    {
        public DataMSG()
        {
        }

        public static void UnconnectedData(NetIncomingMessage msg)
        {
            Console.WriteLine(msg.ReadString() + " |MSG TYPE: " + msg.MessageType + " msg.Length: " + msg.LengthBytes);
        }

        public static void Data(NetIncomingMessage msg, Client client)
        {
            try
            {

                Console.WriteLine("msg.Length :" + msg.LengthBytes);
                SecureMessageDecoder sed = new SecureMessageDecoder(msg);
                byte[][] contents = sed.ReadAndDecodeContents();
                if (client.ecdhrsaProvider.IsVerified(contents[0], contents[1], contents[2]))
                {
                    Console.WriteLine("IS VERIFIED AND READY TO GOOOOOOOOOOOOOOO");
                }
                else
                {
                    Console.WriteLine("NOT VERIFIED");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        //Receipt of delivery
        public static void Receipt(NetIncomingMessage msg)
        {
            Console.WriteLine(msg.ReadString() + " |MSG TYPE: " + msg.MessageType + " msg.Length: " + msg.LengthBytes);
        }
    }
}