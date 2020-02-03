using System.Data.SqlClient;
using BaseSecurity.Interfaces;

namespace BaseSecurity.Db
{
    public class DbSession : IDbSession
    {
        private string _connectionString;

        public DbSession(string connectionString) {
            _connectionString = connectionString;
        }

        public SqlConnection CreateConnection() {
            return new SqlConnection(_connectionString);
        }
    }
}