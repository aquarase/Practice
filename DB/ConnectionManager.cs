using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgApp.Database
{
    internal class ConnectionManager
    {
        private readonly string _connectionString;

        public ConnectionManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task ExecuteWithConnectionAsync(Func<SqlConnection, Task> action)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await action(connection);
            }
        }

        public void PrintConnectionStatistics()
        {
            var pool = new SqlConnectionStringBuilder(_connectionString);
            Console.WriteLine($"Pooling: {pool.Pooling}");
            Console.WriteLine($"Max Pool Size: {pool.MaxPoolSize}");
            Console.WriteLine($"Min Pool Size: {pool.MinPoolSize}");
        }
    }
}
