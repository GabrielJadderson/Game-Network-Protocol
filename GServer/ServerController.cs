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
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
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
            try
            {
                _netServer.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void ShutdownServer()
        {
            try
            {
                _netServer.Shutdown("");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        void OnProcessExit(object sender, EventArgs e)
        {
            ShutdownServer();
        }
    }
}