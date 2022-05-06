using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WPF_MVVMLight_CRUD.DAL.Interface;
using WPF_MVVMLight_CRUD.Model;

namespace WPF_MVVMLight_CRUD.DAL.Implement
{
    public sealed class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(IDbTransaction transaction) : base(transaction)
        {

        }

        public override void Add(Department entity)
        {
            Connection.Execute(@"INSERT INTO Departments (DeptNo, Dname, Location) 
                                      VALUES (@DeptNo, @Dname, @Location)", entity);
        }

        public override int Create(Department entity)
        {
            Console.WriteLine("Create Start");
            var sql = @"INSERT INTO Departments 
                        (
                            [DeptNo]
                           ,[Dname]
                           ,[Location]
                        ) 
                        VALUES 
                        (
                            @DeptNo   
                           ,@Dname
                           ,@Location
                        );

                        SELECT @@IDENTITY;
                    ";
            return Connection.QuerySingle<int>(sql, entity, Transaction);
        }

        public override void Delete(Department entity)
        {
            throw new NotImplementedException();
        }

        public override Department Get(int Id)
        {
            return Connection.Query<Department>("SELET * FROM Departments where DeptNo = @id", new { id = Id }).SingleOrDefault();
        }

        public override IEnumerable<Department> GetAll()
        {
            return Connection.Query<Department>("SELECT * FROM Departments ", null, Transaction);
        }

        public override void Update(Department entity)
        {
            throw new NotImplementedException();
        }
    }
}
