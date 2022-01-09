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
                
                var transaction = con.BeginTransaction();
                
                var command = con.CreateCommand();
                command.CommandText = @"INSERT INTO log_entries(ip_address, http_method, http_location, http_code, requested_at, package_size) VALUES ($ip_address, $http_method, $http_location, $http_code, $requested_at, $package_size);";

                var ipAddressParameter = command.CreateParameter();
                ipAddressParameter.ParameterName = "$ip_address";
                command.Parameters.Add(ipAddressParameter);
                
                var httpMethodParameter = command.CreateParameter();
                httpMethodParameter.ParameterName = "$http_method";
                command.Parameters.Add(httpMethodParameter);
                
                var httpLocationParameter = command.CreateParameter();
                httpLocationParameter.ParameterName = "$http_location";
                command.Parameters.Add(httpLocationParameter);
                
                var httpCodeParameter = command.CreateParameter();
                httpCodeParameter.ParameterName = "$http_code";
                command.Parameters.Add(httpCodeParameter);
                
                var requestedAtParameter = command.CreateParameter();
                requestedAtParameter.ParameterName = "$requested_at";
                command.Parameters.Add(requestedAtParameter);
                
                var packageSizeParameter = command.CreateParameter();
                packageSizeParameter.ParameterName = "$package_size";
                command.Parameters.Add(packageSizeParameter);
                
                foreach (var entry in entries)
                {
                    ipAddressParameter.Value = entry.IpAddress;
                    httpMethodParameter.Value = entry.HttpMethod;
                    httpLocationParameter.Value = entry.HttpLocation;
                    httpCodeParameter.Value = entry.HttpCode;
                    requestedAtParameter.Value = entry.RequestedAt;
                    packageSizeParameter.Value = entry.PackageSize is null ? DBNull.Value : entry.PackageSize;
                    command.ExecuteNonQuery();
                }
                
                transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error",
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

        public List<LogEntry> QueryToEntries(string query)
        {
            var con = CreateConnection();
            con.Open();
            var command = con.CreateCommand();
            command.CommandText = query;
            
            var reader = command.ExecuteReader();
            var list = new List<LogEntry>();
            
            while (reader.Read())
            {
                var entry = new LogEntry
                {
                    IpAddress = reader["ip_address"].ToString(),
                    HttpMethod = reader["http_method"].ToString(),
                    HttpCode = reader["http_code"].ToString(),
                    HttpLocation = reader["http_location"].ToString(),
                    RequestedAt = reader["requested_at"].ToString(),
                    PackageSize = reader["package_size"].ToString()
                };

                list.Add(entry);
            }

            reader.Close();
            return list;
        }
    }
}