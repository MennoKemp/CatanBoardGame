using Auxilia.Extensions;
using Ludumia.Grids.Hexagonal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CatanBoardGame
{
	public class Board
	{
		public Robber Robber { get; set; } = new Robber();

		public Tile[] Tiles
		{
			get => _tiles;
			set
			{
				_tiles = value ?? Array.Empty<Tile>();
				Robber.Tile ??= _tiles.SingleOrDefault(t => t.TileType == TileType.Desert);
			}
		}
		private Tile[] _tiles = Array.Empty<Tile>();

		public Intersection[] Intersections
		{
			get => _intersections;
			set => _intersections = value ?? Array.Empty<Intersection>();
		}
		private Intersection[] _intersections = Array.Empty<Intersection>();

		public Path[] Paths
		{
			get => _paths;
			set => _paths = value ?? Array.Empty<Path>();
		}
		private Path[] _paths = Array.Empty<Path>();

		public Harbor[] Harbors
		{
			get => _harbors;
			set => _harbors = value ?? Array.Empty<Harbor>();
		}
		private Harbor[] _harbors = Array.Empty<Harbor>();

		public IEnumerable<Road> Roads
		{
			get => Paths.Select(p => p.Road).WhereNotNull();
		}
		public IEnumerable<Village> Villages
		{
			get => Intersections.Select(i => i.Settlement).OfType<Village>();
		}
		public IEnumerable<City> Cities
		{
			get => Intersections.Select(i => i.Settlement).OfType<City>();
		}

		public Tile this[Tile tile]
		{
			get => Tiles.FirstOrDefault(t => t.Equals(tile));
		}
		private Tile this[Hexagon hexagon]
		{
			get => Tiles.FirstOrDefault(t => t.Hexagon.Equals(hexagon));
		}
		public Intersection this[Intersection intersection]
		{
			get => Intersections.FirstOrDefault(i => i.Equals(intersection));
		}
		private Intersection this[Corner corner]
		{
			get => Intersections.FirstOrDefault(i => i.Corner.Equals(corner));
		}
		public Path this[Path path]
		{
			get => Paths.FirstOrDefault(p => p.Equals(path));
		}
		private Path this[Edge edge]
		{
			get => Paths.FirstOrDefault(p => p.Edge.Equals(edge));
		}

		public Tile[] GetTiles(Tile tile)
		{
			return HexagonalGridNavigator.GetHexagons(tile.Hexagon).Select(h => this[h]).WhereNotNull().ToArray();
		}
		public Tile[] GetTiles(Intersection intersection)
		{
			return HexagonalGridNavigator.GetHexagons(intersection.Corner).Select(h => this[h]).WhereNotNull().ToArray();
		}
		public Tile[] GetTiles(Path path)
		{
			return HexagonalGridNavigator.GetHexagons(path.Edge).Select(h => this[h]).WhereNotNull().ToArray();
		}

		public Intersection[] GetIntersections(Tile tile)
		{
			return HexagonalGridNavigator.GetCorners(tile.Hexagon).Select(c => this[c]).WhereNotNull().ToArray();
		}
		public Intersection[] GetIntersections(Intersection intersection)
		{
			return HexagonalGridNavigator.GetCorners(intersection.Corner).Select(c => this[c]).WhereNotNull().ToArray();
		}
		public Intersection[] GetIntersections(Path path)
		{
			return HexagonalGridNavigator.GetCorners(path.Edge).Select(c => this[c]).WhereNotNull().ToArray();
		}

		public Path[] GetPaths(Tile tile)
		{
			return HexagonalGridNavigator.GetEdges(tile.Hexagon).Select(e => this[e]).WhereNotNull().ToArray();
		}
		public Path[] GetPaths(Intersection intersection)
		{
			return HexagonalGridNavigator.GetEdges(intersection.Corner).Select(e => this[e]).WhereNotNull().ToArray();
		}
		public Path[] GetPaths(Path path)
		{
			return HexagonalGridNavigator.GetEdges(path.Edge).Select(e => this[e]).WhereNotNull().ToArray();
		}

		public Harbor[] GetHarbors(Tile tile)
		{
			return  GetIntersections(tile).SelectMany(GetHarbors).Distinct().ToArray();
		}
		public Harbor[] GetHarbors(Intersection intersection)
		{
			return GetPaths(intersection).SelectMany(GetHarbors).Distinct().ToArray();
		}
		public Harbor[] GetHarbors(Path path)
		{
			return Harbors.Where(h => h.Path.Equals(path)).ToArray();
		}

		public bool IsCorner(Tile tile)
		{
			return GetTiles(tile).Length == 3;
		}
		public bool IsEdge(Tile tile)
		{
			return GetTiles(tile).Length == 4;
		}
		public bool IsCenter(Tile tile)
		{
			return tile.Hexagon.Coordinates.Equals(new Point(0, 0));
		}
		public bool IsInnerCircle(Tile tile)
		{
			return GetTiles(tile).Length == 6 && !IsCenter(tile);
		}

		public bool IsCoastal(Tile tile)
		{
			return GetTiles(tile).Length < 6;
		}
		public bool IsCoastal(Intersection intersection)
		{
			return GetTiles(intersection).Length < 3;
		}
		public bool IsCoastal(Path path)
		{
			return GetTiles(path).Length < 2;
		}

		public Tile[] GetCoastalTiles(Tile start)
		{
			if (!IsCorner(start))
				throw new ArgumentException("Tile is not coastal.", nameof(start));

			List<Tile> coastalTiles = new List<Tile> { this[start] };

			while (GetTiles(coastalTiles.Last()).FirstOrDefault(t => IsCoastal(t) && !coastalTiles.Contains(t)) is Tile tile)
				coastalTiles.Add(tile);

			return coastalTiles.ToArray();
		}
		public Tile[] GetInnerCircleTiles(Tile start)
		{
			if (!IsInnerCircle(start))
				throw new ArgumentException("Tile is in the inner circle.", nameof(start));

			List<Tile> coastalTiles = new List<Tile> { this[start] };

			while (GetTiles(coastalTiles.Last()).FirstOrDefault(t => IsInnerCircle(t) && !coastalTiles.Contains(t)) is Tile tile)
				coastalTiles.Add(tile);

			return coastalTiles.ToArray();
		}
		public Tile[] GetSpiral(Tile start)
		{
			List<Tile> spiral = GetCoastalTiles(start).ToList();
			spiral.AddRange(GetInnerCircleTiles(GetTiles(start).Single(IsInnerCircle)));
			spiral.Add(Tiles.Single(IsCenter));

			return spiral.ToArray();
		}
	}
}
