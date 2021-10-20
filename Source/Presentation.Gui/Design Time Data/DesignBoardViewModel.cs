using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using CatanBoardGame.Engine;
using CatanBoardGame.Presentation.Gui.ViewModels;
using System.Windows.Media;
using Auxilia.Extensions;

namespace CatanBoardGame.Presentation.Gui.Design
{
	public class DesignBoardViewModel : BoardViewModel
	{
		private static readonly Board Board = new BoardCreator().CreateBoard(0);

		private static readonly Player[] Players =
		{
			new Player("Red", Colors.Red),
			new Player("Blue", Colors.Blue),
			new Player("Orange", Colors.Orange),
			new Player("White", Colors.White)
		};

		private class DesignAction : IMultipleChoiceAction
		{
			private static readonly IGameObject[] Options = 
			{
				new Road(Players[0], Board[new Path(0, 0, 0, 1)]),
				new Road(Players[0], Board[new Path(0, 1, 1, 1)]),
				new Road(Players[0], Board[new Path(1, 1, 2, 1)]),
				new Road(Players[0], Board[new Path(2, 1, 2, 0)]),
				new Road(Players[0], Board[new Path(2, 0, 1, 0)]),
				new Road(Players[0], Board[new Path(1, 0, 0, 0)]),
			};

			public ActionType ActionType { get; } = ActionType.BuildRoad;
			public bool CanExecute { get; } = true;
			public bool IsAvailable { get; } = true;
			public event NotifyCollectionChangedEventHandler CollectionChanged;
			public GameObjectType OptionType { get; } = GameObjectType.Road;
			public int SelectionSize { get; } = 1;
			public bool CanCancel { get; } = true;
			public bool IsSelected { get; set; }
			public void Update(Game game)
			{
			}

			public IEnumerable<IGameObject> GetOptions()
			{
				return Options;
			}

			public IEnumerable<IGameObject> GetSelection()
			{
				return Options.First().Yield();
			}

			public void SetSelection(IEnumerable<IGameObject> selection)
			{
			}
		}

		public DesignBoardViewModel() 
		{
			Update(CreateBoard(), new DesignAction());
		}

		public static Board CreateBoard()
		{
			static void AddRoad(int playerIndex, params int[] coordinates)
			{
				Path path = Board[new Path(coordinates[0], coordinates[1], coordinates[2], coordinates[3])];
				path.Road = new Road(Players[playerIndex], path);
			}

			AddRoad(0, -2, 1, -1, 1);
			AddRoad(0, 3, 1, 3, 2);
			AddRoad(1, 0, -1, 1, -1);
			AddRoad(1, 1, -1, 1, 0);
			AddRoad(1, -2, 2, -1, 2);
			AddRoad(2, 4, 0, 3, 0);
			AddRoad(2, 0, 3, 1, 3);
			AddRoad(3, -1, 0, -2, 0);
			AddRoad(3, 3, -1, 2, -1);
			AddRoad(3, 2, -1, 1, -1);
			AddRoad(3, 1, 0, 0, 0);
			AddRoad(3, 0, 0, -1, 0);

			static void AddVillage(int playerIndex, params int[] coordinates)
			{
				Intersection intersection = Board[new Intersection(coordinates[0], coordinates[1])];
				intersection.Settlement = new Village(Players[playerIndex], intersection);
			}

			AddVillage(0, -2, 1);
			AddVillage(1, 0, -1);
			AddVillage(1, 1, 0);
			AddVillage(2, 4, 0);
			AddVillage(3, -1, 0);

			static void AddCity(int playerIndex, params int[] coordinates)
			{
				Intersection intersection = Board[new Intersection(coordinates[0], coordinates[1])];
				intersection.Settlement = new City(Players[playerIndex], intersection);
			}

			AddCity(0, 3, 1);
			AddCity(1, -2, 2);
			AddCity(2, 0, 3);
			AddCity(3, 3, -1);
			
			return Board;
		}
	}
}
