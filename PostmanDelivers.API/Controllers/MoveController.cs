using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PostmanDelivers.API.ControllerModels;
using TikTakToe.Engine;

namespace PostmanDelivers.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoveController : ControllerBase
    {
        IConfiguration _config;

        public MoveController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public ActionResult<MoveResult> Post(GameMoveRequestModel mdl)
        {
            try
            {
                Game g = new Game();
                string cnxn = _config.GetValue<string>("ConnectionStr");
                MoveResult result = g.Move(mdl.xoro, mdl.whichSquare, mdl.playerID, mdl.gameId, cnxn);
                return Ok(result);
            }
            catch  (ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                return BadRequest(ioe.Message);
            }
        }
    }
}