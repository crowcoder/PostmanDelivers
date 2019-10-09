using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace PostmanDelivers.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EchoController : ControllerBase
    {
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public ActionResult Post([FromForm] Dictionary<string, string> stringsToEcho)
        {
            return Ok(stringsToEcho);
        }
    }
}