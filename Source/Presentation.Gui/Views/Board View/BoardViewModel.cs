using Auxilia.Presentation.ViewModels;
using CatanBoardGame.Engine;
using CatanBoardGame.Presentation.Gui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CatanBoardGame.Presentation.Gui.ViewModels
{
	public class BoardViewModel : ViewModelBase
	{
		public event EventHandler<SelectedGamePiecesChangedEventArgs> SelectionChanged;

		public int SelectionSize
		{
			get => _selectionSize;
			set => SetProperty(ref _selectionSize, value);
		}
		private int _selectionSize;

		public IEnumerable<IGameObjectModel> GameObjects
		{
			get => _gameObjects;
			private set => SetProperty(ref _gameObjects, value.OrderBy(p => p.GameObject is ISettlement));
		}
		private IEnumerable<IGameObjectModel> _gameObjects;

		public IEnumerable<IGameObjectModel> SelectedGameObjects
		{
			get => _selectedGameObjects;
			set
			{
				SetProperty(ref _selectedGameObjects, value);
				SelectionChanged?.Invoke(this, new SelectedGamePiecesChangedEventArgs(_selectedGameObjects.Select(m => m.GameObject)));
			}
		}
		private IEnumerable<IGameObjectModel> _selectedGameObjects;

		public void Update(Board board, IMultipleChoiceAction currentAction)
		{
			SelectionSize = currentAction?.SelectionSize ?? 0;

			List<IGameObjectModel> gameObjects = new List<IGameObjectModel>();
			gameObjects.AddRange(board.Tiles.Select(t => new TileModel(t)));
			gameObjects.AddRange(board.Paths.Select(p => new PathModel(p)));
			gameObjects.AddRange(board.Harbors.Select(h => new HarborModel(h)));
			gameObjects.Add(new RobberModel(board.Robber));

			gameObjects.AddRange(board.Roads.Select(r => new RoadModel(r)));
			gameObjects.AddRange(board.Villages.Select(v => new VillageModel(v)));
			gameObjects.AddRange(board.Cities.Select(c => new CityModel(c)));

			if (currentAction != null)
				AddActionOptions(gameObjects, currentAction);

			Application.Current.Dispatcher.Invoke(() => GameObjects = gameObjects);
		}

		private static void AddActionOptions(List<IGameObjectModel> gamePieces, IMultipleChoiceAction action)
		{
			List<IGameObject> options = action.GetOptions().ToList();
			List<IGameObject> selection = action.GetSelection().ToList();

			switch (action.OptionType)
			{
				case GameObjectType.Road:
				{
					IEnumerable<Path> paths = options.OfType<Road>().Select(r => r.Path);
					gamePieces.RemoveAll(m => m is RoadModel r && paths.Contains(r.GameObject.Path));
					gamePieces.AddRange(options.OfType<Road>().Select(r => new RoadModel(r)
					{
						IsSelectable = true,
						IsSelected = selection.Contains(r)
					}));
						
					break;
				}
				case GameObjectType.Village:
				case GameObjectType.City:
				{
					IEnumerable<Intersection> intersections = options.OfType<ISettlement>().Select(s => s.Intersection);
					gamePieces.RemoveAll(m => m is VillageModel village && intersections.Contains(village.GameObject.Intersection));
					gamePieces.RemoveAll(m => m is CityModel city && intersections.Contains(city.GameObject.Intersection));

					if (action.OptionType == GameObjectType.Village)
					{
						gamePieces.AddRange(options.OfType<Village>().Select(v => new VillageModel(v)
						{
							IsSelectable = true,
							IsSelected = selection.Contains(v)
						}));
					}
					else
					{
						gamePieces.AddRange(options.OfType<City>().Select(c => new CityModel(c)
						{
							IsSelectable = true,
							IsSelected = selection.Contains(c)
						}));
					}

					break;
				}
			}
		}
	}
}
