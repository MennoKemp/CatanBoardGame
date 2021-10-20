namespace CatanBoardGame
{
	public interface IStructure : IGameObject
	{
		Player Owner { get; }
	}
}
