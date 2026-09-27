using System.Collections.ObjectModel;
using System.Windows.Controls;
using Turnierprogramm2.Models;
using Turnierprogramm2.UserControls;

namespace Turnierprogramm2.Views
{
    /// <summary>
    /// Interaktionslogik für SpielerListeViewPage.xaml
    /// </summary>
    public partial class SpielerListeViewPage : Page
    {
        public SpielerListeViewPage(ObservableCollection<Spieler> spieler)
        {
            InitializeComponent();
            SpielerListe.DataContext = spieler;
        }
    }
}
