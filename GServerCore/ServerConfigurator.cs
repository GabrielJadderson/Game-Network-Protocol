using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Microsoft.Extensions.Configuration;

namespace GServerCore
{
    public class ServerConfigurator
    {
        public IPAddress IP;
        public int port;
        public NetPeerConfiguration netPeerConfig;
        public IConfiguration externalConfig;
        public ServerConfigurator()
        {
            InitStartupConfig();
        }

        public void InitStartupConfig()
        {
            try
            {
                ConfigureExternalConfig();
                ConfigureNetPeer(externalConfig);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                System.Environment.Exit(1);
            }
        }

        public void ConfigureExternalConfig()
        {
            externalConfig = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("ServerConfig.json").Build();
            IP = IPAddress.Parse(externalConfig["IPAddress"]);
        }

        public void ConfigureNetPeer(IConfiguration config)
        {
            if (config == null) throw new Exception();
            port = int.Parse(config["port"]);
            if (port == 0) throw new Exception();
            netPeerConfig = new NetPeerConfiguration("GJABD_GAME");
            netPeerConfig.Port = port;
            netPeerConfig.LocalAddress = IPAddress.Any;
            netPeerConfig.EnableUPnP = true;
            netPeerConfig.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            //ON PRODUCTION:
            //config.DisableMessageType(NetIncomingMessageType.WarningMessage);
        }


    }
}