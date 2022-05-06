using System.Data;

namespace WPF_MVVMLight_CRUD.DAL.Interface
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
