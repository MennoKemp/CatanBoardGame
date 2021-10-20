using Auxilia.Extensions;
using System;
using System.Collections.Generic;

namespace CatanBoardGame.Presentation.Gui.Models
{
	public class SelectedGamePiecesChangedEventArgs : EventArgs
	{
		public SelectedGamePiecesChangedEventArgs(IEnumerable<IGameObject> selectedGamePieces)
		{
			SelectedGamePieces = selectedGamePieces.ThrowIfNull(nameof(selectedGamePieces));
		}

		public IEnumerable<IGameObject> SelectedGamePieces { get; }
	}
}
