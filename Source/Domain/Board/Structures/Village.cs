using System;
using Auxilia.Extensions;

namespace CatanBoardGame
{
	public class Village : ISettlement, IEquatable<Village>
	{
		public Village(Player owner, Intersection intersection)
		{
			Owner = owner.ThrowIfNull(nameof(owner));
			Intersection = intersection.ThrowIfNull(nameof(intersection));
		}

		public GameObjectType GameObjectType { get; } = GameObjectType.Village;

		public Player Owner { get; }
		public Intersection Intersection { get; }

		public bool Equals(Village other)
		{
			return Intersection.Equals(other?.Intersection);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Village);
		}

		public override int GetHashCode()
		{
			return Intersection.GetHashCode();
		}
	}
}