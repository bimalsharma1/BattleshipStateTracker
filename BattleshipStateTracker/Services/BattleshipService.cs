using BattleshipStateTracker.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleshipStateTracker.Controllers.Services
{
    public class BattleshipService
    {
        private IRepositoryService _repositotyService;

        private bool[,] _board;

        public BattleshipService(IRepositoryService repositoryService)
        {
            _repositotyService = repositoryService;
        }

        // Board size is customisable, it will always be a square grid.
        public void CreateBoard(string playerName, int boardSize)
        {
            _board = new bool[boardSize, boardSize];
            _repositotyService.CreateBoard(playerName, _board);
        }

        public void AddBattleship(int[,] boardSize)
        {
           // _board = new bool[boardSize, boardSize];
        }

    }
}
