using Auxilia.Extensions;
using Ludumia.Grids.Hexagonal;
using Ludumia.NumberGeneration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CatanBoardGame.Engine
{
	public class GameEngine
	{
		internal static Random Random = new Random();

		private readonly BoardCreator _boardCreator = new BoardCreator();
		private readonly ResourceManager _resourceManager = new ResourceManager();

		public GameEngine()
		{
			HexagonGeometry.GridOrientation = GridOrientation.PointyTopped;
			Actions = CreateActions();
		}

		public event EventHandler StateChanged;

		public Game Game { get; private set; }
		public Board Board
		{
			get => Game.Board;
		}
		public Player ActivePlayer
		{
			get => Game.ActivePlayer;
		}

		public ActionCollection Actions { get; }
		public IMultipleChoiceAction CurrentAction
		{
			get => Actions.OfType<IMultipleChoiceAction>().SingleOrDefault(a => a.IsSelected);
		}

		public ObservableCollection<Event> Events { get; } = new ObservableCollection<Event>();
		
		public void CreateGame(int seed, Player[] players)
		{
			players.ThrowIfNullEmptyOrContainsNull(nameof(players));
			Random = new Random(seed);

			Game = new Game
			{
				Seed = seed,
				Dice = new Dice(2, seed),
				Board = _boardCreator.CreateBoard(seed),
				Players = players.Randomize(Random).ToArray(),
				ActivePlayer = players.First(),
				CurrentPhase = Phase.Colonization,
				Actions = new List<IAction>()
			};
			
			Actions.OfType<BuildVillageAction>().Execute(a => a.IsSelected = true);
			Update();
		}

		public void CancelAction()
		{
			if(CurrentAction?.CanCancel ?? true)
				return;

			((MultipleChoiceAction)CurrentAction).IsSelected = false;
			Update();
		}

		public void ExecuteAction(IAction action)
		{
			if(action is not Action { CanExecute: true } actionToExecute)
				return;

			Event lastEvent = actionToExecute.Execute(Game);
			Events.Add(lastEvent);
			Game.Actions.Add(action);
			Actions.OfType<MultipleChoiceAction>().Execute(a => a.Reset());

			switch (Game.LastAction?.ActionType)
			{
				case ActionType.BuildRoad:
				{
					if (Game.CurrentPhase != Phase.Colonization)
						break;

					if (Game.ActivePlayer.Villages.Count == 2)
						_resourceManager.DealInitialResources(Game, ActivePlayer);

					int roadCount = Game.Players.Sum(p => p.Roads.Count);

					if (roadCount < Game.Players.Count * 2)
					{
						if (roadCount != Game.Players.Count)
						{
							Game.ActivePlayer = roadCount < Game.Players.Count
								? Game.NextPlayer
								: Game.PreviousPlayer;
						}

						Actions.GetAction<BuildVillageAction>().IsSelected = true;
					}
					else
					{
						Game.CurrentPhase = Phase.Production;
					}

					break;
				}
				case ActionType.BuildVillage:
				{
					Actions.GetAction<BuildRoadAction>().IsSelected = true;

					break;
				}
			}

			Update();
			StateChanged?.Invoke(this, new EventArgs());
		}
		
		public void Update()
		{
			Actions.Cast<Action>().Execute(a => a.Validate(Game));
		}

		private ActionCollection CreateActions()
		{
			ActionCollection actions = new ActionCollection(GetType().Assembly
				.GetTypes()
				.Where(t => typeof(Action).IsAssignableFrom(t) && !t.IsAbstract)
				.Select(t => (IAction)Activator.CreateInstance(t)));

			actions.OfType<IMultipleChoiceAction>().Execute(a => a.CollectionChanged += (_, _) => Update());

			return actions;
		}
	}
}
