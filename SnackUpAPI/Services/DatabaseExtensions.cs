using System.Linq;
using System.Collections.Generic;

namespace SnackUpAPI.Services
{
	public static class DatabaseExtensions
	{
		public static T QuerySingleOrDefault<T>(this DatabaseService dbService, string query, object? parameters = null)
		{
			var result = dbService.Query<T>(query, parameters);

			// Usa 'default' per tipi valore, null per tipi riferimento.
			return result.SingleOrDefault() ?? default!;
		}
	}
}
