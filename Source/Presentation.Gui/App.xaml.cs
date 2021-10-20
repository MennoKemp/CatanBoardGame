using System.Windows;

namespace CatanBoardGame.Presentation.Gui
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			new MainWindow().ShowDialog();
		}
	}
}
