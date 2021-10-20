namespace CatanBoardGame.Engine
{
	internal interface IPaidAction
	{
		ResourceCollection GetCosts(Game game);
	}
}