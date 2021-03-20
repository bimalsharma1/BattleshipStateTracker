using BattleshipStateTracker.Interfaces;
using BattleshipStateTracker.Models;
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

        public void AddBattleship(int[,] boardSize)
        {
           // _board = new bool[boardSize, boardSize];
        }

    }
}
