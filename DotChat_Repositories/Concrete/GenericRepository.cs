using DotChat_Entities.DbSet;
using DotChat_Repositories.Abstract;
using DotChat_Repositories.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DotChat_Repositories.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext context;

        public GenericRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddAsync(T item)
        {
            try
            {

                context.Set<T>().Add(item);
                return await SaveAsync() > 0; //Bir tek nesne gelip ekleme işlemi yapıldığı için Save() metodundan 1 dönüyorsa buradan true dönsün.
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> AddAsync(List<T> items)
        {
            try
            {
                //Listeye ekleme işleminde herhangi bir hata olduğunda 
                //Using bloğu scope tamamlandıktan sorna imha ediliyor
                //TransactionScope: Bir işlem adımı varsa (birden fazla ekleme/Silme vs vs gibi işlem çeşidi barındırırsa) kullanılır
                using (TransactionScope scope = new TransactionScope())
                {
                    //context.Set<T>().AddRange(items); bütün olarak eklenecek listeyi ekler
                    foreach (var item in items)
                    {

                        context.Set<T>().Add(item);
                    }
                    scope.Complete(); //Bütün hepsi eklendiyse complete olacak(kaydedilecek) aksi halde hiç olmamış gibi davranıcak
                }
                return await SaveAsync() > 0;

            }
            catch (Exception)
            {

                return false;
            }
        }
        public async Task<bool> UpdateAsync(T item)
        {
            try
            {

                context.Set<T>().Update(item);
                return await SaveAsync() > 0; //Bir tek nesne gelip ekleme işlemi yapıldığı için Save() metodundan 1 dönüyorsa buradan true dönsün.
            }
            catch (Exception)
            {

                return false;
            }
        }
        public async Task<bool> RemoveAsync(T item)
        {
            try
            {

                context.Set<T>().Remove(item);
                return await SaveAsync() > 0; //Bir tek nesne gelip ekleme işlemi yapıldığı için Save() metodundan 1 dönüyorsa buradan true dönsün.
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> RemoveAsync(int id)
        {
            try
            {

                using (TransactionScope scope = new TransactionScope())
                {
                    T item = await GetByIdAsync(id);
                    scope.Complete();
                    return await RemoveAsync(item.Id);
                }


            }
            catch (Exception)
            {

                return false;
            }
        }
        public async Task<bool> RemoveAllAsync(Expression<Func<T, bool>> exp)
        {
            try
            {

                using (TransactionScope scope = new TransactionScope())
                {
                    var items = GetDefault(exp); // verilen ifadelere göre ilgili nesneleri items'a atıyoruz
                    int counter = 0;
                    foreach (var item in items)
                    {

                        bool opResult = await UpdateAsync(item); //Db den silmiyoruz durumunu InActive olarak işaretliyoruz. Bunuda update metodu ile gerçekleştiriyoruz. İşlem sonucunuda opResult'ta tutuyoruz. (Update true or false)
                        if (opResult)
                        {
                            counter++; //Eğer ilgili item güncellendiyse sayac 1 artar.
                        }
                    }
                    if (items.Count == counter)
                    {
                        scope.Complete();//Koleksiyondaki eleman sayısı ile silme işlemi gerçekleşen eleman sayısı eşit ise işlem tamamen başarılıdır.
                        return true;// başarılı olduğu için true döndür.
                    }
                    else
                    {
                        scope.Dispose();//Aksi halde bu scope dispose et
                        return false;//Başarısı olduğu için false döndür.
                    }


                }


            }
            catch (Exception)
            {

                return false;
            }
        }
        public async Task<T> GetByIdAsync(int id)
        {
           return context.Set<T>().Find(id); //tek satırlı metot yazma

        } 
        public async Task<IQueryable<T>> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            var query = context.Set<T>().Where(x => x.Id == id);
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }
        public async Task<T> GetByDefaultAsync(Expression<Func<T, bool>> exp)
        {
            return context.Set<T>().FirstOrDefault(exp);
        }

        public List<T> GetDefault(Expression<Func<T, bool>> exp)
        {
            return context.Set<T>().Where(exp).ToList();
        }

        //public List<T> GetActive()
        //{
        //    return context.Set<T>().Where(x => x.IsActive == true).ToList();
        //}


        //public IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes)
        //{
        //    var query = context.Set<T>().Where(x => x.IsActive == true);
        //    if (includes != null)
        //        query = includes.Aggregate(query, (current, include) => current.Include(include));
        //    return query;
        //}
        public async Task<List<T>> GetAllAsync()
        {
            return context.Set<T>().ToList();
        }
        public async Task<IQueryable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            var query = context.Set<T>().AsQueryable();
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }
        public async Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> exp, params Expression<Func<T, object>>[] includes)
        {
            var query = context.Set<T>().Where(exp);
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> exp)
        {
            return context.Set<T>().Any(exp);
        }
        //public bool Activate(int id)
        //{
        //    T item = GetById(id);
        //    item.IsActive = true;
        //    return Update(item);
        //}
        public async Task<int> SaveAsync()
        {
            return context.SaveChanges();
        }
        public void DetachEntity(T item)
        {
            context.Entry<T>(item).State = EntityState.Detached;// Bir entry'i (item) takip etmeyi bırakmak için kullanılır.
        }

        public Task<List<T>> GetDefaultAsync(Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }
    }
}
