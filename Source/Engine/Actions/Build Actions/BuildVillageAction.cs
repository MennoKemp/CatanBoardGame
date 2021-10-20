using System.Linq;
using Auxilia.Extensions;

namespace CatanBoardGame.Engine
{
	internal class BuildVillageAction : MultipleChoiceAction<Village>, IPaidAction
	{
		public override GameObjectType OptionType { get; } = GameObjectType.Village;
		public override ActionType ActionType { get; } = ActionType.BuildVillage;
		public override Phase[] ValidPhases { get; } = { Phase.Building, Phase.Colonization };
		
		public override void Update(Game game)
		{
			Options = game.Board.Intersections.Where(i => CanPlaceVillage(game, i)).Select(i => new Village(game.ActivePlayer, i));
			CanCancel = game.CurrentPhase != Phase.Colonization;
		}
		
		public ResourceCollection GetCosts(Game game)
		{
			return game.CurrentPhase == Phase.Building
				? Costs.Village
				: new ResourceCollection();
		}

		protected override bool CanExecuteAction(Game game)
		{
			return game.CurrentPhase == Phase.Colonization 
				? game.ActivePlayer.Roads.Count == game.ActivePlayer.Villages.Count
				: game.ActivePlayer.Villages.Count < Limits.Villages;
		}
		
		protected override Event ExecuteAction(Game game)
		{
			Intersection intersection = game.Board[Selection.Single().Intersection];
			game.ActivePlayer.Villages.Add((Village)(intersection.Settlement = new Village(game.ActivePlayer, intersection)));

			return new Event(EventType.VillageBuilt, game.ActivePlayer, "Built a village.");
		}

		private static bool CanPlaceVillage(Game game, Intersection intersection)
		{
			return intersection.Settlement == null &&
			       game.Board.GetIntersections(intersection).All(i => i.Settlement == null) && 
			       (game.CurrentPhase == Phase.Colonization ||
				   game.Board.GetPaths(intersection).Any(p => p.Road?.Owner == game.ActivePlayer));
		}
	}
}
