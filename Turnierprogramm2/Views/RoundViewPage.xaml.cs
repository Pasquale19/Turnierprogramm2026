using System.Windows.Controls;
using Turnierprogramm2.ViewModel;
namespace Turnierprogramm2.Views
{
    /// <summary>
    /// Interaktionslogik für RoundViewPage.xaml
    /// </summary>
    public partial class RoundViewPage : Page
    {
        public RoundViewPage() { InitializeComponent(); }
        public RoundViewPage(RoundViewModel VM)
        {
            InitializeComponent();
            this.DataContext = VM;
            dg.ItemsSource = VM.Games;
        }
    }
}
