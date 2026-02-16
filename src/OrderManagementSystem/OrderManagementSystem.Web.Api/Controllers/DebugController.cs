using Microsoft.AspNetCore.Mvc;

namespace OrderManagementSystem.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DebugController : ControllerBase
    {
        [HttpGet("throw")]
        public IActionResult Throw()
        {
            throw new Exception("Demo exception from DebugController");
        }
    }
}
