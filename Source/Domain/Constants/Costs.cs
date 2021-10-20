namespace CatanBoardGame
{
	public static class Costs
	{
		public static readonly ResourceCollection Road = new ResourceCollection(new[]
		{
			ResourceType.Lumber,
			ResourceType.Brick
		});

		public static readonly ResourceCollection Village = new ResourceCollection(new[]
		{
			ResourceType.Lumber,
			ResourceType.Brick,
			ResourceType.Wool,
			ResourceType.Grain
		});

		public static readonly ResourceCollection City = new ResourceCollection(new[]
		{
			ResourceType.Grain,
			ResourceType.Grain,
			ResourceType.Ore,
			ResourceType.Ore,
			ResourceType.Ore
		});

		public static readonly ResourceCollection DevelopmentCard = new ResourceCollection(new[]
		{
			ResourceType.Wool,
			ResourceType.Grain,
			ResourceType.Ore
		});
	}
}
