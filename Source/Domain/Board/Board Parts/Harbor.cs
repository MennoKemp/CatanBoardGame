using System;
using Ludumia.Grids.Hexagonal;

namespace CatanBoardGame
{
	public class Harbor : IGameObject, IEquatable<Harbor>
	{
		public Harbor(ResourceType resourceResourceType, PointyToppedDirection direction, Path path, Tile tile)
		{
			ResourceType = resourceResourceType;
			Direction = direction;
			Path = path;
			Tile = tile;
		}

		public GameObjectType GameObjectType { get; } = GameObjectType.Harbor;
		public ResourceType ResourceType { get; }
		public PointyToppedDirection Direction { get; }
		public Path Path { get; }
		public Tile Tile { get; }

		public bool Equals(Harbor other)
		{
			return Path.Equals(other?.Path);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Harbor);
		}

		public override int GetHashCode()
		{
			return Path.GetHashCode();
		}
	}
}
