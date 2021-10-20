using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace CatanBoardGame.Engine
{
	public interface IMultipleChoiceAction : IAction, INotifyCollectionChanged
	{
		GameObjectType OptionType { get; }
		int SelectionSize { get; }
		bool CanCancel { get; }

		bool IsSelected { get; set; }

		void Update(Game game);

		IEnumerable<IGameObject> GetOptions();

		IEnumerable<IGameObject> GetSelection();
		void SetSelection(IEnumerable<IGameObject> selection);
	}

	internal abstract class MultipleChoiceAction : Action, IMultipleChoiceAction
	{
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public abstract GameObjectType OptionType { get; }
		
		public virtual int SelectionSize { get; } = 1;

		public bool IsSelected { get; set; }
		public bool CanCancel { get; protected set; } = true;

		public abstract void Update(Game game);

		public abstract IEnumerable<IGameObject> GetOptions();

		public abstract IEnumerable<IGameObject> GetSelection();
		public abstract void SetSelection(IEnumerable<IGameObject> selection);

		public abstract void ClearSelection();
		
		protected void OnSelectionChanged()
		{
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		public void Reset()
		{
			IsSelected = false;
			ClearSelection();
		}
	}

	internal abstract class MultipleChoiceAction<T> : MultipleChoiceAction where T : IGameObject
	{
		public IEnumerable<T> Options { get; protected set; }

		public IEnumerable<T> Selection
		{
			get => _selection;
			set
			{
				_selection = value.ToList();
				OnSelectionChanged();
			}
		}
		private IEnumerable<T> _selection = Enumerable.Empty<T>();

		public override IEnumerable<IGameObject> GetOptions()
		{
			return (IEnumerable<IGameObject>)Options;
		}
		
		public override IEnumerable<IGameObject> GetSelection()
		{
			return Selection.Cast<IGameObject>();
		}
		public override void SetSelection(IEnumerable<IGameObject> selection)
		{
			Selection = selection.Cast<T>();
		}

		public override void ClearSelection()
		{
			_selection = Enumerable.Empty<T>();
		}
	}
}
