using System.Linq;
using Auxilia.Extensions;

namespace CatanBoardGame.Engine
{
	internal class ResourceManager
	{
		public void DealInitialResources(Game game, Player player)
		{
			game.Board.GetTiles(player.Villages.Last().Intersection)
				.Select(t => t.GetResource())
				.OfType<ResourceType>()
				.Execute(player.Resources.Add);
		}
	}
}
