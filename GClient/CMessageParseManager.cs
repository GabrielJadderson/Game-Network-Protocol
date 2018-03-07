using System;
using GabrielJadderson.Network;
using GClient.MessageHandlers;
using Lidgren.Network;

namespace GClient
{
    public class CMessageParseManager
    {
        public CMessageParseManager()
        {

        }

        public static void parseMessage(NetIncomingMessage msg, Client client)
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
                    ManageStatus(msg, client);
                    break;

                case NetIncomingMessageType.ConnectionApproval:
                    StatusMSG.ConnectionApprovalMSG(msg);
                    break;

                case NetIncomingMessageType.UnconnectedData:
                    DataMSG.UnconnectedData(msg);
                    break;

                case NetIncomingMessageType.Data:
                    DataMSG.Data(msg, client);
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
                    Console.WriteLine("Unhandled type: " + msg.MessageType + " msg.Length: " + msg.LengthBytes);
                    break;
            }
        }

        public static void ManageStatus(NetIncomingMessage msg, Client client)
        {
            NetConnectionStatus status = (NetConnectionStatus)msg.PeekByte();
            switch (status)
            {
                case NetConnectionStatus.Connected:
                    /*
                    NetOutgoingMessage buffer = peer.CreateMessage();
                    buffer.Write(0b0011_1111);
                    buffer.Write(CKeyExchangeManager.publicKey); // a 128 byte large byte array
                    buffer.Write(0b0011_0000);
                    buffer.Write(0b1100_1111);
                    buffer.Write(Client._peer.UniqueIdentifier);
                    peer.SendMessage(buffer, msg.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                    */
                    NetOutgoingMessage m = client.netPeer.CreateMessage();
                    m.Write("you little shit.");
                    client.netPeer.SendMessage(m, msg.SenderConnection, NetDeliveryMethod.ReliableOrdered);

                    Console.WriteLine("APPROVAL: uniqueId " + NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier));

                    Console.WriteLine("Connected " + msg.SenderEndPoint.ToString() + " msg.Length: " + msg.LengthBytes);
                    Console.WriteLine("Connections count: " + client.netPeer.Connections.Count);
                    break;

                case NetConnectionStatus.Disconnected:
                    msg.ReadByte();
                    string reason = msg.ReadString();

                    if (string.IsNullOrEmpty(reason))
                    {
                        Console.WriteLine("Disconnected from " + msg.SenderEndPoint.ToString() + " msg.Length: " + msg.LengthBytes);
                    }
                    else
                    {
                        if (reason.Length == 1)
                        {
                            switch (Convert.ToByte(reason[0]))
                            {
                                case (byte)DisconnectResponse.NONE:
                                    break;

                                case (byte)DisconnectResponse.INVALID_VERSION:
                                    ClientUpdateManager.CheckForUpdates();
                                    break;
                                //TODO: handle other DisconnectResponses.
                                case (byte)DisconnectResponse.BAD_SESSION_ID:
                                case (byte)DisconnectResponse.THROTTLED:
                                case (byte)DisconnectResponse.RECONNECT:
                                case (byte)DisconnectResponse.SERVER_BEING_UPDATED:
                                case (byte)DisconnectResponse.BANNED:
                                    break;

                                default: break;
                            }
                        }
                        Console.WriteLine("Disconnected from " + msg.SenderEndPoint.ToString() + " , Reason: " + reason);
                    }
                    break;

                case NetConnectionStatus.Disconnecting:
                    Console.WriteLine("Disconnecting " + msg.SenderEndPoint.ToString() + " msg.Length: " + msg.LengthBytes);
                    break;

                case NetConnectionStatus.InitiatedConnect:
                    Console.WriteLine("InitiatedConnect " + msg.SenderEndPoint.ToString() + " msg.Length: " + msg.LengthBytes);
                    break;

                case NetConnectionStatus.None:
                    Console.WriteLine("None" + " msg.Length: " + msg.LengthBytes);
                    break;

                case NetConnectionStatus.ReceivedInitiation:
                    Console.WriteLine("ReceivedInitiation " + msg.SenderEndPoint.ToString() + " msg.Length: " + msg.LengthBytes);
                    break;

                case NetConnectionStatus.RespondedAwaitingApproval:
                    Console.WriteLine("RespondedAwaitingApproval " + msg.SenderEndPoint.ToString() + " msg.Length: " + msg.LengthBytes);
                    break;

                case NetConnectionStatus.RespondedConnect:
                    Console.WriteLine("RespondedConnect " + msg.SenderEndPoint.ToString() + " msg.Length: " + msg.LengthBytes);
                    break;
            }
        }

    }
}