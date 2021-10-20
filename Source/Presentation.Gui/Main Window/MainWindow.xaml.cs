using CatanBoardGame.Presentation.Gui.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace CatanBoardGame.Presentation.Gui
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = new MainViewModel();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			((MainViewModel)DataContext).Dispose();
		}
	}
}
