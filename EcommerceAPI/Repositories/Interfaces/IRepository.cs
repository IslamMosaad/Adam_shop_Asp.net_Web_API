namespace EcommerceAPI.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(int page = 0, int pageSize = 0);
        Task<T> GetByIdAsync(int id);
        Task<bool> IsExistAsync(int id);
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task DeleteAsync(T entity);
        Task SaveChangesAsync();
        void ChangeStateToModified(T entity);
        void ChangeStateToDetached(T entity);
        Task<T> GetEntityAsync(Func<T, bool> filter);
    }
}
