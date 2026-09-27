using System.Windows;
using Turnierprogramm2.ViewModel;

namespace Turnierprogramm2
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(MainViewModel VM)
        {
            InitializeComponent();
            this.DataContext = VM;
        }
    }
}
