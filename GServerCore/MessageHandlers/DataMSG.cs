using System;
using GabrielJadderson.Network.Cryptography;
using Lidgren.Network;

namespace GServer.MessageHandlers
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

        public static void Data(NetIncomingMessage msg, Server server)
        {
            Console.WriteLine(msg.ReadString() + " |MSG TYPE: " + msg.MessageType + " msg.Length: " + msg.LengthBytes);

            SecureMessageEncoder sem = new SecureMessageEncoder(server.netServer, msg.SenderConnection);
            /*
            sem.WriteBytes(Encoding.ASCII.GetBytes("Big Boi ass whip"));

            sem.WriteBytes(Encoding.ASCII.GetBytes("Heelo i am jack"));
            sem.WriteBytes(Encoding.ASCII.GetBytes("type and scrambled it to make a type " +
                                                   "specimen book. It has survived not only five centuries, " +
                                                   "but also the leap into electronic typesetting, remaining essentially " +
                                                   "unchanged. It was popularised in the 1960s with the release of Letraset " +
                                                   "sheets containing Lorem Ipsum passages, and more recently with desktop " +
                                                   "publishing software like Aldus PageMaker including versions of Lorem Ipsum"));
            sem.WriteBytes(Encoding.ASCII.GetBytes("fuck you"));
            sem.WriteBytes(Encoding.ASCII.GetBytes("Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of  (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum,, comes from a line in section 1.10.32."));
            */

            byte[] dhpk = server.ecdhrsaProvider.GetECDHPublicKey();
            byte[] sh = server.ecdhrsaProvider.GetSignedHash();
            byte[] rsapk = server.ecdhrsaProvider.GetRSAPublicKey();

            Console.WriteLine("dh: " + dhpk.Length);
            Console.WriteLine("sh: " + sh.Length);
            Console.WriteLine("rsa: " + rsapk.Length);
            Console.WriteLine("the complete length: " + (dhpk.Length + sh.Length + rsapk.Length));

            sem.WriteBytes(dhpk);
            sem.WriteBytes(sh);
            sem.WriteBytes(rsapk);
            sem.PackAndSend();
        }

        //Receipt of delivery
        public static void Receipt(NetIncomingMessage msg)
        {
            Console.WriteLine(msg.ReadString() + " |MSG TYPE: " + msg.MessageType + " msg.Length: " + msg.LengthBytes);
        }
    }
}