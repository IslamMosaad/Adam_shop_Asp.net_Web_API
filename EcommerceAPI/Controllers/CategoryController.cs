using AutoMapper;
using EcommerceAPI.DTO;
using EcommerceAPI.Models;
using EcommerceAPI.Unit_OF_Work;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EcommerceAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly IUnitOfWork<Category> _CategorytUnit;
        private readonly IMapper _Mapper;

        public CategoryController(IUnitOfWork<Category> Unit, IMapper Mapper)
        {
            _CategorytUnit = Unit;
            _Mapper = Mapper;
        }


        // GET: api/Category
        // [Authorize]
        [HttpGet]
        [Produces("application/json")] //to send data in this format only 
        public async Task<ActionResult> GetCategories()
        {
            List<Category> categories = await _CategorytUnit.Repository.GetAllAsync();
            //Mapping
            List<CategoryDTO> categoryDTOs = _Mapper.Map<List<CategoryDTO>>(categories);
 
            return Ok(categoryDTOs);
        }

        // GET: api/Category/5
        [HttpGet("{id:int}", Name = "GetCategoryByIdRoute")]
        [Produces("application/json")] //to send data in this format only 
        public async Task<ActionResult> GetCategory(int id)
        {
            var category = await _CategorytUnit.Repository.GetByIdAsync(id);
            if (category == null) return NotFound();
            CategoryDTO categoryDTO = _Mapper.Map<CategoryDTO>(category);
            return Ok(categoryDTO);
        }


        // POST: api/Category
        [HttpPost]
        [Consumes("application/json")] 
        public async Task<ActionResult> PostCategory(CategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid) return BadRequest();
            Category category = _Mapper.Map<Category>(categoryDTO);

           //UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _CategorytUnit.Repository.InsertAsync(category);
            await _CategorytUnit.Repository.SaveChangesAsync();


            string url = Url.Link("GetCategoryByIdRoute", new { id = category.Id });

            categoryDTO.id = category.Id;

            return Created(url, categoryDTO);
        }

        // PUT: api/categorys/5
        [HttpPut("{id}")]
        [Consumes("application/json")] 
        public async Task<IActionResult> Putcategory(int id, CategoryDTO categoryDTO)
        {
            bool isExist = await _CategorytUnit.Repository.IsExistAsync(id);
            if (id != categoryDTO.id || !isExist) return BadRequest();
            Category category = _Mapper.Map<Category>(categoryDTO);
            _CategorytUnit.Repository.ChangeStateToModified(category);
            try
            {
                await _CategorytUnit.Repository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }



        // DELETE: api/category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletecategoryDTO(int id)
        {
            if (!(await _CategorytUnit.Repository.IsExistAsync(id))) return NotFound();
            await _CategorytUnit.Repository.DeleteAsync(id);
            await _CategorytUnit.Repository.SaveChangesAsync();
            return NoContent();
        }


    }
}
