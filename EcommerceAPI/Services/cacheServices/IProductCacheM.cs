using EcommerceAPI.DTO;
using EcommerceAPI.Models;

namespace EcommerceAPI.Services.cacheServices
{
    public interface IProductCacheM
    {
        public Task<Product> GetProductFromCachedDataAsync(int key);
        public Task<ProductDTO> GetProductFromCachedDataRedisAsync(int id);
        public Task<bool> UpdateProductAsync(ProductDTO updatedProductDto);
    }
}
