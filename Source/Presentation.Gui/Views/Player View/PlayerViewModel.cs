using Auxilia.Extensions;
using Auxilia.Presentation.ViewModels;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace CatanBoardGame.Presentation.Gui.ViewModels
{
	public class PlayerViewModel : ViewModelBase
	{
		private readonly NotifyCollectionChangedEventHandler _resourcesChangedHandler;

		public PlayerViewModel()
		{
			_resourcesChangedHandler = (_, _) => Update(Players);
		}

		public Player[] Players
		{
			get => _players;
			set
			{
				_players?.Execute(p => p.Resources.CollectionChanged -= _resourcesChangedHandler);
				SetProperty(ref _players, value);
				_players.Execute(p => p.Resources.CollectionChanged += _resourcesChangedHandler);
			}
		}

		private Player[] _players;

		public void Update(IList<Player> players)
		{
			Players = players.ToArray();
		}
	}
}
