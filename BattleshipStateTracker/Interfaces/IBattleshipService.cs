using System.Collections.Generic;
using System.Threading.Tasks;
using BattleshipStateTracker.Models;

namespace BattleshipStateTracker.Interfaces
{
	public interface IBattleshipService
	{
		public void CreateBoard(string playerName, int boardSize);
		public Task AddBattleship(Ship ship);
		public string Attack(string board, AttackPosition position);
	}
}
