using System.ComponentModel;
using System.Windows;
using Turnierprogramm2.ViewModel;

namespace Turnierprogramm2.Views
{
    /// <summary>
    /// Interaktionslogik für GameView.xaml
    /// </summary>
    public partial class GameView : Window
    {
        internal readonly GameViewModel gameViewModel;
        public GameView(INotifyPropertyChanged gameViewModel)
        {

            InitializeComponent(); this.gameViewModel = gameViewModel as GameViewModel;
            LayoutRoot.DataContext = gameViewModel;
            //LVSets.ItemsSource = this.gameViewModel.Sets;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
