using Ludumia.Grids.Hexagonal;
using System;
using Auxilia.Extensions;

namespace CatanBoardGame
{
	public class Tile : IGameObject, IEquatable<Tile>
	{
		public Tile(int x, int y)
		{
			Hexagon = new Hexagon(x, y);
		}
		public Tile(Hexagon hexagon)
		{
			Hexagon = hexagon;
		}

		public GameObjectType GameObjectType { get; } = GameObjectType.Tile;

		public Hexagon Hexagon { get; }

		public int X
		{
			get => Hexagon.X;
		}
		public int Y
		{
			get => Hexagon.Y;
		}

		public TileType TileType { get; set; }
		public int ProductionNumber { get; set; }
		public int Yield
		{
			get => 6 - Math.Abs(ProductionNumber - 7);
		}

		public ResourceType? GetResource()
		{
			return !TileType.IsAnyOf(TileType.Desert, TileType.Sea)
				? TileType.ToResourceType()
				: null;
		}

		public bool Equals(Tile other)
		{
			return Hexagon.Equals(other?.Hexagon);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Tile);
		}

		public override int GetHashCode()
		{
			return Hexagon.GetHashCode();
		}

		public override string ToString()
		{
			return Hexagon.ToString();
		}
	}
}
