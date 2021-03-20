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
        public void CreateBoard(string playerName, int boardSize)
        {
            List<Position> board = new List<Position>();
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
        }

        public async Task AddBattleship(Ship ship)
        {
            
            try
            {
                var boards = _repositotyService.GetBoards(ship.BoardName).Result;
                var board = boards.FirstOrDefault();
                board.Ships = new List<Ship>
                {
                    ship
                };

                board.Board = PlaceShipOnBoard(board.Board, ship);
                await _repositotyService.AddShip(board);

            } catch (Exception ex)
            {
                throw new InvalidOperationException($"Cannot find the board {ex.Message}");
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
            Console.WriteLine(JsonConvert.SerializeObject(board));
            var hitPosition = board.Board.FirstOrDefault(b => b.XPosition == position.XPosition && b.YPosition == position.YPosition);
            Console.WriteLine(JsonConvert.SerializeObject(hitPosition));
            return hitPosition != null && hitPosition.HasShip ? "Hit" : "Miss";
        }
    }
}
 