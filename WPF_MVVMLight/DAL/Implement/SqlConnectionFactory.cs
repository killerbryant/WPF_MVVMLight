using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using WPF_MVVMLight_CRUD.DAL.Interface;

namespace WPF_MVVMLight_CRUD.DAL.Implement
{
    public class SqlConnectionFactory : IConnectionFactory
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public IDbConnection CreateConnection() => new SqlConnection(connectionString);
    }
}
