using Ludumia.Grids.Hexagonal;
using System;

namespace CatanBoardGame
{
	public class Path : IGameObject, IEquatable<Path>
	{
		public Path(int x1, int y1, int x2, int y2)
		{
			Edge = new Edge(x1, y1, x2, y2);
		}
		public Path(Edge edge)
		{
			Edge = edge;
		}

		public GameObjectType GameObjectType { get; } = GameObjectType.Path;

		public Edge Edge { get; }

		public PointyEdgeOrientation Orientation
		{
			get => Edge.Orientation;
		}
		public int X1
		{
			get => Edge.X1;
		}
		public int Y1
		{
			get => Edge.Y1;
		}
		public int X2
		{
			get => Edge.X2;
		}
		public int Y2
		{
			get => Edge.Y2;
		}
		
		public Road Road { get; set; }

		public bool Equals(Path other)
		{
			return Edge.Equals(other?.Edge);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Path);
		}

		public override int GetHashCode()
		{
			return Edge.GetHashCode();
		}

		public override string ToString()
		{
			return Edge.ToString();
		}
	}
}
