using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleshipStateTracker.Interfaces;
using BattleshipStateTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace BattleshipStateTracker.Controllers
{
    [Route("api/[controller]")]
    public class BattleshipStateTrackerController : ControllerBase
    {
        private IBattleshipService _battleshipService;

        public BattleshipStateTrackerController(IBattleshipService battleshipService)
        {
            _battleshipService = battleshipService;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "This is the Battleship State Tracker", "value2" };
        }

        // GET api/values/name/1/3
        [HttpGet("{boardName}/{xPosition}/{yPosition}")]
        public string Get(string boardName, int xPosition, int yPosition)
        {
            AttackPosition attackPosition = new AttackPosition { XPosition = xPosition, YPosition = yPosition };
            return _battleshipService.Attack(boardName, attackPosition);
        }

        // POST api/values
        // POST api/values
        [HttpPost]
        public string Post([FromBody] BoardName name)
        {
            const int boardSize = 10; // Fix board size
            Console.WriteLine("inside controller");
            _battleshipService.CreateBoard(name.Name, boardSize);
            Console.WriteLine("success");
            return "success";
        }

        // PUT api/values/5
        [HttpPut]
        public void Put([FromBody] Ship ship)
        {
            _battleshipService.AddBattleship(ship);
        }
    }
}
