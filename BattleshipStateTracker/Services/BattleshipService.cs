using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleshipStateTracker.Interfaces;
using BattleshipStateTracker.Models;
using Newtonsoft.Json;

namespace BattleshipStateTracker.Services
{
    public class BattleshipService : IBattleshipService
    {
        private IRepositoryService _repositoryService;

        public BattleshipService(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        // Board size is customisable, it will always be a square grid.
        public string CreateBoard(string playerName, int boardSize)
        {
            try
            {
                var board = new List<ShipPosition>();
                for (var i = 1; i <= boardSize; i++)
                {
                    for (var j = 1; j <= boardSize; j++)
                    {
                        board.Add(new ShipPosition
                        {
                            Position = new Position() {
                                XPosition = i,
                                YPosition = j
                            }
                        });
                    }
                }
                _repositoryService.CreateBoard(playerName, board);
                return "success";
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return $"The board already exists.";
            }
        }

        public async Task<string> AddBattleship(Ship ship)
        {
            try
            {
                var boards = _repositoryService.GetBoards(ship.BoardName).Result;
                Console.WriteLine(JsonConvert.SerializeObject(boards));
                var board = boards.FirstOrDefault();
                if (board == null) return "failed to add ship";
                board.Ships = new List<Ship>
                {
                    ship
                };

                await _repositoryService.AddShip(board);

                return "success";

            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return $"Cannot find the board, please create the board";
            }
        }

        public string Attack(Attack attackPosition)
        {
            const int minBoardSize = 1;
            const int maxBoardSize = 10;
            if (attackPosition.AttackPosition.XPosition < minBoardSize 
                || attackPosition.AttackPosition.XPosition > maxBoardSize 
                || attackPosition.AttackPosition.YPosition < minBoardSize 
                || attackPosition.AttackPosition.YPosition > maxBoardSize)
            {
                return "Invalid attack position, please try again";
            }

            var boards = _repositoryService.GetBoards(attackPosition.BoardName).Result;
            var board = boards.FirstOrDefault();
            var shipPosition = board?.Ships.FirstOrDefault();
            Console.WriteLine(JsonConvert.SerializeObject(shipPosition));

            if (shipPosition == null)
            {
                Console.WriteLine("An error has occurred, invalid ship position.");
                return "An error has occurred, invalid ship position.";
            }

            Console.WriteLine(shipPosition.Orientation);

            var isHit = string.Equals(shipPosition.Orientation, "vertical", StringComparison.InvariantCultureIgnoreCase)
            ? attackPosition.AttackPosition.XPosition == shipPosition.StartPosition.XPosition &&
              attackPosition.AttackPosition.YPosition >= shipPosition.StartPosition.YPosition &&
              attackPosition.AttackPosition.YPosition <= shipPosition.EndPosition.YPosition
            : attackPosition.AttackPosition.YPosition == shipPosition.StartPosition.YPosition &&
              attackPosition.AttackPosition.XPosition >= shipPosition.StartPosition.XPosition &&
              attackPosition.AttackPosition.XPosition <= shipPosition.EndPosition.XPosition;

             return isHit ? "Hit" : "Miss";
            

        }
    }
}
 