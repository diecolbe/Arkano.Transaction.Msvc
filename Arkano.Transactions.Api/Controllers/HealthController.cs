using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Arkano.Transactions.Api.Controllers
{
    [ApiController]    
    [Route("api/v1/[controller]")]
    [SwaggerTag("Servicio para consutlar el estado de salud del API")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("API is running");
    }
}
