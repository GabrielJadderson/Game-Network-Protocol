using System;
using Lidgren.Network;
using System.Net;
using System.Text;
using GabrielJadderson.Network.Cryptography;

namespace GClient
{
    public class Client
    {
        public readonly NetPeer netPeer;
        public readonly ECDHRSAProvider ecdhrsaProvider;

        public Client(string ip, int port)
        {
            try
            {
                ecdhrsaProvider = new ECDHRSAProvider();

                NetPeerConfiguration config = new NetPeerConfiguration("GJABD_GAME");
                config.EnableUPnP = true;
                config.LocalAddress = IPAddress.Any;

                /* FOR PRODUCTION
                   config.PingInterval = 5f;
                   config.ConnectionTimeout = 10f;
                   config.ResendHandshakeInterval = 1f;
                   config.MaximumHandshakeAttempts = 2;
                */

                netPeer = new NetPeer(config);
                netPeer.Start(); // needed for initialization

                Console.WriteLine("started peer");

                NetOutgoingMessage sendMsg = netPeer.CreateMessage();
                sendMsg.Write("RELEASE_GAME_WEB");
                sendMsg.Write(ClientConstants.version.ToString());

                //send authentication public key and verifications.
                SecureMessageEncoder sem = new SecureMessageEncoder(sendMsg);
                sem.WriteBytes(ecdhrsaProvider.GetECDHPublicKey());
                sem.WriteBytes(ecdhrsaProvider.GetSignedHash());
                sem.WriteBytes(ecdhrsaProvider.GetRSAPublicKey());
                sendMsg = sem.PackAndGet();


                IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(ip), port);
                NetConnection connection = netPeer.Connect(ipEnd, sendMsg);

                Console.WriteLine("connected to server");

                // in your separate thread
                while (netPeer.MessageReceivedEvent.WaitOne())
                {
                    NetIncomingMessage msg = netPeer.ReadMessage();
                    CMessageParseManager.parseMessage(msg, this);
                    netPeer.Recycle(msg);
                    Console.WriteLine("Unique identifier is " + NetUtility.ToHexString(netPeer.UniqueIdentifier));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }
}