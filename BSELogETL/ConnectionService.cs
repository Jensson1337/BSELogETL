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
            return new SqliteConnection("Data Source=identifier.sqlite");
        }

        public void PushFilenames(string[] files)
        {
            var con = CreateConnection();
            con.Open();
            var command = con.CreateCommand();
            foreach (var file in files)
            {
                command.CommandText = $"INSERT INTO filenames(filename) VALUES (\"{file}\")";
                command.ExecuteNonQuery();
            }
        }

        public bool PushEntries(LogEntry[] entries)
        {
            if (entries.Length == 0)
            {
                return true;
            }
            
            try
            {
                var con = CreateConnection();
                con.Open();
                var command = con.CreateCommand();
                
                foreach (var entry in entries)
                {
                    command.CommandText = "INSERT INTO log_entries(ip_address, http_method, http_location, http_code, requested_at, package_size) VALUES ";
                    command.CommandText += $"(\"{entry.IpAddress}\", \"{entry.HttpMethod}\", \"{entry.HttpLocation}\", \"{entry.HttpCode}\", \"{entry.RequestedAt}\", \"{entry.PackageSize}\")";
                    command.CommandText += ";";
                    command.ExecuteNonQuery();
                }

                return true;
            }
            catch
            {
                MessageBox.Show("The import action failed.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        public List<string> GetPushedLogs()
        {
            var con = CreateConnection();
            con.Open();
            var command = con.CreateCommand();
            command.CommandText = "SELECT filename FROM filenames";
            SqliteDataReader reader = command.ExecuteReader();
            List<string> logList = new List<string> { };
            while (reader.Read())
            {
                logList.Add(reader["filename"].ToString());
            }

            reader.Close();
            return logList;
        }
    }
}