namespace EcommerceAPI.Services.cacheServices
{
    public interface IRedisCache<T> where T : class
    {
        public  Task<T> getByIdAsync(int id);
        public  Task<T> setByIdAsync(int id, T entity);
    }
}
