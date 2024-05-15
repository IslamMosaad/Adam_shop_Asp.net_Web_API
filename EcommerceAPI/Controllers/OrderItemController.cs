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
    public class OrderItemController : ControllerBase
    {

        private readonly IUnitOfWork<OrderItem> _OrderItemUnit;
        private readonly IMapper _Mapper;

        public OrderItemController(IUnitOfWork<OrderItem> Unit, IMapper Mapper)
        {
            _OrderItemUnit = Unit;
            _Mapper = Mapper;
        }


        // GET: api/OrderItem
        // [Authorize]
        [HttpGet]
        [Produces("application/json")] //to send data in this format only 
        public async Task<ActionResult> GetOrderItems()
        {
            List<OrderItem> orderItems = await _OrderItemUnit.Repository.GetAllAsync();
            //Mapping
            List<OrderItemDTO> orderItemsDTO = _Mapper.Map<List<OrderItemDTO>>(orderItems);
 
            return Ok(orderItemsDTO);
        }

        // GET: api/OrderItem/5
        [HttpGet("{id:int}", Name = "GetOrderItemByIdRoute")]
        [Produces("application/json")] //to send data in this format only 
        public async Task<ActionResult> GetOrderItem(int id)
        {
            OrderItem orderItem = await _OrderItemUnit.Repository.GetByIdAsync(id);
            if (orderItem == null) return NotFound();
            OrderItemDTO orderItemDTO = _Mapper.Map<OrderItemDTO>(orderItem);
            return Ok(orderItemDTO);
        }


        // POST: api/OrderItem
        [HttpPost]
        [Consumes("application/json")] 
        public async Task<ActionResult> PostOrderItem(OrderItemDTO orderItemDTO)
        {
            if (!ModelState.IsValid) return BadRequest();
            OrderItem orderItem = _Mapper.Map<OrderItem>(orderItemDTO);

            await _OrderItemUnit.Repository.InsertAsync(orderItem);
            await _OrderItemUnit.Repository.SaveChangesAsync();


            string url = Url.Link("GetOrderItemByIdRoute", new { id = orderItem.Id });

            orderItemDTO.id = orderItem.Id;

            return Created(url, orderItemDTO);
        }

        // PUT: api/OrderItems/5
        [HttpPut("{id}")]
        [Consumes("application/json")] 
        public async Task<IActionResult> PutOrderItem(int id, OrderItemDTO orderItemDTO)
        {
            bool isExist = await _OrderItemUnit.Repository.IsExistAsync(id);
            if (id != orderItemDTO.id || !isExist) return BadRequest();
            OrderItem OrderItem = _Mapper.Map<OrderItem>(orderItemDTO);
            _OrderItemUnit.Repository.ChangeStateToModified(OrderItem);
            try
            {
                await _OrderItemUnit.Repository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }



        // DELETE: api/OrderItem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItemDTO(int id)
        {
            if (!(await _OrderItemUnit.Repository.IsExistAsync(id))) return NotFound();
            await _OrderItemUnit.Repository.DeleteAsync(id);
            await _OrderItemUnit.Repository.SaveChangesAsync();
            return NoContent();
        }


    }
}
