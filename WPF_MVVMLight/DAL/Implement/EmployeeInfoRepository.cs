using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WPF_MVVMLight_CRUD.DAL.Interface;
using WPF_MVVMLight_CRUD.Model;

namespace WPF_MVVMLight_CRUD.DAL.Implement
{
    public sealed class EmployeeInfoRepository : GenericRepository<EmployeeInfo>, IEmployeeInfoRepository
    {
        public EmployeeInfoRepository(IDbTransaction transaction) : base(transaction)
        {

        }

        public override void Add(EmployeeInfo entity)
        {
            Connection.Execute(@"INSERT INTO EmployeeInfo (EmpNo, EmpName, Salary, DeptName, Designation) 
                                      VALUES (@EmpNo, @EmpName, @Salary, @DeptName, @Designation)", entity);
        }

        public override int Create(EmployeeInfo entity)
        {
            var sql = @"INSERT INTO EmployeeInfo 
                        (
                            [EmpName]
                           ,[Salary]
                           ,[DeptName]
                           ,[Designation]
                        ) 
                        VALUES 
                        (
                            @EmpName
                           ,@Salary
                           ,@DeptName
                           ,@Designation
                        );

                        SELECT @@IDENTITY;
                    ";
            return Connection.QuerySingle<int>(sql, entity, Transaction);
        }

        public override void Delete(EmployeeInfo entity)
        {
            throw new NotImplementedException();
        }

        public override EmployeeInfo Get(int Id)
        {
            return Connection.Query<EmployeeInfo>("SELET * FROM EmployeeInfo where EmpNO = @id", new { id = Id }).SingleOrDefault();
        }

        public override IEnumerable<EmployeeInfo> GetAll()
        {
            return Connection.Query<EmployeeInfo>("SELECT * FROM EmployeeInfo ", null, Transaction);
        }

        public override void Update(EmployeeInfo entity)
        {
            throw new NotImplementedException();
        }
    }
}
