using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace GServer.DB
{
    public class DBLogin
    {

        private SqliteConnection _connection; //TODO: a pool of connections to increase concurrency.
        private string _conString;

        private DBManager _dbm;
        public DBLogin(DBManager dbm, string database)
        {
            _dbm = dbm;
            /*
            _conString = "Data Source=" + database + ";" +
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

            Init();


            //SetupLoginDatabase(_connection);
        }

        public void Init()
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

        /// <summary>
        /// Tries to Create a new user if it fails it returns an appropriate error-code for the failure.
        /// </summary>
        /// <param name="name"> The User name this is an email address and must be unique</param>
        /// <param name="phash"> The phash</param>
        /// <param name="id"> If the user was created sucessfully returns the user's id, if not returns -1</param>
        /// <param name="connection"> The connection to do this.</param>
        /// <returns>An integer representing the error-code or 1 for success.
        /// On failure the following error-codes are returned:
        /// -1: general failure
        /// -2: user already exists with that username
        /// </returns>
        public int CreateNewUser(string name, byte[] phash, out long id, out byte[] guid, SqliteConnection connection)
        {
            if (name != null && phash != null && connection != null)
            {
                try
                {
                    guid = GenerateGUID();
                    string sql = "INSERT INTO Users (UName, PHash, GUID) VALUES (@name, @hash, @guid)";
                    using (SqliteCommand cm = new SqliteCommand(sql, connection))
                    {
                        cm.Parameters.Add(new SqliteParameter("@name", name));
                        cm.Parameters.Add(new SqliteParameter("@hash", phash));
                        cm.Parameters.Add(new SqliteParameter("@guid", guid));
                        cm.ExecuteNonQuery();
                        id = GetLastInsertRowId(connection);
                        return 1;
                    }
                }
                catch (SqliteException e)
                {
                    switch (e.ErrorCode) //SQLiteErrorCode
                    {
                        case 1: // SQL logic error
                            id = -1;
                            guid = null;
                            return -1;

                        case 19: //constraint failed = uname not unique -> user already exists with that uname.
                            id = -1;
                            guid = null;
                            return -2;

                        default: //don't touch
                            id = -1;
                            guid = null;
                            return -1;
                    }
                }
            }
            id = -1;
            guid = null;
            return -1;
        }

        public long GetLastInsertRowId(SqliteConnection connection)
        {
            // Assuming connection is an open connection from your INSERT
            using (SqliteCommand command = new SqliteCommand("SELECT last_insert_rowid()", connection))
            {
                return (long)command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Tries to get the user, if successful returns the id and phash as well.
        /// </summary>
        /// <param name="uname"> The username to search for, the user must exist in the database for this method to return true.</param>
        /// <param name="id"> returns -1 on failure or the actual id of the user in the database.</param>
        /// <param name="phash">return null on failure or the actual phash of the user in the db.</param>
        /// <param name="connection"> The connection to execute this.</param>
        /// <returns>True on success, false otherwise. </returns>
        public bool GetUserFromName(string uname, out long id, out byte[] phash, out byte[] guid, SqliteConnection connection)
        {
            if (uname != null && connection != null)
            {
                try
                {
                    string sql3 = "SELECT ID, PHash, GUID FROM Users WHERE UName=@name";
                    SqliteCommand cmd = new SqliteCommand(sql3, connection);
                    cmd.Parameters.Add(new SqliteParameter("@name", uname));
                    using (SqliteDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            id = (long)dr["ID"];
                            phash = (byte[])dr["PHash"];
                            guid = (byte[])dr["GUID"];
                            return true;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    id = -1;
                    phash = null;
                    guid = null;
                    return false;
                }
            }
            id = -1;
            phash = null;
            guid = null;
            return false;
        }

        public bool GetUserFromGUID(byte[] guid, out long id, out byte[] phash, out string uname, SqliteConnection connection)
        {
            if (guid != null && connection != null)
            {
                try
                {
                    string sql3 = "SELECT ID, UName, PHash FROM Users WHERE GUID=@param";
                    SqliteCommand cmd = new SqliteCommand(sql3, connection);
                    cmd.Parameters.Add(new SqliteParameter("@param", guid));
                    using (SqliteDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            id = (long)dr["ID"];
                            uname = (string)dr["UName"];
                            phash = (byte[])dr["PHash"];
                            return true;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    id = -1;
                    phash = null;
                    uname = null;
                    return false;
                }
            }
            id = -1;
            phash = null;
            uname = null;
            return false;
        }

        public bool VerifyUser(string name, byte[] phash, out long id, out byte[] guid, SqliteConnection connection)
        {
            if (name != null && phash != null && connection != null)
            {
                try
                {
                    string sql = "SELECT ID, GUID FROM Users WHERE UName=@param AND PHash=@param2";
                    using (SqliteCommand c = new SqliteCommand(sql, connection))
                    {
                        c.Parameters.Add(new SqliteParameter("@param", name));
                        c.Parameters.Add(new SqliteParameter("@param2", phash));
                        using (SqliteDataReader dr = c.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                string nameX = null;
                                id = (long)dr["ID"];
                                guid = (byte[])dr["GUID"];
                                if (guid != null && guid.Length == 20)
                                    return true;
                                return false;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    id = -1;
                    guid = null;
                    return false;
                }
            }
            id = -1;
            guid = null;
            return false;
        }

        public List<long> GetIDResultset(SqliteCommand cmd)
        {
            if (cmd != null)
            {
                using (SqliteDataReader dr = cmd.ExecuteReader())
                {
                    List<long> longs = new List<long>();
                    int count = 0;
                    while (dr.Read())
                    {
                        longs.Add(dr.GetInt64(0));
                    }
                    return longs;
                }
            }
            return null;
        }

        //returns 20 bytes 16 for guid and 4 for date
        public byte[] GenerateGUID()
        {
            return Guid.NewGuid().ToByteArray().Concat(BitConverter.GetBytes(UInt32.Parse(DateTime.Now.ToString("yyMMdd")))).ToArray();
        }

        public void ReadGUID(byte[] GUID, out string GUIDString, out int Date)
        {
            if (GUID != null)
            {
                try
                {
                    byte[] g = GUID.Take(16).ToArray();
                    byte[] d = new byte[4];
                    for (int i = 0; i < 4; i++)
                        d[i] = GUID[i + 16];

                    GUIDString = string.Concat(g.Select(b => b.ToString("X2")).ToArray());
                    Date = BitConverter.ToInt32(d, 0);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            GUIDString = null;
            Date = -1;
        }


        private void SetupLoginDatabase(SqliteConnection connection)
        {
            string sql1 = "CREATE TABLE Users (ID INTEGER PRIMARY KEY AUTOINCREMENT, UName TEXT UNIQUE NOT NULL, PHash BLOB NOT NULL, GUID TEXT UNIQUE NOT NULL)";
            _dbm.TryExecuteNonQuery(sql1, connection);
            string sql2 = "CREATE UNIQUE INDEX idx_users_uname ON Users (UName);"; //create index on name
            _dbm.TryExecuteNonQuery(sql2, connection);
            string sql3 = "CREATE UNIQUE INDEX idx_users_guid ON Users (GUID);"; //create index on guid
            _dbm.TryExecuteNonQuery(sql3, connection);
        }

    }
}