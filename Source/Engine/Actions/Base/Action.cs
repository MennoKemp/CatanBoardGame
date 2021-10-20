using Auxilia.Extensions;
using System.Linq;

namespace CatanBoardGame.Engine
{
	internal abstract class Action : IAction
	{
		public abstract ActionType ActionType { get; }
		public abstract Phase[] ValidPhases { get; }

		public bool IsAvailable
		{
			get => CanExecute || IsSelectable;
		}
		public bool CanExecute { get; private set; }
		public bool IsSelectable { get; private set; }

		public bool CanUndo { get; } = false;
		
		public void Validate(Game game)
		{
			CanExecute = game.CurrentPhase.IsAnyOf(ValidPhases) &&
			               (this is not IPaidAction paidAction || game.ActivePlayer.CanAfford(paidAction.GetCosts(game))) &&
			               CanExecuteAction(game);

			if (this is IMultipleChoiceAction multipleChoiceAction)
			{
				multipleChoiceAction.Update(game);
				IsSelectable = CanExecute && multipleChoiceAction.GetOptions().Any();
				CanExecute = multipleChoiceAction.IsSelected
					? multipleChoiceAction.GetSelection().Count() == multipleChoiceAction.SelectionSize
					: CanExecute;
			}
		}
		
		public Event Execute(Game game)
		{
			if (this is IPaidAction paidAction)
				game.ActivePlayer.Resources.Remove(paidAction.GetCosts(game));

			return ExecuteAction(game);
		}
		
		protected abstract Event ExecuteAction(Game game);

		protected abstract bool CanExecuteAction(Game game);
	}
}
