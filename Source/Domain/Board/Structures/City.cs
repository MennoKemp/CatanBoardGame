using Auxilia.Extensions;
using System;

namespace CatanBoardGame
{
	public class City : ISettlement, IEquatable<City>
	{
		public City(Player owner, Intersection intersection)
		{
			Owner = owner.ThrowIfNull(nameof(owner));
			Intersection = intersection.ThrowIfNull(nameof(intersection));
		}

		public GameObjectType GameObjectType { get; } = GameObjectType.City;

		public Player Owner { get; }
		public Intersection Intersection { get; }

		public bool Equals(City other)
		{
			return Intersection.Equals(other?.Intersection);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as City);
		}

		public override int GetHashCode()
		{
			return Intersection.GetHashCode();
		}
	}
}
