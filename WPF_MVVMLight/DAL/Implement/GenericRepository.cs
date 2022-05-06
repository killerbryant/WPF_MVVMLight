using System;
using System.Collections.Generic;
using System.Data;
using WPF_MVVMLight_CRUD.DAL.Interface;

namespace WPF_MVVMLight_CRUD.DAL
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected IDbTransaction Transaction { get; }
        protected IDbConnection Connection => Transaction.Connection;

        public GenericRepository(IDbTransaction transaction)
        {
            Transaction = transaction;
        }

        public virtual void Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual int Create(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual TEntity Get(int Id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public virtual void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
