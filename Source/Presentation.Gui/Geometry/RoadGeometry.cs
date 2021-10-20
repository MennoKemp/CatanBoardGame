using Ludumia.Grids.Hexagonal;
using System;
using System.Windows;
using System.Windows.Media;

namespace CatanBoardGame.Presentation.Gui.Geometry
{
	public class RoadGeometry : Freezable
	{
		private const double Pi = Math.PI;

		public RoadGeometry()
		{
			UpdateGeometry();
		}

		public static readonly DependencyProperty TileSizeProperty = DependencyProperty.Register(
			nameof(TileSize),
			typeof(double),
			typeof(RoadGeometry),
			new PropertyMetadata(OnPropertyChanged));
		public double TileSize
		{
			get => (double)GetValue(TileSizeProperty);
			set => SetValue(TileSizeProperty, value);
		}

		public static readonly DependencyProperty RoadWidthProperty = DependencyProperty.Register(
			nameof(RoadWidth),
			typeof(double),
			typeof(RoadGeometry),
			new PropertyMetadata(OnPropertyChanged));
		public double RoadWidth
		{
			get => (double)GetValue(RoadWidthProperty);
			set => SetValue(RoadWidthProperty, value);
		}

		public static readonly DependencyProperty GeometryProperty = DependencyProperty.Register(
			nameof(Geometry),
			typeof(System.Windows.Media.Geometry),
			typeof(RoadGeometry));
		public System.Windows.Media.Geometry Geometry
		{
			get => (System.Windows.Media.Geometry)GetValue(GeometryProperty);
			set => SetValue(GeometryProperty, value);
		}
		
		protected override Freezable CreateInstanceCore()
		{
			return this;
		}

		private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as RoadGeometry)?.UpdateGeometry();
		}

		private void UpdateGeometry()
		{
			double width = RoadWidth;
			double length = TileSize / 2 - width / (2 * Math.Cos(Pi / 6));

			Geometry = new PathGeometry(new[]
				{
					new PathFigure(new Point(-width / 2, -length / 2), new[]
					{
						new LineSegment(new Point(width / 2, -length / 2), true),
						new LineSegment(new Point(width / 2, length / 2), true),
						new LineSegment(new Point(-width / 2, length / 2), true)
					}, true)
				},
				FillRule.Nonzero,
				new TranslateTransform(HexagonGeometry.Corners[0].X * TileSize, 0));
		}
	}
}
