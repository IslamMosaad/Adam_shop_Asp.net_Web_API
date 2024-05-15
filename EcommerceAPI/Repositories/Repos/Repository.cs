using EcommerceAPI.Repositories.Interfaces;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Repositories.Repos
{
    public class Repository<T> : IRepository<T> where T : class
    {
        ECommerceDBContext context;
        public Repository(ECommerceDBContext _context)
        {
            context = _context;
        }

        //Allow pagination
        public async Task<List<T>> GetAllAsync(int page = 0, int pageSize = 0)
        {
            if (page > 0 && pageSize > 0) // if he send the parameters --> send pagination
            {
                return context.Set<T>().Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
            return context.Set<T>().ToList();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return context.Set<T>().Find(id);
        }

        public async Task<bool> IsExistAsync(int id)
        {
            var entity = context.Set<T>().Find(id);
            if (entity == null) return false;
            context.Entry(entity).State = EntityState.Detached;
            return true;
        }

        public async Task InsertAsync(T entity)
        {
            context.Set<T>().Add(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public async Task DeleteAsync(int id)
        {
            T entity = await GetByIdAsync(id);
            if (entity != null)
            {
                context.Set<T>().Remove(entity);
            }

        }
        public async Task DeleteAsync(T entity)
        {
            if (entity != null)
            {
                context.Set<T>().Remove(entity);
            }

        }

        public async Task SaveChangesAsync()
        {
            context.SaveChanges();
        }

        public void ChangeStateToModified(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }
        public void ChangeStateToDetached(T entity)
        {
            context.Entry(entity).State = EntityState.Detached;
        }

        public async Task<T> GetEntityAsync(Func<T, bool> filter)
        {
            return context.Set<T>().FirstOrDefault(filter);
        }
    }

}
