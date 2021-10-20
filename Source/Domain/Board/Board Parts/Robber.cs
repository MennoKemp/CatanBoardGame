using Auxilia.Extensions;

namespace CatanBoardGame
{
	public class Robber : IGameObject
	{
		public Robber()
		{
		}
		public Robber(Tile tile)
		{
			Tile = tile.ThrowIfNull(nameof(tile));
		}

		public GameObjectType GameObjectType { get; } = GameObjectType.Robber;

		public Tile Tile { get; set; }
	}
}
