using Auxilia;
using Auxilia.Extensions;
using System;

namespace CatanBoardGame.Presentation.Gui.Models
{
	public interface IGameObjectModel : IEquatable<IGameObjectModel>
	{
		IGameObject GameObject { get; }

		bool IsHighlighted { get; set; }
		bool IsSelectable { get; set; }
		bool IsSelected { get; set; }

		Type Type { get; }
	}

	public abstract class GameObjectModel<T> : ObservableObject, IEquatable<IGameObjectModel>, IGameObjectModel where T : IGameObject
	{
		protected GameObjectModel(T gamePiece)
		{
			GameObject = gamePiece.ThrowIfNull(nameof(gamePiece));
		}
		
		public T GameObject { get; }

		public bool IsHighlighted
		{
			get => _isHighlighted;
			set => SetProperty(ref _isHighlighted, value);
		}
		private bool _isHighlighted;

		public bool IsSelectable
		{
			get => _isSelectable;
			set
			{
				IsHighlighted = true;
				SetProperty(ref _isSelectable, value);
			}
		}
		private bool _isSelectable;

		public bool IsSelected
		{
			get => _isSelected;
			set => SetProperty(ref _isSelected, value);
		}
		private bool _isSelected;

		public Type Type
		{
			get => typeof(T);
		}

		IGameObject IGameObjectModel.GameObject
		{
			get => GameObject;
		}

		public bool Equals(IGameObjectModel other)
		{
			return GameObject.Equals(other?.GameObject);
		}
	}
}
