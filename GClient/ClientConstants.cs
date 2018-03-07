using System;

namespace GClient
{
    public class ClientConstants
    {
        public static readonly Version version = new Version(1, 0, 0, 10); //touching these can break everything and won't allow peers to connect to the server.
    }
}