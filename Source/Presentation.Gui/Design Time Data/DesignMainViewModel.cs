using CatanBoardGame.Presentation.Gui.ViewModels;

namespace CatanBoardGame.Presentation.Gui.Design
{
	public class DesignMainViewModel : MainViewModel
	{
		public new BoardViewModel BoardViewModel { get; } = new DesignBoardViewModel();

		public new DesignPlayerViewModel PlayerViewModel { get; } = new DesignPlayerViewModel();
	}
}
