using System.Collections.Generic;

namespace GabrielJadderson.Network
{
    public class NetworkConstants
    {
        /// <summary>
        /// The seconds that must elapse for a channel to be disconnected after no read operations.
        /// </summary>
        public const byte READ_IDLE_SECONDS = 5;

        /// <summary>
        /// The maximum amount of incoming messages per cycle.
        /// </summary>
        public const byte MESSAGE_LIMIT = 15;

        /// <summary>
        /// The sessions list contains the connected sessions. upon disconnection of a peer the session is removed.
        /// </summary>
        private readonly List<int> sessions = new List<int>();
        public List<int> Sessions => sessions;

        /// <summary>
        /// A private constructor to discourage external instantiation.
        /// </summary>
        private NetworkConstants()
        {

        }
    }
}