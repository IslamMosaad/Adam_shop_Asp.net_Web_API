using EcommerceAPI.Models;

namespace EcommerceAPI.Services
{
    public interface IProductCacheM
    {
        public Task<Product> GetProductFromCachedDataAsync(int key);
    }
}
