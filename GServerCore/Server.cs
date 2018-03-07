using System;
using Lidgren.Network;
using System.Net;
using GabrielJadderson.Network.Cryptography;
using GServer.DB;
using GServerCore;

namespace GServer
{
    public class Server
    {
        public DBManager databaseManager;
        public NetServer netServer;
        public ServerController serverController;
        public SMessageParseManager sMessageParseManager;
        public ECDHRSAProvider ecdhrsaProvider;
        public ServerConfigurator ServerConfigurator;

        public Server()
        {
            databaseManager = new DBManager();
            sMessageParseManager = new SMessageParseManager();
            ecdhrsaProvider = new ECDHRSAProvider();
            ServerConfigurator = new ServerConfigurator();

            init();
        }

        public void init()
        {
            netServer = new NetServer(ServerConfigurator.netPeerConfig);

            serverController = new ServerController(netServer);
            serverController.StartServer();

            netServer.UPnP.ForwardPort(ServerConfigurator.port, "GServer");


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