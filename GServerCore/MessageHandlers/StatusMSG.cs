using System;
using Lidgren.Network;

namespace GServer.MessageHandlers
{
    public class StatusMSG
    {

        public StatusMSG()
        {
        }

        public static void NatIntroductionSuccess(NetIncomingMessage msg)
        {
            Console.WriteLine(msg.ReadString() + " |MSG TYPE: " + msg.MessageType + " msg.Length: " + msg.LengthBytes);
        }

        public static void ConnectionLatencyUpdated(NetIncomingMessage msg)
        {
            Console.WriteLine(msg.ReadString() + " |MSG TYPE: " + msg.MessageType + " msg.Length: " + msg.LengthBytes);
        }

        public static void ConnectionApprovalMSG(NetIncomingMessage msg)
        {
            Console.WriteLine(msg.ReadString() + " |MSG TYPE: " + msg.MessageType + " msg.Length: " + msg.LengthBytes);
        }


        public static void DiscoveryResponse(NetIncomingMessage msg)
        {
            Console.WriteLine(msg.ReadString() + " |MSG TYPE: " + msg.MessageType + " msg.Length: " + msg.LengthBytes);
        }

        public static void DiscoveryRequest(NetIncomingMessage msg)
        {
            Console.WriteLine(msg.ReadString() + " |MSG TYPE: " + msg.MessageType + " msg.Length: " + msg.LengthBytes);
        }
    }
}