using CatanBoardGame.Presentation.Gui.Models;
using Ludumia.Grids.Hexagonal;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CatanBoardGame.Presentation.Gui.Converters
{
	public class GamePieceToTransformConverter : Freezable, IValueConverter
	{
		public static readonly DependencyProperty TileSizeProperty = DependencyProperty.Register(
			nameof(TileSize),
			typeof(double),
			typeof(GamePieceToTransformConverter),
			new FrameworkPropertyMetadata(100.0));
		public double TileSize
		{
			get => (double)GetValue(TileSizeProperty);
			set => SetValue(TileSizeProperty, value);
		}
		
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (value as IGameObjectModel)?.GameObject switch
			{
				Tile tile => TransformTile(tile, parameter?.Equals("Number") ?? false),
				Robber robber => TransformTile(robber.Tile),
				Path path => TransformPath(path),
				Road road => TransformPath(road.Path),
				Village village => TransformSettlement(village),
				City city => TransformSettlement(city),
				Harbor harbor => TransformHarbor(harbor),
				_ => null
			};
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		private Transform TransformTile(Tile tile, bool transformNumber = false)
		{
			TransformGroup transformGroup = new TransformGroup();

			if (transformNumber)
				transformGroup.Children.Add(new TranslateTransform(-TileSize / 2, -TileSize / 2));

			transformGroup.Children.Add(new TranslateTransform(tile.X * HexagonGeometry.HorizontalSpacing * TileSize, tile.Y * HexagonGeometry.VerticalSpacing * TileSize));

			return transformGroup;
		}

		private Transform TransformPath(Path path)
		{
			TransformGroup transformGroup = new TransformGroup();
			
			if (path.Orientation != PointyEdgeOrientation.Vertical)
			{
				transformGroup.Children.Add(new RotateTransform((int)path.Orientation * 60));

				if (path.Orientation == PointyEdgeOrientation.TopLeftToBottomRight)
					transformGroup.Children.Add(new TranslateTransform { X = -HexagonGeometry.HorizontalSpacing * TileSize });
			}

			transformGroup.Children.Add(new TranslateTransform(path.X1 * HexagonGeometry.HorizontalSpacing * TileSize, path.Y1 * HexagonGeometry.VerticalSpacing * TileSize));
			return transformGroup;
		}

		private Transform TransformSettlement(ISettlement settlement)
		{
			TransformGroup transformGroup = new TransformGroup();
			
			if (settlement.Intersection.IsHigh)
				transformGroup.Children.Add(new TranslateTransform { Y = -HexagonGeometry.Height / 4 * TileSize });

			transformGroup.Children.Add(new TranslateTransform(settlement.Intersection.X * HexagonGeometry.HorizontalSpacing * TileSize, settlement.Intersection.Y * HexagonGeometry.VerticalSpacing * TileSize));
			return transformGroup;
		}

		private Transform TransformHarbor(Harbor harbor)
		{
			TransformGroup transformGroup = new TransformGroup();
			transformGroup.Children.Add(new RotateTransform((int)harbor.Direction * 60));
			transformGroup.Children.Add(new TranslateTransform(harbor.Tile.X * HexagonGeometry.HorizontalSpacing * TileSize, harbor.Tile.Y * HexagonGeometry.VerticalSpacing * TileSize));
			return transformGroup;
		}

		protected override Freezable CreateInstanceCore()
		{
			return new GamePieceToTransformConverter();
		}
	}
}
