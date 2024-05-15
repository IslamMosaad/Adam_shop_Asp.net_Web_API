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
    public class CartItemController : ControllerBase
    {

        private readonly IUnitOfWork<CartItem> _cartItemUnit;
        private readonly IMapper _Mapper;

        public CartItemController(IUnitOfWork<CartItem> Unit, IMapper Mapper)
        {
            _cartItemUnit = Unit;
            _Mapper = Mapper;
        }


        // GET: api/cartItem
        // [Authorize]
        [HttpGet]
        [Produces("application/json")] //to send data in this format only 
        public async Task<ActionResult> GetCartItems()
        {
            List<CartItem> cartItems = await _cartItemUnit.Repository.GetAllAsync();
            //Mapping
            List<CartItemDTO> cartItemsDTO = _Mapper.Map<List<CartItemDTO>>(cartItems);

            return Ok(cartItemsDTO);
        }



        // GET: api/cartItem/5
        [HttpGet("{product_id:int}", Name = "GetCartItemByIdRoute")]
        [Produces("application/json")] //to send data in this format only 
        public async Task<ActionResult> GetcartItem(int product_id)
        {
            string user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CartItem cartItem = await _cartItemUnit.Repository.GetEntityAsync(c=>c.UserID == user_id&&c.ProductID==product_id);
            if (cartItem == null) return NotFound();
            CartItemDTO cartItemDTO = _Mapper.Map<CartItemDTO>(cartItem);
            return Ok(cartItemDTO);
        }



        // POST: api/cartItem
        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult> PostcartItem(CartItemDTO cartItemDTO)
        {
            cartItemDTO.user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid) return BadRequest();
            CartItem cartItem = _Mapper.Map<CartItem>(cartItemDTO);

            CartItem oldItem= await _cartItemUnit.Repository.GetEntityAsync(c=>c.UserID==cartItem.UserID&&c.ProductID==cartItem.ProductID);
            if(oldItem!=null)//update only the quantity
            {
                oldItem.Quantity = cartItem.Quantity;
                _cartItemUnit.Repository.ChangeStateToModified(oldItem);
                try
                {
                    await _cartItemUnit.Repository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return BadRequest(ex.Message);
                }
                return NoContent();
            }
            //Add new CartItem

            await _cartItemUnit.Repository.InsertAsync(cartItem);
            await _cartItemUnit.Repository.SaveChangesAsync();


            string url = Url.Link("GetcartItemByIdRoute", new { id = cartItem.Id });

            cartItemDTO.id = cartItem.Id;

            return Created(url, cartItemDTO);
        }



        // PUT: api/cartItems
        [HttpPut]
        [Consumes("application/json")]
        public async Task<IActionResult> PutcartItem(List<CartItemDTO> cartItemsDTO)
        {
            string user_id= User.FindFirstValue(ClaimTypes.NameIdentifier);
            foreach (CartItemDTO cartItemDTO in cartItemsDTO)
            {
                if (!ModelState.IsValid) return BadRequest();
                CartItem oldItem = await _cartItemUnit.Repository.GetEntityAsync(c => c.UserID == cartItemDTO.user_id && c.ProductID == cartItemDTO.product_id);
                if (oldItem != null)//update only the quantity
                {
                    oldItem.Quantity = cartItemDTO.quantity;
                    _cartItemUnit.Repository.ChangeStateToModified(oldItem);
                    try
                    {
                        await _cartItemUnit.Repository.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        return BadRequest(ex.Message);
                    }
                   
                }
            }
            return NoContent();
        }



        // DELETE: api/cartItem/5
        [HttpDelete("{product_id}")]
        public async Task<IActionResult> DeletecartItemDTO(int product_id)
        {
            string user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CartItem cartItem = await _cartItemUnit.Repository.GetEntityAsync(c => c.UserID == user_id && c.ProductID == product_id);
            if (cartItem==null) return NotFound();
            await _cartItemUnit.Repository.DeleteAsync(cartItem);
            await _cartItemUnit.Repository.SaveChangesAsync();
            return NoContent();
        }


    }
}
