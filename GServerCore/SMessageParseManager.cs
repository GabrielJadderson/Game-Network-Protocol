using System;
using GServer.Connection;
using GServer.MessageHandlers;
using Lidgren.Network;

namespace GServer
{
    public class SMessageParseManager
    {
        private ConnectionApprovalManager _connectionApprovalManager;

        public SMessageParseManager()
        {
            _connectionApprovalManager = new ConnectionApprovalManager();
        }

        public void parseMessage(NetIncomingMessage msg, Server server)
        {
            switch (msg.MessageType)
            {

                case NetIncomingMessageType.DiscoveryRequest:
                    StatusMSG.DiscoveryRequest(msg);
                    break;

                case NetIncomingMessageType.DiscoveryResponse:
                    StatusMSG.DiscoveryResponse(msg);
                    break;

                case NetIncomingMessageType.NatIntroductionSuccess:
                    StatusMSG.NatIntroductionSuccess(msg);
                    break;

                case NetIncomingMessageType.ConnectionLatencyUpdated:
                    StatusMSG.ConnectionLatencyUpdated(msg);
                    break;

                case NetIncomingMessageType.StatusChanged:
                    ManageStatus(msg, server);
                    break;

                case NetIncomingMessageType.ConnectionApproval:
                    _connectionApprovalManager.Approve(msg, server);
                    break;

                case NetIncomingMessageType.UnconnectedData:
                    DataMSG.UnconnectedData(msg);
                    break;

                case NetIncomingMessageType.Data:
                    DataMSG.Data(msg, server);
                    break;

                case NetIncomingMessageType.Receipt:
                    DataMSG.Receipt(msg);
                    break;

                case NetIncomingMessageType.VerboseDebugMessage:
                    ControlMSG.VerboseDebugMessage(msg);
                    break;

                case NetIncomingMessageType.DebugMessage:
                    ControlMSG.DebugMessage(msg);
                    break;

                case NetIncomingMessageType.WarningMessage:
                    ControlMSG.WarningMessage(msg);
                    break;

                case NetIncomingMessageType.Error:
                    ControlMSG.Error(msg);
                    break;

                case NetIncomingMessageType.ErrorMessage:
                    ControlMSG.ErrorMessage(msg);
                    break;

                default:
                    Console.WriteLine("Unhandled type: " + msg.MessageType);
                    break;
            }
        }

        public static void ManageStatus(NetIncomingMessage msg, Server server)
        {
            NetConnectionStatus status = (NetConnectionStatus)msg.PeekByte();
            switch (status)
            {
                case NetConnectionStatus.Connected:
                    Console.WriteLine("Connected " + msg.SenderEndPoint.ToString());
                    Console.WriteLine("Connections count: " + server.netServer.Connections.Count);
                    break;

                case NetConnectionStatus.Disconnected:
                    Console.WriteLine("Disconnected " + msg.SenderEndPoint.ToString());
                    break;

                case NetConnectionStatus.Disconnecting:
                    Console.WriteLine("Disconnecting " + msg.SenderEndPoint.ToString());
                    break;

                case NetConnectionStatus.InitiatedConnect:
                    Console.WriteLine("InitiatedConnect " + msg.SenderEndPoint.ToString());
                    break;

                case NetConnectionStatus.None:
                    Console.WriteLine("None " + msg.SenderEndPoint.ToString());
                    break;

                case NetConnectionStatus.ReceivedInitiation:
                    Console.WriteLine("ReceisvedInitiation " + msg.SenderEndPoint.ToString());
                    break;

                case NetConnectionStatus.RespondedAwaitingApproval:
                    Console.WriteLine("RespondedAwaitingApproval " + msg.SenderEndPoint.ToString());
                    break;

                case NetConnectionStatus.RespondedConnect:
                    Console.WriteLine("RespondedConnect " + msg.SenderEndPoint.ToString());
                    break;
            }
        }


    }
}