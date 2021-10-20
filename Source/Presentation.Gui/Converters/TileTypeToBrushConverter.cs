using Auxilia.Presentation.Converters;
using System;
using System.Globalization;
using System.Windows.Media;

namespace CatanBoardGame.Presentation.Gui.Converters
{
	public class TileTypeToBrushConverter : ConverterBase
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is TileType resourceType
				? resourceType switch
				{
					TileType.Desert => Brushes.Orange,
					TileType.Forest => Brushes.DarkGreen,
					TileType.Hills => Brushes.Red,
					TileType.Pastures => Brushes.LightGreen,
					TileType.Fields => Brushes.Yellow,
					TileType.Mountains => Brushes.Gray,
					_ => Brushes.Transparent
				}
				: Brushes.Transparent;
		}
	}
}
