using System;
using Microsoft.Data.Sqlite;


namespace GServer.DB
{
    public class DBGame
    {
        private SqliteConnection _connection; //TODO: a pool of connections to increase concurrency.
        private string _conString;

        private DBManager _dbm;
        public DBGame(DBManager dbm, string database)
        {
            _dbm = dbm;
            /*
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
            */
            SqliteConnectionStringBuilder connectionString = new SqliteConnectionStringBuilder();
            connectionString.DataSource = database;
            connectionString.Cache = SqliteCacheMode.Shared;
            connectionString.Mode = SqliteOpenMode.ReadWrite;
            _conString = connectionString.ToString();

            init();
        }

        public void init()
        {
            try
            {
                _connection = new SqliteConnection(_conString);
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