using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace PostmanDelivers.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EchoController : ControllerBase
    {
        IConfiguration _config;
        public EchoController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public ActionResult Post([FromForm] Dictionary<string, string> stringsToEcho)
        {
            stringsToEcho.Add("env", _config.GetValue<string>("APP_ENVIRONMENT") ?? "No environment configured");
            return Ok(stringsToEcho);
        }
    }
}