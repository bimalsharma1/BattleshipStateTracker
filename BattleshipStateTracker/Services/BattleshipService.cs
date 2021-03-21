using BattleshipStateTracker.Interfaces;
using BattleshipStateTracker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleshipStateTracker.Controllers.Services
{
    public class BattleshipService : IBattleshipService
    {
        private IRepositoryService _repositotyService;

        public BattleshipService(IRepositoryService repositoryService)
        {
            _repositotyService = repositoryService;
        }

        // Board size is customisable, it will always be a square grid.
        public string CreateBoard(string playerName, int boardSize)
        {
            try
            {
                var board = new List<Position>();
                for (int i = 1; i <= boardSize; i++)
                {
                    for (int j = 1; j <= boardSize; j++)
                    {
                        board.Add(new Position
                        {
                            XPosition = i,
                            YPosition = j,
                            HasShip = false
                        });
                    }
                }
                _repositotyService.CreateBoard(playerName, board);
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
                var boards = _repositotyService.GetBoards(ship.BoardName).Result;
                Console.WriteLine(JsonConvert.SerializeObject(boards));
                var board = boards.FirstOrDefault();
                board.Ships = new List<Ship>
                {
                    ship
                };

                board.Board = PlaceShipOnBoard(board.Board, ship);
                await _repositotyService.AddShip(board);
                return "success";

            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return $"Cannot find the board, please create the board";
            }
        }

        private List<Position> PlaceShipOnBoard(List<Position> board, Ship ship)
        {
            var xStart = ship.StartPosition.XPosition < ship.EndPosition.XPosition ? ship.StartPosition.XPosition : ship.EndPosition.XPosition;
            var xEnd = ship.StartPosition.XPosition > ship.EndPosition.XPosition ? ship.StartPosition.XPosition : ship.EndPosition.XPosition;
            var yStart = ship.StartPosition.YPosition < ship.EndPosition.YPosition ? ship.StartPosition.YPosition : ship.EndPosition.YPosition;
            var yEnd = ship.StartPosition.YPosition > ship.EndPosition.YPosition ? ship.StartPosition.YPosition : ship.EndPosition.YPosition;
            foreach (var position in board)
            {
                position.HasShip = position.XPosition >= xStart && position.XPosition <= xEnd
                                    && position.YPosition >= yStart && position.YPosition <= yEnd;
            }
            Console.WriteLine(JsonConvert.SerializeObject(board));
            return board;
        }

        public string Attack(string boardName, AttackPosition position)
        {
            var boards = _repositotyService.GetBoards(boardName).Result;
            var board = boards.FirstOrDefault();
            var hitPosition = board.Board.FirstOrDefault(b => b.XPosition == position.XPosition && b.YPosition == position.YPosition);
            Console.WriteLine(JsonConvert.SerializeObject(hitPosition));

            if (hitPosition == null)
            {
                Console.WriteLine("An error has occurred, please try again or contact the IT team.");
                return "An error has occurred, please try again or contact the IT team.";
            }

            return hitPosition.HasShip ? "Hit" : "Miss";
        }
    }
}
 