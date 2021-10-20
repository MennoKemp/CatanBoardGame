using System.Linq;

namespace CatanBoardGame.Engine
{
	internal class BuildRoadAction : MultipleChoiceAction<Road>, IPaidAction
	{
		public override GameObjectType OptionType { get; } = GameObjectType.Road;
		public override ActionType ActionType { get; } = ActionType.BuildRoad;
		public override Phase[] ValidPhases { get; } = { Phase.Building, Phase.Colonization };
		
		public override void Update(Game game)
		{
			Options = game.Board.Paths.Where(p => CanPlaceRoad(game, p)).Select(p => new Road(game.ActivePlayer, p));
			CanCancel = game.CurrentPhase != Phase.Colonization;
		}

		public ResourceCollection GetCosts(Game game)
		{
			return game.CurrentPhase == Phase.Building
				? Costs.Road
				: new ResourceCollection();
		}
		
		protected override bool CanExecuteAction(Game game)
		{
			return game.CurrentPhase == Phase.Colonization
				? game.ActivePlayer.Roads.Count < game.ActivePlayer.Villages.Count
				: game.ActivePlayer.Roads.Count < Limits.Roads;
		}

		protected override Event ExecuteAction(Game game)
		{
			Path path = game.Board[Selection.Single().Path];
			game.ActivePlayer.Roads.Add(path.Road = new Road(game.ActivePlayer, path));
			return new Event(EventType.RoadBuilt, game.ActivePlayer, "Built a road.");
		}

		private static bool CanPlaceRoad(Game game, Path path)
		{
			Player activePlayer = game.ActivePlayer;
			return path.Road == null &&
			       (game.CurrentPhase == Phase.Colonization
				? game.Board.GetIntersections(path).Any(i =>								// Check if there is an intersection connected to the path...
					  i.Settlement is Village village && village.Owner == activePlayer &&   // with a village owned by the player...
					  game.Board.GetPaths(i).All(r => r.Road?.Owner != activePlayer))		// and no roads connected to it.
				: game.Board.GetIntersections(path).Any(i =>								// Check if there is an intersection connected to the path...
					i.Settlement?.Owner == activePlayer ||									// with a settlement owned by the player or...
					i.Settlement == null && game.Board.GetPaths(i).Any(p =>					// a connected path with a road owned by that player.
						p.Road?.Owner == activePlayer)));		
		}
	}
}
