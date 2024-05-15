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
    public class OrderController : ControllerBase
    {

        private readonly IUnitOfWork<Order> _OrderUnit;
        private readonly IMapper _Mapper;

        public OrderController(IUnitOfWork<Order> Unit, IMapper Mapper)
        {
            _OrderUnit = Unit;
            _Mapper = Mapper;
        }


        // GET: api/Order
        // [Authorize]
        [HttpGet]
        [Produces("application/json")] //to send data in this format only 
        public async Task<ActionResult> GetOrders()
        {
            List<Order> orders = await _OrderUnit.Repository.GetAllAsync();
            //Mapping
            List<OrderDTO> ordersDTO = _Mapper.Map<List<OrderDTO>>(orders);
 
            return Ok(ordersDTO);
        }

        // GET: api/Order/5
        [HttpGet("{id:int}", Name = "GetOrderByIdRoute")]
        [Produces("application/json")] //to send data in this format only 
        public async Task<ActionResult> GetOrder(int id)
        {
            Order order = await _OrderUnit.Repository.GetByIdAsync(id);
            if (order == null) return NotFound();
            OrderDTO orderDTO = _Mapper.Map<OrderDTO>(order);
            return Ok(orderDTO);
        }


        // POST: api/Order
        [HttpPost]
        [Consumes("application/json")] 
        public async Task<ActionResult> PostOrder(OrderDTO orderDTO)
        {
            orderDTO.userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid) return BadRequest();
            Order order = _Mapper.Map<Order>(orderDTO);

            await _OrderUnit.Repository.InsertAsync(order);
            await _OrderUnit.Repository.SaveChangesAsync();


            string url = Url.Link("GetOrderByIdRoute", new { id = order.Id });

            orderDTO.id = order.Id;

            return Created(url, orderDTO);
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        [Consumes("application/json")] 
        public async Task<IActionResult> PutOrder(int id, OrderDTO orderDTO)
        {
            orderDTO.userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isExist = await _OrderUnit.Repository.IsExistAsync(id);
            if (id != orderDTO.id || !isExist) return BadRequest();
            Order Order = _Mapper.Map<Order>(orderDTO);
            _OrderUnit.Repository.ChangeStateToModified(Order);
            try
            {
                await _OrderUnit.Repository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }



        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDTO(int id)
        {
            if (!(await _OrderUnit.Repository.IsExistAsync(id))) return NotFound();
            await _OrderUnit.Repository.DeleteAsync(id);
            await _OrderUnit.Repository.SaveChangesAsync();
            return NoContent();
        }


    }
}
