using EcommerceAPI.DTO;
using EcommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;
        private IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _configuration = configuration;
            _userService = userService;
        }


        // /api/auth/register/true                     
        [HttpPost("Register/{isAdmin:bool}")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTO model, [FromRoute]bool isAdmin=false)
        {
            if (ModelState.IsValid)
            {
                UserManagerResponseDTO result = await _userService.RegisterUserAsync(model, isAdmin);
                if (result.IsSuccess) return Ok(result); // Status Code: 200 
                else return BadRequest(result);
            }
            return BadRequest("Some properties are not valid"); // Status code: 400
        }




        // /api/auth/login
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO model)
        {
            if (ModelState.IsValid)
            {
                UserManagerResponseDTO result = await _userService.LoginUserAsync(model);

                if (result.IsSuccess)
                {
                    //await _mailService.SendEmailAsync(model.Email, "New login", "<h1>Hey!, new login to your account noticed</h1><p>New login to your account at " + DateTime.Now + "</p>");
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid");
        }



        // /api/auth/logout
        [HttpGet("logout")]
        public async Task<IActionResult> LoginAsync()
        {
            UserManagerResponseDTO result = await _userService.LogOutUserAsync();
            return Ok(result);

        }







    }
}
