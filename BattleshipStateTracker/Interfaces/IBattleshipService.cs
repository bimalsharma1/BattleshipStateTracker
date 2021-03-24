using System.Collections.Generic;
using System.Threading.Tasks;
using BattleshipStateTracker.Models;

namespace BattleshipStateTracker.Interfaces
{
	public interface IBattleshipService
	{
		public string CreateBoard(string playerName, int boardSize);
		public Task<string> AddBattleship(Ship ship);
		public string Attack(Attack attack);
	}
}
