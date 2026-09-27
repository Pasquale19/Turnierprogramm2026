using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Turnierprogramm2.Models;

namespace Turnierprogramm2.ViewModel
{
    [Serializable]
    public class GameViewModel : BaseViewModel
    {
        #region Variable
        readonly int Gewinnsätze;

        readonly int anzSets; readonly int maxPkt;
        #endregion
        #region Konstruktor
        public GameViewModel(Doppel doppel1, Doppel doppel2)
        {
            this.Doppel1 = doppel1;
            this.Doppel2 = doppel2;
            TurnierEinstellungen turnierEinstellungen = ((TurnierEinstellungen)Application.Current.Resources["Turniereinstellungen"]);
            Gewinnsätze = turnierEinstellungen.GewinnSätze;
            maxPkt = turnierEinstellungen.MaxPunkte;
            anzSets = turnierEinstellungen.anzSets;

            for (int i = 0; i < anzSets; i++)
            {
                Sets.Add(new Satz());
            }
        }
        #endregion
        #region Properties
        Doppel _doppel1 = new Doppel();
        public Doppel Doppel1
        {
            get => _doppel1;
            set
            {
                _doppel1 = value;

                NotifyPropertyChanged(nameof(Doppel1));
            }
        }
        Doppel _doppel2 = new Doppel();
        public Doppel Doppel2
        {
            get => _doppel2;
            set
            {
                _doppel2 = value;
                NotifyPropertyChanged(nameof(Doppel2));
            }
        }


        ObservableCollection<Satz> _Sets = new ObservableCollection<Satz>();
        public ObservableCollection<Satz> Sets
        {
            get => _Sets;
            set
            {
                _Sets = value;
                NotifyPropertyChanged(nameof(Sets));
            }
        }
        string _resultString = "0 : 0";
        /// <summary>
        /// for dsiplaying in the dataGridTextColumn cause Multibinding doesnt update
        /// </summary>
        public string resultString
        {
            get => _resultString;
            set { _resultString = value; NotifyPropertyChanged(nameof(resultString)); }
        }

        #endregion


        [NonSerialized]
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();
        public bool HasErrors => !ValidGame;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            throw new NotImplementedException();
        }
        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        void calcResult()
        {
            int[] res = new int[2];

            foreach (Satz set in Sets)
            {
                if (set.ValidInput)
                {
                    res[0] += set.Result[0];
                    res[1] += set.Result[1];
                }
            }

            Result = res;
        }
        private int[] _Result = new int[2];
        /// <summary>
        /// Property that returns the Result as [gewonnene Sätze Paarung 1,gewonnene Sätze Paarung 2]
        /// </summary>
        public int[] Result
        {
            get
            {
                int[] res = new int[2];

                foreach (Satz set in Sets)
                {
                    if (set.ValidInput)
                    {
                        res[0] += set.Result[0];
                        res[1] += set.Result[1];
                    }
                }
                resultString = $"{res[0]} : {res[1]}"; return res;

            }
            set
            {
                _Result = value;
            }
        }


        bool ValidGame
        {
            get
            {
                int[] res = Result;
                if (res[0]!=Gewinnsätze || res[1]!=Gewinnsätze)
                {

                }
                if (res[0] == Gewinnsätze && res[1] < Gewinnsätze || res[1] == Gewinnsätze && res[0] < Gewinnsätze) return true;
                else return false;
            }
        }

        public int[] Punkte
        {
            get
            {
                int[] pkt = new int[2];
                foreach (Satz set in Sets)
                {
                    if (set.Punkte1 == null || set.Punkte2 == null)
                    {
                        throw new Exception("Überprüfung der Sätze fehlgeschlagen");
                    }
                    pkt[0] += (int)set.Punkte1;
                    pkt[1] += (int)set.Punkte2;
                }
                return pkt;
            }
        }

        public override string ToString()
        {
            string text="";
            for (int i=0;i<this.Sets.Count;i++)
            {
                text += $"{i+1}:) {this.Sets[i].ResultText()}";
                text += "\n";
            }
          return  text;
        }

        public string ToDetailedString()
        {
            string text = $"Doppel1: {Doppel1.Kind.Name} (LV:{Doppel1.Kind.Level} Siege: {Doppel1.Kind.Siege}) u. {Doppel1.nameE} (Lv{Doppel1.Adult.Level} Siege: {Doppel1.Adult.Siege}) \t vs. \t";
            Spieler kind2 = Doppel2.Kind;
            Spieler adult2 = Doppel2.Adult;
            text += $"Doppel2: {kind2.Name} (LV:{kind2.Level} Siege: {kind2.Siege}) u. {adult2.Name} (Lv{adult2.Level} Siege: {adult2.Siege})";
            return text;
        }

        public int[] SiegPunkte
        {
            get
            {
                int[] pkt = new int[2];
                if (Result[0] > Result[1])
                {
                    pkt[0] = 1;
                }
                else
                {
                    if (Result[0] < Result[1])
                    {
                        pkt[1] = 1;
                    }
                    else
                    {
                        throw new Exception("Spiel ist noch nicht abgeschlossen");
                    }
                }
                return pkt;
            }
        }

    }
}
