using Ludumia.NumberGeneration;
using System.Collections.Generic;
using System.Linq;

namespace CatanBoardGame
{
	public class Game
	{
		public int Seed { get; set; }
		public Dice Dice { get; set; }
		public Board Board { get; set; }
		public IList<Player> Players { get; set; }

		public Player ActivePlayer { get; set; }
		public Player NextPlayer
		{
			get => Players[(Players.IndexOf(ActivePlayer) + 1) % Players.Count];
		}
		public Player PreviousPlayer
		{
			get => Players[(Players.IndexOf(ActivePlayer) - 1 + Players.Count) % Players.Count];
		}

		public Phase CurrentPhase { get; set; }

		public List<IAction> Actions { get; set; }
		public IAction LastAction
		{
			get => Actions.LastOrDefault();
		}
	}
}
