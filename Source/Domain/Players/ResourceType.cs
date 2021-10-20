using System;

namespace CatanBoardGame
{
	public enum ResourceType
	{
		Lumber,
		Brick,
		Wool,
		Grain,
		Ore,
		Generic
	}

	public static class ResourceTypeExtensions
	{
		public static TileType ToTileType(this ResourceType resourceType)
		{
			return resourceType switch
			{
				ResourceType.Lumber => TileType.Forest,
				ResourceType.Brick => TileType.Hills,
				ResourceType.Wool => TileType.Pastures,
				ResourceType.Grain => TileType.Fields,
				ResourceType.Ore => TileType.Mountains,
				_ => throw new NotSupportedException($"Conversion of resource type {resourceType} is not supported.")
			};
		}
	}
}
