using System;

namespace CatanBoardGame
{
	public enum TileType
	{
		Desert,
		Forest,
		Hills,
		Pastures,
		Fields,
		Mountains,
		Sea
	}

	public static class TileTypeExtensions
	{
		public static ResourceType ToResourceType(this TileType tileType)
		{
			return tileType switch
			{
				TileType.Forest => ResourceType.Lumber,
				TileType.Hills => ResourceType.Brick,
				TileType.Pastures => ResourceType.Wool,
				TileType.Fields => ResourceType.Grain,
				TileType.Mountains => ResourceType.Ore,
				_ => throw new NotSupportedException($"Conversion of tile type {tileType} is not supported.")
			};
		}
	}
}
