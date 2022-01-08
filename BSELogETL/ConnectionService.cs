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
            // using SqliteConnection connection1 = new SqliteConnection("Data Source=identifier.sqlite");
            var dataSource = Path.GetFullPath(Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "..\\..\\database\\identifier.sqlite")
            );

            return new SqliteConnection("Data Source=" + dataSource);
        }

        public void PushFilenames(string[] files)
        {
            var con = CreateConnection();
            con.Open();
            var command = con.CreateCommand();
            foreach (var file in files)
            {
                command.CommandText = $"INSERT INTO filenames(filename) values ({file})";
                command.ExecuteNonQuery();
            }
        }

        public bool PushEntries(LogEntry[] entries)
        {
            bool result = false;
            try
            {
                var con = CreateConnection();
                con.Open();
                var command = con.CreateCommand();
                foreach (var entry in entries)
                {
                    command.CommandText =
                        $"INSERT INTO log_entries(ip_adress, http_method, http_location, http_code, requested_at, package_size) values({entry.IpAddress}, {entry.HttpMethod}, {entry.HttpLocation}, {entry.HttpCode}, {entry.RequestedAt}, {entry.PackageSize})";
                    command.ExecuteNonQuery();
                }

                result = true;
            }
            catch
            {
                MessageBox.Show("the import action failed", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
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