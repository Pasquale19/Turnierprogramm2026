using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Turnierprogramm2.Models;

namespace Turnierprogramm2.Views
{
    /// <summary>
    /// Interaktionslogik für nichtGefundeneSpielerView.xaml
    /// </summary>
    public partial class nichtGefundeneSpielerListe : Window
    {
        public nichtGefundeneSpielerListe(IEnumerable<string> Kinder, IEnumerable<string> Adults)
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
            this.Player[0] = new ObservableCollection<string>(Kinder);
            this.Player[1] = new ObservableCollection<string>(Adults);
        }
                  
        public ObservableCollection<string>[] Player
        {
            get { return (ObservableCollection<string>[])GetValue(PlayerProperty); }
            set { SetValue(PlayerProperty, value); }
        }

        public static readonly DependencyProperty PlayerProperty =
            DependencyProperty.Register(nameof(Player), typeof(ObservableCollection<string>[]), typeof(nichtGefundeneSpielerListe),
                                        new PropertyMetadata(new ObservableCollection<string>[2]));
    }
}
