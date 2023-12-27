using DotChat_Entities.DbSet;
using DotChat_Repositories.Abstract;
using DotChat_Services.Abstact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DotChat_Services.Concrete
{
    public class GenericService<T> : IGenericService<T> where T : BaseEntity
    {
        private readonly IGenericRepository<T> repository;

        public GenericService(IGenericRepository<T> repository)
        {
            this.repository = repository;
        }

        //public bool Activate(int id)
        //{
        //    if (id == 0 || GetById(id) == null)
        //        return false;
        //    else
        //        return repository.Activate(id);
        //}

        public async Task<bool> AddAsync(T item)
        {
            try
            {
                return await repository.AddAsync(item);
            }
            catch (Exception ex)
            {

                throw new Exception($"Hata {ex.Message}");
            }
            
        }

        public async Task<bool> AddAsync(List<T> items)
        {
            return await repository.AddAsync(items);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> exp)
        {
            if (exp != null)
                return await repository.AnyAsync(exp);
            else
                return false;
        }

        //public List<T> GetActive()
        //{
        //    return repository.GetActive();
        //}

        //public IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes)
        //{
        //    return repository.GetActive(includes);
        //}

        public async Task<List<T>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<IQueryable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            return await repository.GetAllAsync(includes);
        }

        public async Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> exp, params Expression<Func<T, object>>[] includes)
        {
            return await repository.GetAllAsync(exp, includes);
        }

        public async Task<T> GetByDefaultAsync(Expression<Func<T, bool>> exp)
        {
            return await repository.GetByDefaultAsync(exp);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<IQueryable<T>> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            return await repository.GetByIdAsync(id, includes);
        }

        public async Task<List<T>> GetDefaultAsync(Expression<Func<T, bool>> exp)
        {
            return await repository.GetDefaultAsync(exp);
        }

        public async Task<bool> RemoveAsync(T item)
        {
            if (item == null)
                return false;
            else
                return await repository.RemoveAsync(item);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            if (id <= 0)
                return false;
            else
                return await repository.RemoveAsync(id);
        }

        public async Task<bool> RemoveAllAsync(Expression<Func<T, bool>> exp)
        {
            return await repository.RemoveAllAsync(exp);
        }

        public async Task<bool> UpdateAsync(T item)
        {

            if (item == null)
                return false;
            else
                return await repository.UpdateAsync(item);
        }
    }
}

