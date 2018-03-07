using System.Threading;

namespace GServer.Concurrency
{
    public class ConcurrentExector
    {
        private Thread LoginThread; //used to handle authentications and login requests
        private Thread GameThread; //handles positional msgs and so on.
        private Thread MessageThread; //this thread waits for incomming messages and sends them to either the LoginThread or the GameThread.

        public ConcurrentExector()
        {

        }
    }
}