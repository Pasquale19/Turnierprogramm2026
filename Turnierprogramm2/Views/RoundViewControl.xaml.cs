using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Turnierprogramm2.Utilities;
using Turnierprogramm2.ViewModel;

namespace Turnierprogramm2.Views
{
    /// <summary>
    /// Interaktionslogik für RoundViewPage.xaml
    /// </summary>
    public partial class RoundViewControl : UserControl
    {

        public RoundViewControl()
        {
            //MessageBox.Show("Round View Control Created");
            InitializeComponent();
            LayoutRoot.DataContext = this;
            StyleColumn();
            runde = (int)Application.Current.Resources["Runde"];
            //MessageBox.Show($"Roundviewcontrol {runde.ToString()} created");
        }
        #region binadable Properties
        public static readonly DependencyProperty FinishRoundProperty =
       DependencyProperty.Register(nameof(FinishRound),
           typeof(ICommand), typeof(RoundViewControl), new UIPropertyMetadata(null));
        public ICommand FinishRound
        {
            get { return (ICommand)GetValue(FinishRoundProperty); }
            set { SetValue(FinishRoundProperty, value); }
        }

        public static readonly DependencyProperty NextRoundProperty =
DependencyProperty.Register(nameof(NextRound),
   typeof(Action), typeof(RoundViewControl), new UIPropertyMetadata(null));
        public Action NextRound
        {
            get { return (Action)GetValue(NextRoundProperty); }
            set { SetValue(NextRoundProperty, value); }
        }

        public static readonly DependencyProperty readGamesCmdProperty =
    DependencyProperty.Register(nameof(readGamesCmd),
        typeof(ICommand), typeof(RoundViewControl), new UIPropertyMetadata(null));
        public ICommand readGamesCmd
        {
            get { return (ICommand)GetValue(readGamesCmdProperty); }
            set { SetValue(readGamesCmdProperty, value); }
        }

        public static readonly DependencyProperty randomResultCmdProperty =
 DependencyProperty.Register(nameof(randomResultCmd),
     typeof(ICommand), typeof(RoundViewControl), new UIPropertyMetadata(null));
        public ICommand randomResultCmd
        {
            get { return (ICommand)GetValue(randomResultCmdProperty); }
            set { SetValue(randomResultCmdProperty, value); }
        }

        public static readonly DependencyProperty checkGamesCmdProperty =
 DependencyProperty.Register(nameof(checkGamesCmd),
     typeof(ICommand), typeof(RoundViewControl), new UIPropertyMetadata(null));
        public ICommand checkGamesCmd
        {
            get { return (ICommand)GetValue(checkGamesCmdProperty); }
            set { SetValue(checkGamesCmdProperty, value); }
        }

        public static readonly DependencyProperty developerModeActiveProperty =
 DependencyProperty.Register(nameof(developerModeActive),
     typeof(Boolean), typeof(RoundViewControl), new UIPropertyMetadata(false));
        public Boolean developerModeActive
        {
            get { return (Boolean)GetValue(developerModeActiveProperty); }
            set { SetValue(developerModeActiveProperty, value); }
        }

        public static readonly DependencyProperty RundeProperty =
 DependencyProperty.Register(nameof(Runde),
     typeof(int), typeof(RoundViewControl), new UIPropertyMetadata(1));
        public int Runde
        {
            get { return (int)GetValue(RundeProperty); }
            set { SetValue(RundeProperty, value); }
        }





        public FullyObservableCollection<GameViewModel> Games
        {
            get { return (FullyObservableCollection<GameViewModel>)GetValue(GamesProperty); }
            set { SetValue(GamesProperty, value); }
        }
        public static readonly DependencyProperty GamesProperty =
        DependencyProperty.Register(nameof(Games), typeof(FullyObservableCollection<GameViewModel>), typeof(RoundViewControl),
                                    new PropertyMetadata(new FullyObservableCollection<GameViewModel>()));
        #endregion

        #region open Gameviewmethods
        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space
            //DataGridRow row = ItemsControl.ContainerFromElement((DataGrid)sender,
            //                                    e.OriginalSource as DependencyObject) as DataGridRow;

            if (!(sender is DataGridRow)) return;
            DataGridRow row = sender as DataGridRow;
            if (row == null) return;

            if (selectedCellIndex == 4 || selectedCellIndex == 9)
            {

                int index = row.GetIndex();
                GameView gameview = new GameView(Games[index]);
                gameview.ShowDialog();
            }
        }
        int selectedCellIndex;
        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridCell dataGridCellTarget = (DataGridCell)sender;
            selectedCellIndex = dataGridCellTarget.Column.DisplayIndex;
        }
        #endregion
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(Games.Count.ToString());

            //btnFinish.Visibility = Visibility.Hidden;
            int resourcerunde = (int)Application.Current.Resources["Runde"];
            NextRound();


            //not working because runde ist falsch
            MessageBoxResult BoxResult = MessageBox.Show($"export der Rundenergebnisse Runde:{Runde}?", "Export?",
                 MessageBoxButton.YesNo);

            if (BoxResult == MessageBoxResult.Yes)
            {
                //this.Button_Click_Export(sender, e);
                try
                {
                    string file = $"Ergebnisse\\Runde{resourcerunde}.pdf";
                    toPDF.DataGrid2(dg, file, $"Runde{runde}", true);
                    MessageBox.Show($"{file} exportiert");
                }
                catch { MessageBox.Show("Fehler bei Export"); }
            }

        }
        readonly int runde;
        void StyleColumn()
        {
            foreach (DataGridTextColumn col in dg.Columns.OfType<DataGridTextColumn>())
            {
                string name = col.Header.ToString();
                int index = col.DisplayIndex;
                if (name[0] == 'I' && name[1] == 'D')
                {
                    col.Width = 50;
                    //col.Visibility = Visibility.Hidden;
                    col.IsReadOnly = true;
                }


            }
        }
        void switchMode()
        {
            if (developerModeActive)
            {
                foreach (DataGridTextColumn col in dg.Columns.OfType<DataGridTextColumn>())
                {
                    //col.Visibility = Visibility.Visible;
                }
            }
            else
            {
                StyleColumn();
            }
        }

        public static readonly DependencyProperty isFinishedProperty =
         DependencyProperty.Register("isFinished", typeof(bool), typeof(RoundViewControl), new
            PropertyMetadata(false));

        public bool isFinished
        {
            get { return (bool)GetValue(isFinishedProperty); }
            set { SetValue(isFinishedProperty, value); }
        }

        private void Button_Click_Export(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = ".pdf";
            dlg.Filter = "Pdf Files|*.pdf";
            dlg.FileName = $"AuslosungRunde{Runde}";
            if (dlg.ShowDialog() != true) return;
            string file = dlg.FileName;
            toPDF.DataGrid2(dg, $"{file}", $"Runde{Runde}", true);
            ////string Path = (string)Application.Current.Resources["ExportPath"];
            ////toPDF.DataGrid2(dg,$"{Path}\\Runde{runde}",$"Runde{runde}",true);
        }

        private void Button_Click_RoundNumber(object sender, RoutedEventArgs e)
        {
            int resourcerunde = (int)Application.Current.Resources["Runde"];
            MessageBox.Show($"Runde:{Runde} resourceRunde{resourcerunde}: Viewmodel {LayoutRoot.DataContext}");
        }

      
    }
}
