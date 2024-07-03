using EcommerceAPI.DTO;
using EcommerceAPI.Models;
using EcommerceAPI.Unit_OF_Work;
using Microsoft.Extensions.Caching.Memory;

namespace EcommerceAPI.Services
{
    public class ProductCacheM: IProductCacheM
    {
        private readonly IMemoryCache _cache;
        private readonly IUnitOfWork<Product> _ProductUnit;

        public ProductCacheM(IMemoryCache cache, IUnitOfWork<Product> ProductUnit)
        {
            _cache = cache;
            _ProductUnit = ProductUnit;
        }

        public async Task<Product> GetProductFromCachedDataAsync(int id)
        {
            string key =id.ToString();
            if (!_cache.TryGetValue(key, out Product value))
            {
                // Data is not in cache, so fetch or compute the data
                var product = await _ProductUnit.Repository.GetByIdAsync(id);
                value = product;

                // Set the data in cache with a 1-minute expiration
                _cache.Set(key, value, TimeSpan.FromMinutes(5));
            }

            // Return the cached data
          
            return value;
        }


        //public async Task<Product> GetProductFromCachedDataAsync(int id)
        //{
        //    string key = id.ToString();
        //    if (!_cache.TryGetValue(key, out Product value))
        //    {
        //        // Handling concurrency to avoid race conditions in cache updates
        //        // We can use a double-checked lock with a Lazy<Task<T>> to properly handle async locks

        //        var lazyProduct = new Lazy<Task<Product>>(async () =>
        //        {
        //            // Data is not in cache, so fetch or compute the data
        //            var product = await _ProductUnit.Repository.GetByIdAsync(id);
        //            _cache.Set(key, product, TimeSpan.FromMinutes(5));
        //            return product;
        //        });

        //        // Ensure only one Lazy<Task<T>> is created and used for the key
        //        var actualProduct = _cache.GetOrCreate(key, entry => lazyProduct);

        //        // Await the result of the Lazy<Task<T>>
        //        value = await actualProduct.Value;
        //    }
        //    // Return the cached data
        //    return value;
        //}


    }
}
