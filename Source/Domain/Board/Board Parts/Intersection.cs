using Ludumia.Grids.Hexagonal;
using System;

namespace CatanBoardGame
{
	public class Intersection : IGameObject, IEquatable<Intersection>
	{
		public Intersection(int x, int y)
		{
			Corner = new Corner(x, y);
		}
		public Intersection(Corner edge)
		{
			Corner = edge;
		}

		public Corner Corner { get; }

		public bool IsHigh
		{
			get => Corner.IsHigh;
		}

		public int X
		{
			get => Corner.X;
		}
		public int Y
		{
			get => Corner.Y;
		}

		public GameObjectType GameObjectType { get; } = GameObjectType.Intersection;

		public ISettlement Settlement { get; set; }

		public bool Equals(Intersection other)
		{
			return Corner.Equals(other?.Corner);
		}

		public override bool Equals(object obj)
		{ 
			return Equals(obj as Intersection);
		}

		public override int GetHashCode()
		{
			return Corner.GetHashCode();
		}

		public override string ToString()
		{
			return Corner.ToString();
		}
	}
}
