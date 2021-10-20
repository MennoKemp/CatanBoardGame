using Auxilia.Extensions;
using System;

namespace CatanBoardGame
{
	public class Road : IStructure, IEquatable<Road>
	{
		public Road(Player owner, Path path)
		{
			Owner = owner.ThrowIfNull(nameof(owner));
			Path = path.ThrowIfNull(nameof(path));
		}

		public GameObjectType GameObjectType { get; } = GameObjectType.Road;

		public Player Owner { get; }
		public Path Path { get; }

		public bool Equals(Road other)
		{
			return Path.Equals(other?.Path);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Road);
		}

		public override int GetHashCode()
		{
			return Path.GetHashCode();
		}
	}
}
