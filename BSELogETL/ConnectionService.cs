using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace BSELogETL
{
    public class ConnectionService
    {
        private SqliteConnection CreateConnection()
        {
            var connection = new SqliteConnection(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "logBase.sqlite"));

            return connection;
        }

        public void pushFilenames(string[] files)
        {
            var con = CreateConnection();
            con.Open();
            var command = con.CreateCommand();
            foreach (var file in files)
            {
                command.CommandText = "INSERT INTO filenames(filename) values(file)";
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<string> QueryCommand<T>(string command)
        {
            IEnumerable<string> mock = new[] {""};
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