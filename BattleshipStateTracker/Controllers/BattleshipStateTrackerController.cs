using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleshipStateTracker.Controllers.Services;
using BattleshipStateTracker.Interfaces;
using BattleshipStateTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace BattleshipStateTracker.Controllers
{
    [Route("api/[controller]")]
    public class BattleshipStateTrackerController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public string Post([FromBody] string name, [FromServices] IBattleshipService battleshipService)
        {
            const int boardSize = 10; // Fix board size
            battleshipService.CreateBoard(name, boardSize);
            return "success";
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
           
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
