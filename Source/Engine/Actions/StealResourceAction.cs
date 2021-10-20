using Auxilia.Extensions;
using System.Linq;

namespace CatanBoardGame.Engine
{
	internal class StealResourceAction : MultipleChoiceAction<Player>
	{
		public override GameObjectType OptionType { get; } = GameObjectType.Player;
		public override ActionType ActionType { get; } = ActionType.StealResource;
		public override Phase[] ValidPhases { get; } = { Phase.Robbing };
		
		public override void Update(Game game)
		{
			Options = game.Players.Except(game.ActivePlayer).Where(p => p.Resources.Count > 0);
		}

		protected override bool CanExecuteAction(Game game)
		{
			return true;
		}
		
		protected override Event ExecuteAction(Game game)
		{
			Player target = Selection.Single();
			ResourceType resource = target.Resources.Randomize(GameEngine.Random).First();
			target.Resources[resource]--;
			game.ActivePlayer.Resources[resource]++;
			return new Event(EventType.CardStolen, game.ActivePlayer, $"Stole a resource from {target}.");
		}
	}
}
