using System;

namespace GServer
{
    internal class RunServer
    {


        public static Server server;


        static void Main(string[] args)
        {
            try
            {
                server = new Server();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

    }
}