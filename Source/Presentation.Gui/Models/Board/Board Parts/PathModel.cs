using System.Windows;

namespace CatanBoardGame.Presentation.Gui.Models
{
	public class PathModel : GameObjectModel<Path>
	{
		public PathModel(Path path)
			: base(path)
		{
		}

		public Point Center
		{
			get => new Point((GameObject.X1 + GameObject.X2) / 2.0, (GameObject.Y1 + GameObject.Y2) / 2.0);
		}
	}
}
