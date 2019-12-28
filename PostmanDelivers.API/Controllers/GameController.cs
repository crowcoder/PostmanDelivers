using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TikTakToe.Engine;

namespace PostmanDelivers.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        IConfiguration _config;
        public GameController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public ActionResult<Game> Post([FromBody]string[] playerNames)
        {
            try
            {
                Game g = new Game();
                string cnxn = _config.GetValue<string>("ConnectionStr");
                var newGame = g.GenerateNewGame(playerNames[0], playerNames[1], cnxn);
                return CreatedAtAction(nameof(GetById), new { id = newGame.Id }, newGame);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                Game g = new Game();
                string cnxn = _config.GetValue<string>("ConnectionStr");
                g.Delete(id, cnxn);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Game>> Get()
        {
            Game g = new Game();
            string cnxn = _config.GetValue<string>("ConnectionStr");
            var allGames = g.GetAllGames(cnxn);
            return Ok(allGames);
        }

        [HttpGet("{id}")]
        public ActionResult<string> GetById(int id)
        {
            Game g = new Game();
            string cnxn = _config.GetValue<string>("ConnectionStr");
            var theGame = g.GetGameByID(id, cnxn);
            return Ok(theGame);
        }
    }
}