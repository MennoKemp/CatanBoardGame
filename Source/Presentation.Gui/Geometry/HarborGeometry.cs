using System;
using System.Windows;
using System.Windows.Media;

namespace CatanBoardGame.Presentation.Gui.Geometry
{
	public class HarborGeometry : DependencyObject
	{
		private const double Pi = Math.PI;

		public HarborGeometry()
		{
			UpdateGeometry();
		}

		public static readonly DependencyProperty TileSizeProperty = DependencyProperty.Register(
			nameof(TileSize),
			typeof(double),
			typeof(HarborGeometry),
			new PropertyMetadata(OnPropertyChanged));
		public double TileSize
		{
			get => (double)GetValue(TileSizeProperty);
			set => SetValue(TileSizeProperty, value);
		}

		public static readonly DependencyProperty GeometryProperty = DependencyProperty.Register(
			nameof(Geometry),
			typeof(System.Windows.Media.Geometry),
			typeof(HarborGeometry));
		public System.Windows.Media.Geometry Geometry
		{
			get => (System.Windows.Media.Geometry)GetValue(GeometryProperty);
			set => SetValue(GeometryProperty, value);
		}

		private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as HarborGeometry)?.UpdateGeometry();
		}

		private void UpdateGeometry()
		{
			Geometry = new PathGeometry(new[]
				{
					new PathFigure(new Point(0, -0.5), new[]
					{
						new LineSegment(new Point(-Math.Cos(Pi / 6) / 4, -0.625), true),
						new LineSegment(new Point(-Math.Cos(Pi / 6) / 2, -0.5), true),
						new LineSegment(new Point(-Math.Cos(Pi / 6) / 2, -0.25), true)
					}, true)
				},
				FillRule.Nonzero,
				new ScaleTransform(TileSize, TileSize));
		}
	}
}
