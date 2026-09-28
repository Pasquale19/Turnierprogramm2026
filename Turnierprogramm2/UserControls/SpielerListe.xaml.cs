using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Turnierprogramm2.Models;
using Turnierprogramm2.Utilities;

namespace Turnierprogramm2.UserControls
{
    /// <summary>
    /// Interaktionslogik für SpielerListe.xaml
    /// </summary>
    public partial class SpielerListe : UserControl
    {
        public SpielerListe()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;

            //Player.Add(new Spieler(2, "fabrice", 3, 4, 21));
            //Player.Add(new Spieler(3, "uwe", 3, 4, 32));
            //Player[0].AddPartner(new Spieler(29, "partner"));
            //dgUsers.ItemsSource = Player;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (Spieler S in Player) { MessageBox.Show($"{S.Name}"); }
        }

        public static readonly DependencyProperty SortedProperty =
     DependencyProperty.Register(nameof(Sorted), typeof(Boolean),
     typeof(SpielerListe), new FrameworkPropertyMetadata(false));

        // .NET Property wrapper
        public Boolean Sorted
        {
            get { return (Boolean)GetValue(SortedProperty); }
            set { SetValue(SortedProperty, value); }
        }

        public ObservableCollection<Spieler> Player
        {
            get { return (ObservableCollection<Spieler>)GetValue(PlayerProperty); }
            set { SetValue(PlayerProperty, value); }
        }

        public static readonly DependencyProperty PlayerProperty =
            DependencyProperty.Register(nameof(Player), typeof(ObservableCollection<Spieler>), typeof(SpielerListe),
                                        new PropertyMetadata(new ObservableCollection<Spieler>(), OnChanged));

        static void OnChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as SpielerListe).OnChanged();
        }

        void OnChanged()
        {
            if (Player != null)
                Player.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ExtraColumns_CollectionChanged);
        }

        void ExtraColumns_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //Console.WriteLine("Changed");
        }
        public static readonly DependencyProperty FileNameProperty =
  DependencyProperty.Register(nameof(FileName), typeof(string),
  typeof(SpielerListe), new FrameworkPropertyMetadata("Spielerliste"));

        // .NET Property wrapper
        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = "";
            dlg.FileName = "Rangliste";
            dlg.Filter = "pdf Files|*.pdf";
           
            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog() != true) return;
            string file = dlg.FileName;
            toPDF.DataGrid2(dgUsers, file, file, false);
        }

        private void Button_Click_ExportPdf(object sender, RoutedEventArgs e)
        {
            // This respects the current sorting and filtering of the DataGrid.
            List<Spieler> spielerListe = dgUsers.Items
                .OfType<Spieler>()
                .ToList();
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = "";
            dlg.FileName = "Rangliste";
            dlg.Filter = "pdf Files|*.pdf";

            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog() != true) return;
            string file = dlg.FileName;
            toPDF.ExportSpielerToPdf(spielerListe, dlg.FileName);
          

            }

        private void Button_Click_Sort(object sender, RoutedEventArgs e)
        {
            dgUsers.Items.SortDescriptions.Clear();

            // Primary sort: Siege ascending
            dgUsers.Items.SortDescriptions.Add(
                new SortDescription(
                    nameof(Spieler.Siege),
                    ListSortDirection.Descending));

            // Secondary sort: Gegenpunkte descending
            dgUsers.Items.SortDescriptions.Add(
                new SortDescription(
                    nameof(Spieler.Gegenpunkte),
                    ListSortDirection.Ascending));
        }
    }
    }

