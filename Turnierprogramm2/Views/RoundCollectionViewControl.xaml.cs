using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Turnierprogramm2.ViewModel;

namespace Turnierprogramm2.Views
{
    /// <summary>
    /// Interaktionslogik für RoundCollectionViewControl.xaml
    /// </summary>
    public partial class RoundCollectionViewControl : UserControl
    {
        public RoundCollectionViewControl()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        //  public static readonly DependencyProperty RoundsProperty =
        //DependencyProperty.Register(nameof(Rounds), typeof(ObservableCollection<GameViewModel>), typeof(RoundCollectionViewControl), new
        //   PropertyMetadata(new ObservableCollection<GameViewModel>()));

        //  public ObservableCollection<GameViewModel> Rounds
        //  {
        //      get { return (ObservableCollection<GameViewModel>)GetValue(RoundsProperty); }
        //      set { SetValue(RoundsProperty, value); }
        //  }

        public static readonly DependencyProperty RoundCollectionVMProperty =
      DependencyProperty.Register(nameof(RoundVMCollection), typeof(ObservableCollection<RoundViewModel>), typeof(RoundCollectionViewControl), new
         PropertyMetadata(new ObservableCollection<RoundViewModel>()));

        public ObservableCollection<RoundViewModel> RoundVMCollection

        {
            get { return (ObservableCollection<RoundViewModel>)GetValue(RoundCollectionVMProperty); }
            set { SetValue(RoundCollectionVMProperty, value); }
        }
    }
}
