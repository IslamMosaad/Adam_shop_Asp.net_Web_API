using EcommerceAPI.Services.cacheModels;
using EcommerceAPI.Services.cacheServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IRedisCache<Dish> RedisCache;

        public DishController(IRedisCache<Dish> _RedisCache)
        {
            RedisCache = _RedisCache;
        }


        // GET: api/Dish/5
        [HttpGet("{id:int}")]
        [Produces("application/json")]
        public async Task<ActionResult> GetDish(int id)
        {
            //var dish = await Repo.GetByIdAsync(id);
            var dish = await RedisCache.getByIdAsync(id);
            if (dish == null) return NotFound();

            return Ok(dish);
        }


        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult> AddDish(Dish dish)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                await RedisCache.setByIdAsync(dish.id,dish);
               
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
            return Created();
        }
    }
}
