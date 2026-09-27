using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Turnierprogramm2.Models;
using Turnierprogramm2.ViewModel;

namespace Turnierprogramm2.Views
{
    /// <summary>
    /// Interaktionslogik für InputView.xaml
    /// </summary>
    public partial class InputView : Window
    {
        public InputView(InputViewModel model)



        {
            InitializeComponent();

            LayoutRoot.DataContext = model;
            Binding binding = new Binding("Doppels");
            binding.Source = model;
            dgMeldung.SetBinding(DataGrid.ItemsSourceProperty, binding);

            if (model.closeInputWndwAction == null) { model.closeInputWndwAction = new Action(this.Close); }

        }



        public static readonly DependencyProperty DoppelsProperty =
     DependencyProperty.Register(nameof(Doppels), typeof(ObservableCollection<Doppel>), typeof(InputView), new
        PropertyMetadata(new ObservableCollection<Doppel>()));

        public ObservableCollection<Doppel> Doppels
        {
            get { return (ObservableCollection<Doppel>)GetValue(DoppelsProperty); }
            set { SetValue(DoppelsProperty, value); }
        }
    }
}
