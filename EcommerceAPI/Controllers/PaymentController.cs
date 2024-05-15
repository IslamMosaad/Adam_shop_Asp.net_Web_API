using AutoMapper;
using EcommerceAPI.DTO;
using EcommerceAPI.Models;
using EcommerceAPI.Unit_OF_Work;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        private readonly IUnitOfWork<Payment> _PaymentUnit;
        private readonly IMapper _Mapper;

        public PaymentController(IUnitOfWork<Payment> Unit, IMapper Mapper)
        {
            _PaymentUnit = Unit;
            _Mapper = Mapper;
        }


        // GET: api/Payment
        // [Authorize]
        [HttpGet]
        [Produces("application/json")] //to send data in this format only 
        public async Task<ActionResult> Getpayments()
        {
            List<Payment> payments = await _PaymentUnit.Repository.GetAllAsync();
            //Mapping
            List<PaymentDTO> PaymentDTOs = _Mapper.Map<List<PaymentDTO>>(payments);
            return Ok(PaymentDTOs);
        }

        [HttpGet("ForUser")]
        [Produces("application/json")] //to send data in this format only 
        public async Task<ActionResult> GetpaymentsForUser()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Payment> payments = await _PaymentUnit.Repository.GetAllAsync();
            payments = payments.Where(x => x.UserId == userId).ToList();
            //Mapping
            List<PaymentDTO> PaymentDTOs = _Mapper.Map<List<PaymentDTO>>(payments);
            return Ok(PaymentDTOs);
        }

        // GET: api/Payment/5
        [HttpGet("{id:int}", Name = "GetPaymentByIdRoute")]
        [Produces("application/json")] //to send data in this format only 
        public async Task<ActionResult> GetPayment(int id)
        {
            var Payment = await _PaymentUnit.Repository.GetByIdAsync(id);
            if (Payment == null) return NotFound();
            PaymentDTO PaymentDTO = _Mapper.Map<PaymentDTO>(Payment);
            return Ok(PaymentDTO);
        }


        // POST: api/Payment
        [HttpPost]
        [Consumes("application/json")] 
        public async Task<ActionResult> PostPayment(PaymentDTO PaymentDTO)
        {
            if (!ModelState.IsValid) return BadRequest();
            Payment Payment = _Mapper.Map<Payment>(PaymentDTO);

           //UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _PaymentUnit.Repository.InsertAsync(Payment);
            await _PaymentUnit.Repository.SaveChangesAsync();


            string url = Url.Link("GetPaymentByIdRoute", new { id = Payment.Id });

            PaymentDTO.id = Payment.Id;

            return Created(url, PaymentDTO);
        }

        // PUT: api/Payments/5
        [HttpPut("{id}")]
        [Consumes("application/json")] 
        public async Task<IActionResult> PutPayment(int id, PaymentDTO PaymentDTO)
        {
            bool isExist = await _PaymentUnit.Repository.IsExistAsync(id);
            if (id != PaymentDTO.id || !isExist) return BadRequest();
            Payment Payment = _Mapper.Map<Payment>(PaymentDTO);
            _PaymentUnit.Repository.ChangeStateToModified(Payment);
            try
            {
                await _PaymentUnit.Repository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }



        // DELETE: api/Payment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentDTO(int id)
        {
            if (!(await _PaymentUnit.Repository.IsExistAsync(id))) return NotFound();
            await _PaymentUnit.Repository.DeleteAsync(id);
            await _PaymentUnit.Repository.SaveChangesAsync();
            return NoContent();
        }


    }
}
