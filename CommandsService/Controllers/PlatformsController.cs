using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{   
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        public PlatformsController()
        {
            
        }

        [HttpPost]
        public IActionResult TestInboundConnection() {
            Console.WriteLine("---> Inbound POST a command service.");
            return Ok("Inbound test of from Platforms Controller");
        }
    }
}   