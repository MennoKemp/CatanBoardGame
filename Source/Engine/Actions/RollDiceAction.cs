namespace CatanBoardGame.Engine
{
	internal class RollDiceAction : Action
	{
		public override ActionType ActionType { get; } = ActionType.RollDice;
		public override Phase[] ValidPhases { get; } = { Phase.Production };

		protected override bool CanExecuteAction(Game game)
		{
			return true;
		}

		protected override Event ExecuteAction(Game game)
		{
			game.Dice.Roll();
			return new Event(EventType.DiceRolled, game.ActivePlayer, $"Rolled {game.Dice.Sum}.");
		}
	}
}
