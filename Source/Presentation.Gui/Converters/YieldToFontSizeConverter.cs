using System;
using System.Globalization;
using Auxilia.Presentation.Converters;

namespace CatanBoardGame.Presentation.Gui.Converters
{
	public class YieldToFontSizeConverter : ConverterBase
	{
		public double FontSize { get; set; }
		public double YieldFactor { get; set; }

		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is int yield
				? (1 + (yield - 3) * YieldFactor / 2) * FontSize
				: FontSize;
		}
	}
}
