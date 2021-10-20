using Auxilia.Extensions;
using Ludumia.Grids.Hexagonal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CatanBoardGame.Engine
{
	public class BoardCreator
	{
		private static readonly Dictionary<TileType, int> TileTypes = new Dictionary<TileType, int>
		{
			{ TileType.Desert, 1 },
			{ TileType.Forest, 4 },
			{ TileType.Hills, 3 },
			{ TileType.Pastures, 4 },
			{ TileType.Fields, 4 },
			{ TileType.Mountains, 3 }
		};

		private static readonly Dictionary<ResourceType, int> HarborTypes = new Dictionary<ResourceType, int>
		{
			{ ResourceType.Generic, 4 },
			{ ResourceType.Lumber, 1 },
			{ ResourceType.Brick, 1 },
			{ ResourceType.Wool, 1 },
			{ ResourceType.Grain, 1 },
			{ ResourceType.Ore, 1 }
		};

		private static readonly List<int> ProductionNumbers = new List<int>
		{
			5,
			2,
			6,
			3,
			8,
			10,
			9,
			12,
			11,
			4,
			8,
			10,
			9,
			4,
			5,
			6,
			3,
			11
		};

		private Random _random;

		public Board CreateBoard(int seed)
		{
			_random = new Random(seed);

			Board board = new Board();

			AddTiles(board);
			AddIntersections(board);
			AddPaths(board);
			AddHarbors(board);

			return board;
		}

		private void AddTiles(Board board)
		{
			Queue<TileType> tileTypes = new Queue<TileType>(TileTypes.SelectMany(t => Enumerable.Repeat(t.Key, t.Value))
				.Randomize(_random));

			Queue<int> productionValues = new Queue<int>(ProductionNumbers);

			List<Tile> tiles = new List<Tile>();

			void AddTile(Tile tile)
			{
				tile.TileType = tileTypes.Dequeue();
				tiles.Add(tile);
			}

			AddTile(new Tile(0, 0));

			for (int i = 0; i < 2; i++)
			{
				tiles.ToList().SelectMany(t => HexagonalGridNavigator.GetHexagons(t.Hexagon).Select(h => new Tile(h)))
					.Where(t => !tiles.Contains(t))
					.Execute(AddTile);
			}

			board.Tiles = tiles.ToArray();

			foreach (Tile tile in board.GetSpiral(tiles.Where(board.IsCorner).Randomize(_random).First()))
			{
				if (tile.TileType != TileType.Desert)
					tile.ProductionNumber = productionValues.Dequeue();
			}
		}

		private static void AddIntersections(Board board)
		{
			board.Intersections = board.Tiles.SelectMany(t => HexagonalGridNavigator.GetCorners(t.Hexagon).Select(c => new Intersection(c))).Distinct().ToArray();
		}

		private static void AddPaths(Board board)
		{
			board.Paths = board.Tiles.SelectMany(t => HexagonalGridNavigator.GetEdges(t.Hexagon).Select(e => new Path(e))).Distinct().ToArray();
		}

		private void AddHarbors(Board board)
		{
			Queue<ResourceType> harborTypes = new Queue<ResourceType>(HarborTypes.SelectMany(t => Enumerable.Repeat(t.Key, t.Value))
				.Randomize(_random));

			Tile skippedCorner = board.Tiles.Where(board.IsCorner).Randomize(_random).First();
			Intersection firstHarborIntersection = board.GetIntersections(skippedCorner).Randomize(_random).First(i => board.GetTiles(i).Length == 2);

			List<Path> passedPaths = new List<Path>(board.GetPaths(skippedCorner).Where(board.IsCoastal));
			Path currentPath = board.GetPaths(firstHarborIntersection).First(p => !board.GetTiles(p).Contains(skippedCorner));

			List<Harbor> harbors = new List<Harbor>();
			while (currentPath != null)
			{
				PointyToppedDirection harborToppedDirection = currentPath.Orientation switch
				{
					PointyEdgeOrientation.Vertical => board.GetTiles(currentPath).Single().X < currentPath.X1
						? PointyToppedDirection.Right
						: PointyToppedDirection.Left,
					PointyEdgeOrientation.TopLeftToBottomRight => board.GetTiles(currentPath).Single().Y < currentPath.Y1
						? PointyToppedDirection.BottomLeft
						: PointyToppedDirection.TopRight,
					PointyEdgeOrientation.BottomLeftToTopRight => board.GetTiles(currentPath).Single().Y < currentPath.Y1
						? PointyToppedDirection.BottomRight
						: PointyToppedDirection.TopLeft
				};

				if (((passedPaths.Count - 3) % 10).IsAnyOf(0, 3, 6))
					harbors.Add(new Harbor(harborTypes.Dequeue(), harborToppedDirection, currentPath, board.GetTiles(currentPath).Single()));

				passedPaths.Add(currentPath);
				currentPath = board.GetPaths(currentPath).FirstOrDefault(p => board.IsCoastal(p) && !passedPaths.Contains(p));
			}

			board.Harbors = harbors.ToArray();
		}
	}
}
