using Microsoft.AspNetCore.Mvc;

namespace HttpHeaderSecurity.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new[] { "value1", "value2" });
        }
    }
}
