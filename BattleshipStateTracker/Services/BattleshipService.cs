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
            Console.WriteLine(boardSize.ToString());
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
            Console.WriteLine("create board");
            Console.WriteLine(board.ToString());
            _repositotyService.CreateBoard(playerName, board);
        }

        public async Task AddBattleship(Ship ship)
        {
            var boards = _repositotyService.GetBoards(ship.BoardName).Result;
            var board = boards.FirstOrDefault();
            try
            {
                board.Ships = new List<Ship>
                {
                    ship
                };

                board.Board = PlaceShipOnBoard(board.Board, ship);
                Console.WriteLine(JsonConvert.SerializeObject(board));
            } catch (Exception ex)
            {
                throw new InvalidOperationException("Cannot find the board");
                //Console.WriteLine(ex.Message);
            }
            Console.WriteLine(JsonConvert.SerializeObject(board));
            await _repositotyService.AddShip(board);
        }

        private List<Position> PlaceShipOnBoard(List<Position> board, Ship ship)
        {
            for (int i = ship.StartPosition.XPosition; i <= ship.EndPosition.XPosition; i++)
            {
                for (int j = ship.StartPosition.YPosition; j <= ship.EndPosition.YPosition; j++)
                {
                    var currentBoard = board.FirstOrDefault(b => b.XPosition == i && b.YPosition == j);
                    currentBoard.HasShip = true;
                }
            }
            Console.WriteLine(JsonConvert.SerializeObject(board));
            return board;
        }

        public string Attack(string boardName, AttackPosition position)
        {
            Console.WriteLine("A1");
            var boards = _repositotyService.GetBoards(boardName).Result;
            Console.WriteLine("A2");
            var board = boards.FirstOrDefault();
            Console.WriteLine("A3");
            Console.WriteLine(JsonConvert.SerializeObject(board));
            var hitPosition = board.Board.FirstOrDefault(b => b.XPosition == position.XPosition && b.YPosition == position.YPosition);
            Console.WriteLine("A4");
            Console.WriteLine(JsonConvert.SerializeObject(hitPosition));
            Console.WriteLine("A5");
            return hitPosition != null && hitPosition.HasShip ? "Hit" : "Miss";
        }
    }
}
 