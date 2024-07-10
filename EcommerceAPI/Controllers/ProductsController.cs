using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerceAPI.DTO;
using EcommerceAPI.Models;
using EcommerceAPI.Unit_OF_Work;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EcommerceAPI.Services.cacheServices;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork<Product> _ProductUnit;
        private readonly IMapper _Mapper;
        private readonly IProductCacheM _productCacheM;


        public ProductsController(IUnitOfWork<Product> ProductUnit,IMapper Mapper, IProductCacheM productCacheM)
        {
            _ProductUnit = ProductUnit;
            _Mapper = Mapper;
            _productCacheM = productCacheM;
        }

        // GET: api/Products
       // [Authorize]
        [HttpGet]
        [Produces("application/json")] //to send data in this format only 
        public async Task<ActionResult> GetProducts([FromQuery] int page = 0, [FromQuery] int pageSize = 0)
        {
            List<Product> products = await _ProductUnit.Repository.GetAllAsync(page, pageSize);
            //Mapping
            List<ProductDTO> productDTOs = _Mapper.Map<List<ProductDTO>>(products);
            /*
            //List<ProductDTO> productDTOs = new List<ProductDTO>();
            //foreach (Product product in products)
            //{
            //    ProductDTO productDTO = new ProductDTO()
            //    {
            //        id = product.Id,
            //        title = product.Title,
            //        description = product.Description,
            //        img = product.Img,
            //        price = product.Price,
            //        stock = product.Stock,
            //        Product_name = product.Product?.Title,
            //        user_name = product.User?.UserName

            //    };
            //    productDTOs.Add(productDTO);
            //}*/

            return Ok(productDTOs);
        }

        // GET: api/Products/5
        [HttpGet("{id:int}", Name = "GetProductByIdRoute")]
        [Produces("application/json")] //to send data in this format only 
        public async Task<ActionResult> GetProduct(int id)
        {
            //normal way
            //var product = await _ProductUnit.Repository.GetByIdAsync(id);

            //using in memory cache ( built in service  AddMemoryCache())
            //ar product = await _productCacheM.GetProductFromCachedDataAsync(id);

            //using redis distributed service (AddStackExchangeRedisCache)
            var product = await _productCacheM.GetProductFromCachedDataRedisAsync(id);
            if (product == null)return NotFound();
            ProductDTO productDTO = _Mapper.Map<ProductDTO>(product);
            return Ok(productDTO);
        }


        // POST: api/Products
        [Authorize]
        [HttpPost]
        [Consumes("application/json")] //to accept only this type to put in the request body
        public async Task<ActionResult> PostProduct(ProductDTO productDTO)
        {
            if (!ModelState.IsValid) return BadRequest();
            Product product = _Mapper.Map<Product>(productDTO);

            product.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _ProductUnit.Repository.InsertAsync(product);
            await _ProductUnit.Repository.SaveChangesAsync();


            string url = Url.Link("GetProductByIdRoute", new { id = product.Id });

            productDTO.userID = product.UserID;
            productDTO.category_name = product?.Category?.Title;
            return Created(url, productDTO);
        }


        // PUT: api/Products/5
        [Authorize]
        [HttpPut("{id}")]
        [Consumes("application/json")] //to accept only this type to put in the request body
        public async Task<IActionResult> PutProduct(int id, ProductDTO productDTO)
        {
            bool isExist = await _ProductUnit.Repository.IsExistAsync(id);
            if (id != productDTO.id || !isExist)return BadRequest();
            Product product = _Mapper.Map<Product>(productDTO);
            product.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _ProductUnit.Repository.ChangeStateToModified(product);
            try
            {
               
                await _ProductUnit.Repository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }



        // DELETE: api/Products/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductDTO(int id)
        {
            if (!(await _ProductUnit.Repository.IsExistAsync(id)))return NotFound();
            await _ProductUnit.Repository.DeleteAsync(id);
            await _ProductUnit.Repository.SaveChangesAsync();
            return NoContent();
        }


    }
}
