using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Turnierprogramm2.Views
{
    /// <summary>
    /// Interaktionslogik für DisplayWindow.xaml
    /// </summary>
    public partial class DisplayWindow : Window
    {
        public DisplayWindow()
        {
            InitializeComponent();
        }

        public DisplayWindow(string text,string title = "")
        {
            InitializeComponent();
            this.txtBox.Text = text;
            this.Title = title;
        }

        public DisplayWindow(string[,] text, string title = "")
        {
            InitializeComponent();
            for (int i=0;i<text.GetLength(0);i++)
            {
                RowDefinition row = new RowDefinition();
                this.grid.RowDefinitions.Add(row);
                for (int j=0;j<2;j++)
                {
                    TextBlock txtBlock1 = new TextBlock();

                    txtBlock1.Text = text[i,j];

                    txtBlock1.FontSize = 14;                 

                    txtBlock1.VerticalAlignment = VerticalAlignment.Center;

                    Grid.SetRow(txtBlock1, i);

                    Grid.SetColumn(txtBlock1, j);
                }
            }
          
        }


        private void Button_Click_close(object sender, RoutedEventArgs e)
        {
 this.Close();
        }
    }
}
