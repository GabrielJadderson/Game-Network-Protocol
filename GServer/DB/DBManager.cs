using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;


namespace GServer.DB
{
    public class DBManager
    {
        private readonly DBLogin _dBLogin;
        private readonly DBGame _dBGame;
        public DBLogin LoginDatabase => _dBLogin;
        public DBGame GameDatabase => _dBGame;

        public DBManager()
        {
            // createDBs();

            _dBLogin = new DBLogin(this, "LOGIN_DATABASE.sqlite");
            _dBGame = new DBGame(this, "GAME_SERVER_DATABASE.sqlite");
        }


        public bool TryExecuteNonQuery(string command, SQLiteConnection connection)
        {
            try
            {
                using (SQLiteCommand co = new SQLiteCommand(command, connection))
                {
                    co.VerifyOnly();
                    co.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public List<byte[]> ReadMultiBytes(SQLiteCommand c)
        {
            try
            {
                List<byte[]> ds = new List<byte[]>();
                using (SQLiteDataReader reader = c.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ds.Add(GetBytes(reader));
                    }
                }
                return ds;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        static byte[] GetBytes(SQLiteDataReader reader)
        {
            try
            {
                const int CHUNK_SIZE = 2 * 1024;
                byte[] buffer = new byte[CHUNK_SIZE];
                long fieldOffset = 0;
                using (MemoryStream stream = new MemoryStream())
                {
                    long bytesRead;
                    while ((bytesRead = reader.GetBytes(0, fieldOffset, buffer, 0, buffer.Length)) > 0)
                    {
                        stream.Write(buffer, 0, (int)bytesRead);
                        fieldOffset += bytesRead;
                    }
                    return stream.ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }


        /// <summary>
        ///  EXAMPLE:
        ///  Inserts into an already existing table.
        ///  int x = Insert("Images", new[] { "Id", "Data" }, new object[] { 12, data }, _gameServerDB);
        /// </summary>
        /// <param name="Table">The name of the Table eg. "Images"</param>
        /// <param name="types">The types to be inserted, note you must include be the same amount of types as values to be inserted. eg. { "Id", "Data" }</param>
        /// <param name="values">The values new object[] { 12, data }, where data here is a byte[]</param>
        /// <param name="connection">A Valid connection</param>
        /// <returns>-1 if failed/error. On suceed returns a positive value indicating the number of columns/rows effected, usually 1</returns>
        public int Insert(string table, string[] types, object[] values, SQLiteConnection connection)
        {
            if (table != null || types != null || values != null || types.Length != values.Length || connection != null)
            {
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("INSERT INTO " + table + "(");
                    for (int i = 0; i < types.Length; i++)
                    {
                        if (i > 0)
                            sb.Append(",");
                        sb.Append(types[i]);
                    }
                    sb.Append(") VALUES (");
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (i > 0)
                            sb.Append(",");
                        sb.Append("?");
                    }
                    sb.Append(")");
                    using (SQLiteCommand c = new SQLiteCommand(sb.ToString(), connection))
                    {
                        try
                        {
                            for (int i = 0; i < values.Length; i++)
                            {
                                c.Parameters.Add(new SQLiteParameter(types[i], values[i]));
                            }
                            c.VerifyOnly();
                            return c.ExecuteNonQuery();
                        }
                        catch (SQLiteException e)
                        {
                            Console.WriteLine(e);
                            return -1;
                        }
                    }
                }
            }
            return -1;
        }


        private void createDBs()
        {
            SQLiteConnection.CreateFile("GAME_SERVER_DATABASE.sqlite");
            SQLiteConnection.CreateFile("LOGIN_DATABASE.sqlite");
        }


        private void initConnections()
        {






            /*
           byte[] ph = Encoding.ASCII.GetBytes("SHA256 ASDASDG4214213DDQWDQ31122112");
           Console.WriteLine("phl = " + ph.Length);


           for (int i = 0; i < 500; i++)
           {
               string sql22 = "INSERT INTO Users (UName, PHash) VALUES (?, ?)";
               SQLiteCommand cm = new SQLiteCommand(sql22, _loginServerDB);
               cm.Parameters.Add(new SQLiteParameter("Username", "me@example.com"));
               cm.Parameters.Add(new SQLiteParameter("PHash", ph));
               cm.VerifyOnly();
               cm.ExecuteNonQuery();
           }
           */
            /*
             byte[] ph = Encoding.ASCII.GetBytes("SHA256 ASDASDG4214213DDQWDQ31122112");
             Console.WriteLine("phl = " + ph.Length);


             string sql22 = "INSERT INTO Users (UName, PHash) VALUES (?, ?)";
             SQLiteCommand cm = new SQLiteCommand(sql22, _loginServerDB);
             cm.Parameters.Add(new SQLiteParameter("Username", "6542432213"));
             cm.Parameters.Add(new SQLiteParameter("PHash", ph));
             cm.VerifyOnly();
             cm.ExecuteNonQuery();
             */





            /*


            //ID INTEGER PRIMARY KEY AUTOINCREMENT
            //string sql2 = "CREATE TABLE Images(Id INTEGER PRIMARY KEY, Data BLOB)";
            //SQLiteCommand command = new SQLiteCommand(sql2, _gameServerDB);
            //command.ExecuteNonQuery();

            byte[] data = Encoding.ASCII.GetBytes("BIG DICK STRONG AIMBIG DICK STRONG AIMBIG DICK STRONG AIMBIG DICK STRONG AIMBIG DICK STRONG AIM");

            Stopwatch s = new Stopwatch();

            byte[] output = null;
            int id = 0;


            String ss = "SELECT Data FROM Images WHERE Id=12 OR Id=1";
            SQLiteCommand c = new SQLiteCommand(ss, _gameServerDB);

            try
            {
                c.VerifyOnly();
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e);
            }

            s.Start();
            List<byte[]> ds = ReadMultiBytes(c);
            s.Stop();

            if (ds != null)
            {
                Console.WriteLine("contents: " + Encoding.ASCII.GetString(ds[0]));
                Console.WriteLine("contents: " + Encoding.ASCII.GetString(ds[1]));
            }




            Console.WriteLine(s.Elapsed.Milliseconds);
            */
        }

    }
}