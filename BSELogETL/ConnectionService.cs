using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace BSELogETL
{
    public class ConnectionService
    {
        public ConnectionService()
        {
        }

        private SqliteConnection CreateConnection()
        {
            var connectionString = new SqliteConnectionStringBuilder();
            connectionString.DataSource = "./logBase.sqlite";
            var connection = new SqliteConnection(connectionString.ConnectionString);
            
            return connection;
        }

        public IEnumerable<string> QueryCommand<T>(string command)
        {
            IEnumerable<string> mock = new []{""};
            var con = CreateConnection();
            con.Open();
            var com = con.CreateCommand();
            com.CommandText = "Hier SQL Query einfügen";
            var result = com.ExecuteScalar();
            // return result;
            
            return mock;
        }
    }
}