using EcommerceAPI.DTO;
using EcommerceAPI.Models;

namespace EcommerceAPI.Services
{
    public interface IProductCacheM
    {
        public Task<Product> GetProductFromCachedDataAsync(int key);
        public  Task<ProductDTO> GetProductFromCachedDataRedisAsync(int id);
    }
}
