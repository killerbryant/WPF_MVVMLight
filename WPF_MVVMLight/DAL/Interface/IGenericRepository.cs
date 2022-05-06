using System.Collections.Generic;

namespace WPF_MVVMLight_CRUD.DAL.Interface
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        TEntity Get(int Id);
        IEnumerable<TEntity> GetAll();
        void Add(TEntity entity);
        int Create(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
    }
}
