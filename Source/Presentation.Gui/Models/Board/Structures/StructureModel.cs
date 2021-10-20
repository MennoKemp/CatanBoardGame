namespace CatanBoardGame.Presentation.Gui.Models
{
	public interface IStructureModel : IGameObjectModel
	{
		Player Owner { get; }
	}

	public abstract class StructureModel<T> : GameObjectModel<T>, IStructureModel where T : IStructure
	{
		protected StructureModel(T structure) 
			: base(structure)
		{
		}

		public Player Owner
		{
			get => GameObject.Owner;
		}
	}
}
