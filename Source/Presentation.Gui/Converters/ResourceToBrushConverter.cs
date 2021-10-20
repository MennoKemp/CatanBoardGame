using Auxilia.Presentation.Converters;
using System;
using System.Globalization;
using System.Windows.Media;

namespace CatanBoardGame.Presentation.Gui.Converters
{
	public class ResourceToBrushConverter : ConverterBase
	{
		public Brush Desert { get; set; }
		public Brush Forest { get; set; }
		public Brush Hills { get; set; }
		public Brush Pastures { get; set; }
		public Brush Fields { get; set; }
		public Brush Mountains { get; set; }
		public Brush Sea { get; set; }

		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value switch
			{
				ResourceType resourceType => resourceType switch
				{
					ResourceType.Generic => Brushes.Black,
					ResourceType.Lumber => Forest,
					ResourceType.Brick => Hills,
					ResourceType.Wool => Pastures,
					ResourceType.Grain => Fields,
					ResourceType.Ore => Mountains,
					_ => Brushes.Transparent
				},
				TileType tileType => tileType switch
				{
					TileType.Desert => Desert,
					TileType.Forest => Forest,
					TileType.Hills => Hills,
					TileType.Pastures => Pastures,
					TileType.Fields => Fields,
					TileType.Mountains => Mountains,
					TileType.Sea => Sea,
					_ => Brushes.Transparent
				},
				_ => Brushes.Transparent
			};
		}
	}
}
