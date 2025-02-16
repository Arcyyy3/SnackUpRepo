using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnackUpAPI.Data
{
    public interface IDatabaseService
    {
        IEnumerable<T> Query<T>(string sql, object? parameters = null);
        T QuerySingle<T>(string sql, object? parameters = null);
        Task<T> QuerySingleAsync<T>(string sql, object? parameters = null);
        int Execute(string sql, object? parameters = null);
        T ExecuteScalar<T>(string query, object? parameters = null);
        T QuerySingleOrDefault<T>(string query, object? parameters = null);
    }
}
