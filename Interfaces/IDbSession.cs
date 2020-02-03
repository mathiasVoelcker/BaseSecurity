using System.Data.SqlClient;

namespace BaseSecurity.Interfaces
{
    public interface IDbSession
    {
        SqlConnection CreateConnection();
    }
}