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
        private readonly IRepositoryService _repositoryService;
        private const int MinBoardSize = 1;
        private const int MaxBoardSize = 10;

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
                if (IsInvalidShipPosition(ship))
                {
                    return "Invalid ship coordinates or orientation";
                }
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
                return "Cannot find the board, please create the board";
            }
        }

        public string Attack(Attack attack)
        {
            if (IsInvalidPosition(attack.AttackPosition))
            {
                return "Invalid attack position, please check orientation and position and try again";
            }

            var boards = _repositoryService.GetBoards(attack.BoardName).Result;
            var board = boards.FirstOrDefault();
            var shipPosition = board?.Ships.FirstOrDefault();
            Console.WriteLine(JsonConvert.SerializeObject(shipPosition));
            if (shipPosition == null)
            {
                Console.WriteLine("An error has occurred, invalid ship position.");
                return "An error has occurred, invalid ship position.";
            }

            Console.WriteLine(shipPosition.Orientation);
            var isHit = string.Equals(shipPosition.Orientation, Orientation.Vertical, StringComparison.InvariantCultureIgnoreCase)
                        ? GetAttackStatusVerticalOrientation(attack.AttackPosition, shipPosition.StartPosition, shipPosition.EndPosition)
                        : ShipEndPosition(attack.AttackPosition, shipPosition.StartPosition, shipPosition.EndPosition);

             return isHit ? "Hit" : "Miss";
        }

        private bool GetAttackStatusVerticalOrientation(Position attackPosition, Position shipStartPosition, Position shipEndPosition)
        {
            return attackPosition.XPosition == shipStartPosition.XPosition &&
                   attackPosition.YPosition >= shipStartPosition.YPosition &&
                   attackPosition.YPosition <= shipEndPosition.YPosition;
        }

        private bool ShipEndPosition(Position attackPosition, Position shipStartPosition, Position shipEndPosition)
        {
            return attackPosition.YPosition == shipStartPosition.YPosition &&
                   attackPosition.XPosition >= shipStartPosition.XPosition &&
                   attackPosition.XPosition <= shipEndPosition.XPosition;
        }

        private bool IsInvalidShipPosition(Ship ship)
        {
            if ((ship.Orientation != Orientation.Vertical && ship.Orientation != Orientation.Horizontal) || IsInvalidPosition(ship.StartPosition) || IsInvalidPosition(ship.EndPosition))
            {
                return true;
            }

            // Assumption: The ship can be on only 1 row or column
            return ship.Orientation == Orientation.Vertical 
                    ? ship.StartPosition.XPosition != ship.EndPosition.XPosition
                    : ship.StartPosition.YPosition != ship.EndPosition.YPosition;
        }

        private bool IsInvalidPosition(Position attackPosition)
        {
            return attackPosition.XPosition < MinBoardSize
                   || attackPosition.XPosition > MaxBoardSize
                   || attackPosition.YPosition < MinBoardSize
                   || attackPosition.YPosition > MaxBoardSize;
        }
    }
}
 