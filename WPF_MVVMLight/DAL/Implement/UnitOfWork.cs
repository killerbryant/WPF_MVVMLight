using System;
using System.Data;
using WPF_MVVMLight_CRUD.DAL.Interface;

namespace WPF_MVVMLight_CRUD.DAL.Implement
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        private IEmployeeInfoRepository _employeeInfoRepository;

        private IDepartmentRepository _departmentRepository;

        public IEmployeeInfoRepository EmployeeInfoRepository => _employeeInfoRepository ?? (_employeeInfoRepository = new EmployeeInfoRepository(_transaction));

        public IDepartmentRepository DepartmentRepository => _departmentRepository ?? (_departmentRepository = new DepartmentRepository(_transaction));

        public UnitOfWork(IConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.CreateConnection();
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                //throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                ResetRepositories();
            }
        }

        private void ResetRepositories()
        {
            _employeeInfoRepository = null;
            _departmentRepository = null;
        }

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
