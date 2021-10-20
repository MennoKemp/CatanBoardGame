using Auxilia.Presentation.Extensions;
using CatanBoardGame.Presentation.Gui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CatanBoardGame.Presentation.Gui.Views
{
	/// <summary>
	/// Interaction logic for BoardView.xaml
	/// </summary>
	public partial class BoardView : UserControl
	{
		private readonly List<IGameObjectModel> _selectedGameObjects = new List<IGameObjectModel>();

		private Canvas _canvas;

		public BoardView()
		{
			InitializeComponent();
			BoardCanvas.MouseDown += OnBoardMouseDown;
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			_canvas = BoardCanvas.GetVisualChild<Canvas>();
		}
		
		public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(
			nameof(Scale),
			typeof(double),
			typeof(BoardView),
			new PropertyMetadata(1.0));
		public double Scale
		{
			get => (double)GetValue(ScaleProperty);
			set => SetValue(ScaleProperty, value);
		}

		public static readonly DependencyProperty SelectionSizeProperty = DependencyProperty.Register(
			nameof(SelectionSize),
			typeof(int),
			typeof(BoardView));
		public int SelectionSize
		{
			get => (int)GetValue(SelectionSizeProperty);
			set => SetValue(SelectionSizeProperty, value);
		}

		public static readonly DependencyProperty GameObjectsProperty = DependencyProperty.Register(
			nameof(GameObjects),
			typeof(IEnumerable<IGameObjectModel>),
			typeof(BoardView),
			new FrameworkPropertyMetadata(Enumerable.Empty<IGameObjectModel>()));
		public IEnumerable<IGameObjectModel> GameObjects
		{
			get => (IEnumerable<IGameObjectModel>)GetValue(GameObjectsProperty);
			set => SetValue(GameObjectsProperty, value);
		}

		public static readonly DependencyProperty SelectedGameObjectsProperty = DependencyProperty.Register(
			nameof(SelectedGameObjects),
			typeof(IEnumerable<IGameObjectModel>),
			typeof(BoardView),
			new FrameworkPropertyMetadata(Enumerable.Empty<IGameObjectModel>()));
		public IEnumerable<IGameObjectModel> SelectedGameObjects
		{
			get => (IEnumerable<IGameObjectModel>)GetValue(SelectedGameObjectsProperty);
			set => SetValue(SelectedGameObjectsProperty, value);
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			Scale = Math.Min(ActualWidth / 600, ActualHeight / 600);
		}
		
		private void OnBoardMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (_canvas.Children.Cast<ContentPresenter>().FirstOrDefault(c => c.IsMouseOver)?.Content is not IGameObjectModel { IsSelectable: true } gameObject) 
				return;

			if (_selectedGameObjects.Contains(gameObject))
			{
				_selectedGameObjects.Remove(gameObject);
				gameObject.IsSelected = false;

			}
			else
			{
				if (_selectedGameObjects.Count == SelectionSize)
				{
					_selectedGameObjects.First().IsSelected = false;
					_selectedGameObjects.RemoveAt(0);
				}

				_selectedGameObjects.Add(gameObject);
				gameObject.IsSelected = true;
			}

			SelectedGameObjects = _selectedGameObjects.ToList();
		}
	}
}
