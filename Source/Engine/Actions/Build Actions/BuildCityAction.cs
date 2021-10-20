using System.Linq;

namespace CatanBoardGame.Engine
{
	internal class BuildCityAction : MultipleChoiceAction<City>, IPaidAction
	{
		public override GameObjectType OptionType { get; } = GameObjectType.City;
		public override ActionType ActionType { get; } = ActionType.BuildCity;
		public override Phase[] ValidPhases { get; } = { Phase.Building };
		
		public override void Update(Game game)
		{
			Options = game.ActivePlayer.Villages.Select(v => new City(game.ActivePlayer, v.Intersection));
		}
		
		public ResourceCollection GetCosts(Game game)
		{
			return Costs.City;
		}

		protected override bool CanExecuteAction(Game game)
		{
			return game.ActivePlayer.Cities.Count < Limits.Cities;
		}

		protected override Event ExecuteAction(Game game)
		{
			Intersection intersection = game.Board[Selection.Single().Intersection];
			game.ActivePlayer.Villages.Remove((Village)intersection.Settlement);
			game.ActivePlayer.Cities.Add((City)(intersection.Settlement = new City(game.ActivePlayer, intersection)));
			return new Event(EventType.CityBuilt, game.ActivePlayer, "Built a city.");
		}
	}
}
