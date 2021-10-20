using CatanBoardGame.Presentation.Gui.ViewModels;
using System.Windows.Media;

namespace CatanBoardGame.Presentation.Gui.Design
{
	public class DesignPlayerViewModel : PlayerViewModel
	{
		public new Player[] Players { get; } =
		{
			new Player("Red", Colors.Red, new ResourceCollection(new []
			{
				ResourceType.Lumber,
				ResourceType.Brick,
				ResourceType.Wool,
				ResourceType.Grain,
				ResourceType.Ore
			})),
			new Player("Blue", Colors.Blue),
			new Player("Orange", Colors.Orange),
			new Player("White", Colors.White)
		};
	}
}
