using Auxilia.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace CatanBoardGame
{
	public class Player : IGameObject
	{
		public Player(string name, Color color)
		{
			Name = name.ThrowIfNullOrWhiteSpace(nameof(name));
			Color = color;
		}
		public Player(string name, Color color, ResourceCollection resources)
			: this(name, color)
		{
			Resources = resources;
		}

		public GameObjectType GameObjectType { get; } = GameObjectType.Player;
		public string Name { get; set; }
		public Color Color { get; set; }

		public ResourceCollection Resources { get; } = new ResourceCollection();

		public IList<Road> Roads { get; } = new List<Road>();
		public IList<Village> Villages { get; } = new List<Village>();
		public IList<City> Cities { get; } = new List<City>();

		public bool CanAfford(ResourceCollection costs)
		{
			return costs.GroupBy(c => c).All(g => Resources[g.Key] >= g.Count());
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
