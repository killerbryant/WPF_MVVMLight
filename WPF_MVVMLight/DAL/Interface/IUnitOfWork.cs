using System;

namespace WPF_MVVMLight_CRUD.DAL.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IEmployeeInfoRepository EmployeeInfoRepository { get; }

        IDepartmentRepository DepartmentRepository { get; }
        
        void Commit();
    }
}
