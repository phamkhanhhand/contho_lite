using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new[] {
            new { Id = 1, Name = "Bàn phím" },
            new { Id = 2, Name = "Chuột" }
        });
        }
    }
}
