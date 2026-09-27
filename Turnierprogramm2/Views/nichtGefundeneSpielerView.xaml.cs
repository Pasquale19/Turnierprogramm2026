using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Turnierprogramm2.Models;

namespace Turnierprogramm2.Views
{
    /// <summary>
    /// Interaktionslogik für nichtGefundeneSpielerView.xaml
    /// </summary>
    public partial class nichtGefundeneSpielerView : Window
    {
        public nichtGefundeneSpielerView(IEnumerable<Spieler> Kinder, IEnumerable<Spieler> Adults)
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
            this.Kinder = new ObservableCollection<Spieler>(Kinder);
            this.Adults = new ObservableCollection<Spieler>(Adults);
        }

        public ObservableCollection<Spieler> Kinder
        {
            get { return (ObservableCollection<Spieler>)GetValue(KinderProperty); }
            set { SetValue(KinderProperty, value); }
        }

        public static readonly DependencyProperty KinderProperty =
            DependencyProperty.Register(nameof(Kinder), typeof(ObservableCollection<Spieler>), typeof(nichtGefundeneSpielerView),
                                        new PropertyMetadata(new ObservableCollection<Spieler>()));
        public ObservableCollection<Spieler> Adults
        {
            get { return (ObservableCollection<Spieler>)GetValue(AdultsProperty); }
            set { SetValue(AdultsProperty, value); }
        }

        public static readonly DependencyProperty AdultsProperty =
            DependencyProperty.Register(nameof(Adults), typeof(ObservableCollection<Spieler>), typeof(nichtGefundeneSpielerView),
                                        new PropertyMetadata(new ObservableCollection<Spieler>()));
    }
}
