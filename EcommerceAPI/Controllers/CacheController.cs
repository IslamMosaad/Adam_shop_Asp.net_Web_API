using EcommerceAPI.Services.cacheModels;
using EcommerceAPI.Services.cacheServices.sharedCache;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        [HttpPost("write/{minutes:int}")]
        public IActionResult WriteCache([FromBody] Dish data, int minutes)
        {
            if (data == null || string.IsNullOrEmpty(data.id.ToString()))
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                MemoryMappedFileCache<Dish>.Set(data.id.ToString(), data, TimeSpan.FromMinutes(minutes));
                return Ok("Data written to memory-mapped file.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("read/{id:int}")]
        public IActionResult ReadCache(int id)
        {
            try
            {
                var data = MemoryMappedFileCache<Dish>.Get(id.ToString());
                if (data == null)
                {
                    return NotFound("Data not found or expired.");
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
