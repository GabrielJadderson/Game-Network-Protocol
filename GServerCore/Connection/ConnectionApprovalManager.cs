using System;
using System.Collections.Concurrent;
using GabrielJadderson.Network;
using GabrielJadderson.Network.Cryptography;
using Lidgren.Network;

namespace GServer.Connection
{
    public class ConnectionApprovalManager
    {
        private BlockingCollection<NetConnection> queuedApprovals;

        public ConnectionApprovalManager()
        {

        }

        public void Approve(NetIncomingMessage msg, Server server)
        {
            if (msg.PeekString() != null)
            {
                switch (msg.ReadString())
                {
                    case "RELEASE_GAME_IOS":
                    case "RELEASE_GAME_ANDROID":
                    case "RELEASE_GAME_DESKTOP":
                    case "RELEASE_GAME_WEB":

                        if (HasCorrectVersion(msg))
                        {
                            try
                            {
                                //proceed
                                SecureMessageDecoder sed = new SecureMessageDecoder(msg);
                                byte[][] contents = sed.ReadAndDecodeContents();

                                if (server.ecdhrsaProvider.IsVerified(contents[0], contents[1], contents[2]))
                                {
                                    msg.SenderConnection.Approve();
                                    Console.WriteLine("APPROVAL: uniqueId " + NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier));
                                    return;
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                msg.SenderConnection.Deny();
                            }
                        }
                        else
                        {
                            //send msg forcing to update.
                            msg.SenderConnection.Deny(Convert.ToChar(DisconnectResponse.RECONNECT).ToString());
                            return;
                        }
                        break;

                    default: msg.SenderConnection.Deny(); return;
                }
            }
            msg.SenderConnection.Deny();
        }

        public bool HasCorrectVersion(NetIncomingMessage msg)
        {
            string s;
            if ((s = msg.ReadString()) != null)
                if (ServerConstants.version.Equals(new Version(s)))
                    return true;
                else
                    return false;
            return false;
        }
    }
}