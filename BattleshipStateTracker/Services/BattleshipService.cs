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
            for (int i=0;i<10;i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    board.Add(new Position
                    {
                        XPosition = i,
                        YPosition = j,
                        Bombed = false
                    });
                }
            }
            Console.WriteLine("create board");
            Console.WriteLine(board.ToString());
            _repositotyService.CreateBoard(playerName, board);
        }

        public async Task AddBattleship(Ship ship)
        {
            Console.WriteLine("0");
            var boards = _repositotyService.GetBoards(ship).Result;
            Console.WriteLine("1");
            Console.WriteLine(JsonConvert.SerializeObject(boards));
            Console.WriteLine("1a");
            var board = boards.FirstOrDefault();
            Console.WriteLine("10");
            Console.WriteLine(JsonConvert.SerializeObject(board));
            Console.WriteLine("11");
            try
            {
                Console.WriteLine("12");
                board.Ships = new List<Ship>
                {
                    ship
                };
                Console.WriteLine(JsonConvert.SerializeObject(board));
            } catch (Exception ex)
            {
                Console.WriteLine("13");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("14");
            Console.WriteLine(JsonConvert.SerializeObject(board));
            await _repositotyService.AddShip(board);
        }

    }
}
