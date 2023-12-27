using DotChat_Entities.DbSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_Repositories.Abstract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
       Task<bool> AddAsync(T item);

       Task<bool> AddAsync(List<T> items);

        Task<bool> UpdateAsync(T item);

        Task<bool> RemoveAsync(T item);

        Task<bool> RemoveAsync(int id);

        Task<bool> RemoveAllAsync(Expression<Func<T, bool>> exp);

        Task<T> GetByIdAsync(int id);

        Task<IQueryable<T>> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);

        Task<T> GetByDefaultAsync(Expression<Func<T, bool>> exp);

        Task<List<T>> GetDefaultAsync(Expression<Func<T, bool>> exp);

        //List<T> GetActive();

        //IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes);

        Task<List<T>> GetAllAsync();

        Task<IQueryable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);

        Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> exp, params Expression<Func<T, object>>[] includes);

        Task<bool> AnyAsync(Expression<Func<T, bool>> exp);

        //bool Activate(int id);

        Task<int> SaveAsync();

        void DetachEntity(T item);
    }
}
