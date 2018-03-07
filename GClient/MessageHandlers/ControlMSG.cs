using System;
using Lidgren.Network;

namespace GClient.MessageHandlers
{
    public class ControlMSG
    {
        public ControlMSG()
        {

        }

        public static void Error(NetIncomingMessage msg)
        {
            Console.WriteLine(msg.ReadString() + " |MSG TYPE: " + msg.MessageType + " msg.Length: " + msg.LengthBytes);
        }
        public static void VerboseDebugMessage(NetIncomingMessage msg)
        {
            Console.WriteLine(msg.ReadString() + " |MSG TYPE: " + msg.MessageType + " msg.Length: " + msg.LengthBytes);
        }

        public static void DebugMessage(NetIncomingMessage msg)
        {
            Console.WriteLine(msg.ReadString() + " |MSG TYPE: " + msg.MessageType + " msg.Length: " + msg.LengthBytes);
        }

        public static void WarningMessage(NetIncomingMessage msg)
        {
            Console.WriteLine(msg.ReadString() + " |MSG TYPE: " + msg.MessageType + " msg.Length: " + msg.LengthBytes);
        }

        public static void ErrorMessage(NetIncomingMessage msg)
        {
            Console.WriteLine(msg.ReadString() + " |MSG TYPE: " + msg.MessageType + " msg.Length: " + msg.LengthBytes);
        }
    }
}