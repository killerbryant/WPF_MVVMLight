using System.Collections.ObjectModel;
using WPF_MVVMLight_CRUD.DAL.Interface;
using WPF_MVVMLight_CRUD.Intercept;
using WPF_MVVMLight_CRUD.Model;
using WPF_MVVMLight_CRUD.Services.Interface;

namespace WPF_MVVMLight_CRUD.Services.Implement
{
    /// <summary>
    /// Class implementing IDataAccessService interface and implementing
    /// its methods by making call to the Entities using CompanyEntities object
    /// </summary>
    public class DataAccessService : IDataAccessService
    {
        IUnitOfWork _UnitOfWork;
        public DataAccessService(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }
        public ObservableCollection<EmployeeInfo> GetEmployees()
        {
            var emps = _UnitOfWork.EmployeeInfoRepository.GetAll();
            
            ObservableCollection<EmployeeInfo> Employees = new ObservableCollection<EmployeeInfo>();
            
            foreach (var item in emps)
            {
                Employees.Add(item);
            }

            return Employees;
        }

        /// <summary>
        /// 新增EmployeeInfo
        /// </summary>
        /// <param name="Emp">參數</param>
        /// <returns></returns>
        [LoggingMethod]
        public int CreateOneEmployee(EmployeeInfo emp)
        {
            int result = _UnitOfWork.EmployeeInfoRepository.Create(emp);

            _UnitOfWork.Commit();
            return result;
        }
    }
}
