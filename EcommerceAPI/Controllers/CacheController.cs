using EcommerceAPI.Services.cacheModels;
using EcommerceAPI.Services.cacheServices.sharedCache;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        [HttpPost("write")]
        public IActionResult WriteCache([FromBody] Dish data)
        {
            MemoryMappedFileHelper<Dish>.WriteCacheData(data);
            return Ok("Data written to memory-mapped file.");
        }

        [HttpGet("read")]
        public IActionResult ReadCache()
        {
            var data = MemoryMappedFileHelper<Dish>.ReadCacheData();
            return Ok(data);
        }
    }
}
