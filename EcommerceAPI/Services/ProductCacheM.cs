using AutoMapper;
using EcommerceAPI.DTO;
using EcommerceAPI.Models;
using EcommerceAPI.Unit_OF_Work;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace EcommerceAPI.Services
{
    public class ProductCacheM: IProductCacheM
    {
        private readonly IMemoryCache _cache;
        private readonly IDistributedCache _redisCache;
        private readonly IUnitOfWork<Product> _ProductUnit;
        private readonly IMapper _Mapper;
        public ProductCacheM(IMemoryCache cache, IDistributedCache redisCache, 
            IUnitOfWork<Product> ProductUnit, IMapper Mapper)
        {
            _cache = cache;
            _ProductUnit = ProductUnit;
            _redisCache = redisCache;
            _Mapper = Mapper;
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

        #region in memory caching , handling concurrency 
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
        #endregion


        public async Task<ProductDTO> GetProductFromCachedDataRedisAsync(int id)
        {
            string key = id.ToString();

            var productString = await _redisCache.GetStringAsync(key);
            if (productString is not null)
            {
                // Deserialize the product from JSON string
                var productFromCache = JsonSerializer.Deserialize<ProductDTO>(productString);
                return productFromCache;
            }

            // Data is not in cache, so fetch or compute the data
            Product product = await _ProductUnit.Repository.GetByIdAsync(id);

            #region because of using lazy loading i should avoid A possible object cycle error
            ProductDTO productDTO = _Mapper.Map<ProductDTO>(product);
            #endregion

            if (productDTO != null)
            {
                // Serialize the product to JSON string
                var productJson = JsonSerializer.Serialize(productDTO);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    // Set the absolute expiration time
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };
                // Set the data in cache with the expiration options
                await _redisCache.SetStringAsync(key, productJson, cacheEntryOptions);
            }
            // Return the product
            return productDTO;
        }


        public async Task<bool> UpdateProductAsync(ProductDTO updatedProductDto)
        {
            string key = updatedProductDto.id.ToString();

            // Update the product in the database
            Product updatedProduct = _Mapper.Map<Product>(updatedProductDto);
            bool isUpdated = await _ProductUnit.Repository.UpdateAsync(updatedProduct);

            if (isUpdated)
            {
                // Invalidate the cache
                await _redisCache.RemoveAsync(key);

                // Optionally, you can update the cache with the new product data
                var productJson = JsonSerializer.Serialize(updatedProductDto);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    // Set the absolute expiration time
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };
                await _redisCache.SetStringAsync(key, productJson, cacheEntryOptions);
            }

            return isUpdated;
        }

    }
}
