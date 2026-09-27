using System;
using System.Collections.ObjectModel;
using Turnierprogramm2.Models;

namespace Turnierprogramm2.ViewModel
{
    [Serializable]
    public class SpielerListeViewModel : BaseViewModel, IPageViewModel
    {
        private ObservableCollection<Spieler> _Spielerliste = new ObservableCollection<Spieler>();

        public ObservableCollection<Spieler> Spielerliste
        {
            get => _Spielerliste;
            set
            {
                _Spielerliste = value;
                NotifyPropertyChanged(nameof(Spielerliste));
            }
        }
        public SpielerListeViewModel(ObservableCollection<Spieler> spieler)
        {
            Spielerliste = spieler;
        }

        public string Name
        {
            get
            {
                string name = "SpielerListe ";
                if (Spielerliste.Count > 0)
                {
                    if (Spielerliste[0] is adult) name += "Erwachsener";
                    else { name += "Kinder"; }
                }
                return name;
            }
        }
    }

    public class ErwSpielerListeViewModel : SpielerListeViewModel
    {

        public ErwSpielerListeViewModel(ObservableCollection<Spieler> spieler) : base(spieler)
        {

        }
    }
}