using System;
using Lidgren.Network;
using System.Net;
using GabrielJadderson.Network.Cryptography;
using GServer.DB;

namespace GServer
{
    public class Server
    {
        public DBManager DatabaseManager;
        public NetServer netServer;
        public ServerController serverController;
        public SMessageParseManager sMessageParseManager;
        public ECDHRSAProvider ecdhrsaProvider;

        public Server()
        {
            DatabaseManager = new DBManager();
            sMessageParseManager = new SMessageParseManager();
            ecdhrsaProvider = new ECDHRSAProvider();
            init();
        }

        public void init()
        {
            NetPeerConfiguration config = new NetPeerConfiguration("GJABD_GAME");
            config.Port = 14242;
            config.LocalAddress = IPAddress.Any;
            config.EnableUPnP = true;
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            //ON PRODUCTION:
            //config.DisableMessageType(NetIncomingMessageType.WarningMessage);

            netServer = new NetServer(config);

            serverController = new ServerController(netServer);
            try
            {
                serverController.StartServer();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            netServer.UPnP.ForwardPort(14242, "GServer");



            Console.WriteLine("[SA] Server Started and serving.");
            Console.WriteLine("Unique identifier is " + NetUtility.ToHexString(netServer.UniqueIdentifier));

            // in your separate thread
            while (netServer.MessageReceivedEvent.WaitOne()) // this will block until a message arrives
            {
                NetIncomingMessage msg = netServer.ReadMessage();
                sMessageParseManager.parseMessage(msg, this);
                netServer.Recycle(msg);
            }
        }



    }
}