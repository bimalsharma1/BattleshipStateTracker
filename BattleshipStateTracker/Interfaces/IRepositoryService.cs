using System.Collections.Generic;
using System.Threading.Tasks;
using BattleshipStateTracker.Models;

namespace BattleshipStateTracker.Interfaces
{
	public interface IRepositoryService
	{
		public Task CreateBoard(string name, List<Position> board);
		public Task<IEnumerable<BattleshipBoard>> GetBoards(string boardName);
		public Task AddShip(BattleshipBoard board);
	}
}
