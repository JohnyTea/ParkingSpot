using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyParkingSpot.Api.Controllers
{
    [Route("Ping")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public string Get() => "Pong";
    }
}
