using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;

namespace SnackUpAPI.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public IEnumerable<T> Query<T>(string sql, object? parameters = null)
        {
            using (var connection = CreateConnection())
            {
                return connection.Query<T>(sql, parameters);
            }
        }

        public T QuerySingle<T>(string sql, object? parameters = null)
        {
            using (var connection = CreateConnection())
            {
                return connection.QuerySingle<T>(sql, parameters);
            }
        }
        public async Task<T> QuerySingleAsync<T>(string sql, object parameters = null)
        {
            using (var connection = CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<T>(sql, parameters);
            }
        }

        public int Execute(string sql, object? parameters = null)
        {
            using (var connection = CreateConnection())
            {
                return connection.Execute(sql, parameters);
            }
        }
        public T ExecuteScalar<T>(string query, object parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters.GetType().GetProperties())
                        {
                            command.Parameters.AddWithValue($"@{param.Name}", param.GetValue(parameters));
                        }
                    }

                    var result = command.ExecuteScalar();
                    return result == null || result == DBNull.Value ? default : (T)result;
                }
            }
        }
    }
}
