using Auxilia.Extensions;
using System.Linq;

namespace CatanBoardGame.Engine
{
	internal class MoveRobberAction : MultipleChoiceAction<Tile>
	{
		public override GameObjectType OptionType { get; } = GameObjectType.Tile;
		public override ActionType ActionType { get; } = ActionType.MoveRobber;
		public override Phase[] ValidPhases { get; } = { Phase.Robbing };
		
		public override void Update(Game game)
		{
			Options = game.Board.Tiles.Except(game.Board.Robber.Tile);
		}

		protected override bool CanExecuteAction(Game game)
		{
			return true;
		}

		protected override Event ExecuteAction(Game game)
		{
			game.Board.Robber.Tile = Selection.Single();
			return new Event(EventType.RobberMoved, game.ActivePlayer, "Moved the robber.");
		}
	}
}
