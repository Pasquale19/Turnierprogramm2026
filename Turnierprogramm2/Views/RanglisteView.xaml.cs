using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Turnierprogramm2.Models;

namespace Turnierprogramm2.Views
{
    /// <summary>
    /// Interaktionslogik für RanglisteView.xaml
    /// </summary>
    public partial class RanglisteView : Window
    {
        public RanglisteView()
        {
            InitializeComponent();
        }
        public RanglisteView(ObservableCollection<Spieler> kinder, ObservableCollection<Spieler> adults)
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
            IEnumerable<Spieler> sortedKinder = kinder.OrderByDescending(x => x.Siege).ThenByDescending(x => x.Sets).ThenByDescending(x => x.Punkte).ThenBy(x => x.Gegenpunkte);
            IEnumerable<Spieler> sortedadults = adults.OrderByDescending(x => x.Siege).ThenByDescending(x => x.Sets).ThenByDescending(x => x.Punkte).ThenBy(x => x.Gegenpunkte);
            Player[0] = new ObservableCollection<Spieler>(sortedKinder);
            Player[1] = new ObservableCollection<Spieler>(sortedadults);
        }
        public static readonly DependencyProperty PlayerProperty =
    DependencyProperty.Register(nameof(Player), typeof(ObservableCollection<Spieler>[]), typeof(InputView), new
       PropertyMetadata(new ObservableCollection<Spieler>[2]));

        public ObservableCollection<Spieler>[] Player
        {
            get { return (ObservableCollection<Spieler>[])GetValue(PlayerProperty); }
            set { SetValue(PlayerProperty, value); }
        }
    }
}
