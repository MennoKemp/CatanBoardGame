using System;
using System.Windows.Controls;

namespace CatanBoardGame.Presentation.Gui
{
	public class OverlappingStackPanel : StackPanel
	{
		public OverlappingStackPanel()
		{
			LayoutUpdated += (_, _) => OnLayoutUpdated(this, new EventArgs());
		}

		private void OnLayoutUpdated(object sender, EventArgs e)
		{
			if (sender is OverlappingStackPanel stackPanel)
			{
				
			}
		}
	}
}
