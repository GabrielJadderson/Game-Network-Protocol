namespace GClient
{
    public class RunClient
    {

        public static Client client;

        public static void Main(string[] args)
        {
            client = new Client("128.76.136.198", 14241);

        }

    }
}