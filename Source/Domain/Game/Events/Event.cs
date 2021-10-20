using Auxilia.Extensions;

namespace CatanBoardGame
{
	public class Event
	{
		public Event(EventType type, Player player, string message)
		{
			Type = type.ThrowIfNotDefined(nameof(type));
			Player = player.ThrowIfNull(nameof(player));
			Message = message.ThrowIfNullOrWhiteSpace(nameof(message));
		}

		public EventType Type { get; }
		public Player Player { get; }
		public string Message { get; }

		public override string ToString()
		{
			return $"{Player}: {Message}";
		}
	}
}
