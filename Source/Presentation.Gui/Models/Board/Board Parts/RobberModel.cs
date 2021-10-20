namespace CatanBoardGame.Presentation.Gui.Models
{
	public class RobberModel : GameObjectModel<Robber>
	{
		public RobberModel(Robber robber) 
			: base(robber)
		{
		}

		public Tile Tile
		{
			get => GameObject.Tile;
		}
	}
}
