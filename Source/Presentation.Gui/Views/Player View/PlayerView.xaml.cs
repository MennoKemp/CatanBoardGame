using System.Windows;
using System.Windows.Controls;

namespace CatanBoardGame.Presentation.Gui.Views
{
	/// <summary>
	/// Interaction logic for PlayerView.xaml
	/// </summary>
	public partial class PlayerView : UserControl
	{
		public PlayerView()
		{
			InitializeComponent();
		}

		public static readonly DependencyProperty PlayerProperty = DependencyProperty.Register(
			nameof(Player),
			typeof(Player),
			typeof(PlayerView));
		public Player Player
		{
			get => (Player)GetValue(PlayerProperty);
			set => SetValue(PlayerProperty, value);
		}
	}
}
