using Auxilia.Delegation.Commands;
using CatanBoardGame.Engine;
using CatanBoardGame.Presentation.Gui.Models;
using Ludumia.NumberGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

namespace CatanBoardGame.Presentation.Gui.ViewModels
{
	public class MainViewModel : Auxilia.Presentation.ViewModels.MainViewModelBase
	{
		private readonly EventHandler<SelectedGamePiecesChangedEventArgs> _selectedGamePiecesChangedHandler;
		private readonly EventHandler _gameStateChangedHandler;

		public MainViewModel()
		{
			_selectedGamePiecesChangedHandler = OnSelectionChanged;
			BoardViewModel.SelectionChanged += _selectedGamePiecesChangedHandler;

			_gameStateChangedHandler = (_, _) => Update();
			GameEngine = new GameEngine();
			GameEngine.StateChanged += _gameStateChangedHandler;

			GameEngine.CreateGame(0, new[]
			{
				new Player("Red", Colors.Red),
				new Player("Blue", Colors.Blue),
				new Player("Orange", Colors.Orange),
				new Player("White", Colors.White),
			});
			
			Dice.Set(3, 4);
			Update();
		}

		public GameEngine GameEngine { get; }
		
		public Dice Dice { get; } = new Dice(2, 2);

		public BoardViewModel BoardViewModel { get; } = new BoardViewModel();

		public PlayerViewModel PlayerViewModel { get; } = new PlayerViewModel();
		
		public bool CanExecuteAction
		{
			get => _canExecuteAction;
			private set => SetProperty(ref _canExecuteAction, value);
		}
		private bool _canExecuteAction;

		public IEnumerable<IAction> Actions
		{
			get => GameEngine.Actions;
		}

		public ICommand ExecuteActionCommand
		{
			get => new ActionCommand<IAction>(
				ExecuteAction,
				a => a.IsAvailable && GameEngine.CurrentAction == null);
		}

		public ICommand ConfirmActionCommand
		{
			get => new ActionCommand(
				() => GameEngine.ExecuteAction(GameEngine.CurrentAction), 
				() => GameEngine.CurrentAction is IMultipleChoiceAction { CanExecute: true });
		}

		public ICommand CancelActionCommand
		{
			get => new ActionCommand(
				GameEngine.CancelAction,
				() => GameEngine.CurrentAction is IMultipleChoiceAction { CanCancel: true });
		}

		public string ActionInstructions
		{
			get => _actionInstructions;
			private set => SetProperty(ref _actionInstructions, value);
		}
		private string _actionInstructions;

		protected override void ProtectedDispose(bool isDisposing)
		{
			if(BoardViewModel != null)
				BoardViewModel.SelectionChanged -= _selectedGamePiecesChangedHandler;
			
			if(GameEngine != null)
				GameEngine.StateChanged -= _gameStateChangedHandler;
		}

		private void ExecuteAction(IAction action)
		{
			if (action is IMultipleChoiceAction { IsSelected: false } multipleChoiceAction)
				multipleChoiceAction.IsSelected = true;
			else
				GameEngine.ExecuteAction(action);
		}

		private void Update()
		{
			BoardViewModel.Update(GameEngine.Board, GameEngine.CurrentAction);
			PlayerViewModel.Update(GameEngine.Game.Players);

			ActionInstructions = Properties.Resources.ResourceManager.GetString($"{GameEngine.CurrentAction?.ActionType}");
		}

		private void OnSelectionChanged(object sender, SelectedGamePiecesChangedEventArgs e)
		{
			GameEngine.CurrentAction.SetSelection(BoardViewModel.SelectedGameObjects.Select(p => p.GameObject));
			GameEngine.Update();
			Update();
		}
	}
}