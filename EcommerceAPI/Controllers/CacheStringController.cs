using EcommerceAPI.Services.cacheServices.sharedCache;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheStringController : ControllerBase
    {
        [HttpPost("write/{minutes:int}")]
        public IActionResult WriteCache([FromQuery] string key, [FromQuery] string value, int minutes)
        {
            MemoryMappedFileString.Set(key, value, TimeSpan.FromMinutes(minutes));
            return Ok("Data written to memory-mapped file.");
        }

        [HttpGet("read/{key}")]
        public IActionResult ReadCache(string key)
        {
            var value = MemoryMappedFileString.Get(key);
            return Ok(value);
        }
    }
}
