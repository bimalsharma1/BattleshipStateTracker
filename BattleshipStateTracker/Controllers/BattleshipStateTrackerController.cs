using System.Collections.Generic;
using BattleshipStateTracker.Interfaces;
using BattleshipStateTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace BattleshipStateTracker.Controllers
{
    [Route("api/[controller]")]
    public class BattleshipStateTrackerController : ControllerBase
    {
        private readonly IBattleshipService _battleshipService;

        public BattleshipStateTrackerController(IBattleshipService battleshipService)
        {
            _battleshipService = battleshipService;
        }

        // GET api/BattleshipStateTracker
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "This is the Battleship State Tracker", 
                "You can call APIs to create a board, add a ship and attack" };
        }

        // POST api/BattleshipStateTracker
        [HttpPost]
        public string Post([FromBody] BoardName name)
        {
            const int boardSize = 10; // Fix board size
            return _battleshipService.CreateBoard(name.Name, boardSize);
        }

        // PUT api/BattleshipStateTracker/
        [HttpPut]
        public string Put([FromBody] Ship ship)
        {
            return _battleshipService.AddBattleship(ship).Result;
        }

        // PUT api/BattleshipStateTracker/attack
        [HttpPut]
        [Route("attack")]
        public string Attack([FromBody] Attack attack)
        {
            return _battleshipService.Attack(attack);
        }
    }
}
