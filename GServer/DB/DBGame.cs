using System;
using System.Data.SQLite;


namespace GServer.DB
{
    public class DBGame
    {
        private SQLiteConnection _connection; //TODO: a pool of connections to increase concurrency.
        private string _conString;

        private DBManager _dbm;
        public DBGame(DBManager dbm, string database)
        {
            _dbm = dbm;
            _conString = "Data Source=" + database + ";" +
                "UseUTF16Encoding=True;" +
                "Version=3;" +
                "Pooling=True;" +
                "Max Pool Size=100;" +
                "FailIfMissing=True;" +
                "Cache Size=2000;" +
                "New=False;" +
                "Page Size=1024;" +
                "PRAGMA auto_vacuum=2;" +
                "PRAGMA automatic_index=true;";

            init();
        }

        public void init()
        {
            try
            {
                _connection = new SQLiteConnection(_conString);
                _connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                System.Environment.Exit(1);
            }
        }


    }
}