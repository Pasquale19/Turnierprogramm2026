using System.IO;
using System.Text;
using System.Windows;
using Turnierprogramm2.ViewModel;
using Turnierprogramm2.Views;

namespace Turnierprogramm2
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
          //  MessageBox.Show("Turnierprogramm gestartet");
            prepareFolder();

            InputView view = new InputView(new InputViewModel());

            view.ShowDialog();

        }
        /// <summary>
        /// create the Meldungen and Ergebnissefolder if they dont exist
        /// </summary>
        void prepareFolder()
        {
            if (!Directory.Exists("Meldungen"))
            {
                Directory.CreateDirectory("Meldungen");
            }
            if (!Directory.Exists("Ergebnisse"))
            {
                Directory.CreateDirectory("Ergebnisse");
            }
            if (!File.Exists(@"Meldungen/meldungen.txt"))
            {
                using (StreamWriter sw = new StreamWriter(@"Meldungen/meldungen.txt"   ))
                {
                    StringBuilder SB = new StringBuilder("Kind1");
                    SB.Append($"\t{3}");
                    SB.Append($"\t{"Erwachsener1"}");
                    SB.Append($"\t{4}");
                    
                    sw.WriteLine(SB.ToString());
                }
            }

        }
    }


}
