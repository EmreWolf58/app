using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Core.Dtos;
using OrderManagementSystem.Core.Interface;

namespace OrderManagementSystem.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login (LoginRequest request)
        {
            var result = await _auth.LoginAsync(request);
            if (result == null) return Unauthorized(new { message = "Invalid username or password" });
            return Ok(result);
        }
    }
}
