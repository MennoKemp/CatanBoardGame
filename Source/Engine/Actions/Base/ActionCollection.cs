using Auxilia.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CatanBoardGame.Engine
{
	public class ActionCollection : IEnumerable<IAction>
	{
		private readonly List<IAction> _actions;

		public ActionCollection(IEnumerable<IAction> actions)
		{
			_actions = actions
				.ThrowIfNull(nameof(actions))
				.ThrowIfContainsDuplicates(nameof(actions))
				.ToList();
		}
		
		public IAction this[ActionType actionType]
		{
			get => _actions.SingleOrDefault(a => a.ActionType == actionType);
		}

		internal T GetAction<T>() where T : Action
		{
			return _actions.OfType<T>().FirstOrDefault();
		}

		public IEnumerator<IAction> GetEnumerator()
		{
			return _actions.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
