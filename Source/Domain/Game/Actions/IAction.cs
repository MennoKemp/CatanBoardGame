namespace CatanBoardGame
{
	public interface IAction
	{
		ActionType ActionType { get; }
		bool CanExecute { get; }
		bool IsAvailable { get; }
	}
}
