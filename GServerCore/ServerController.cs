using System;
using Lidgren.Network;

namespace GServer
{
    public class ServerController
    {
        private String _startMessage;
        private String _shutdownMessage;

        private NetServer _netServer;

        public ServerController(NetServer netServer)
        {
            _netServer = netServer;
            _startMessage = generateStartMessage();
            _shutdownMessage = generateShutdownMessage();
        }

        public String generateStartMessage()
        {
            return null;
        }

        public String generateShutdownMessage()
        {
            return null;
        }

        public void StartServer()
        {
            _netServer.Start();
        }

        public void ShutdownServer()
        {
            _netServer.Shutdown("");
        }

        void OnProcessExit(object sender, EventArgs e)
        {
            ShutdownServer();
        }
    }
}