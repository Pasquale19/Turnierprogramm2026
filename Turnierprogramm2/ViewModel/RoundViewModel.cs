using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Turnierprogramm2.Models;
using Turnierprogramm2.Views;
//using Turnierprogramm2.Utilities;

namespace Turnierprogramm2.ViewModel
{
    [Serializable]
    public class RoundViewModel : BaseViewModel, IPageViewModel
    {
        #region intern Variable
        readonly List<Spieler> Kinder;
        readonly List<Spieler> Adults;
        readonly int anzSpiele;
        int maxPkt;
        #endregion

        public string Name => $"Runde {Runde}";
        //readonly public int Runde = 0;
        #region Konstruktor

        public RoundViewModel(Action finishRoundAction)
        {
            this.nextRound = finishRoundAction;
        }
        public RoundViewModel(List<Doppel> doppels, int runde, Action finishRoundAction) : this(finishRoundAction)
        {
            // MessageBox.Show($"new Roundviewmodel:parameter runde{runde} Resource runde {(int)Application.Current.Resources["Runde"]}");
            this.Kinder = ((IEnumerable<Spieler>)Application.Current.Resources["Kinder"]).ToList();
            this.Adults = ((IEnumerable<Spieler>)Application.Current.Resources["Adults"]).ToList();
            if (!checkDuplicates(this.Kinder)) { MessageBox.Show("in den KIndern ist ein Fehler"); }
            if (!checkDuplicates(this.Adults)) { MessageBox.Show("in den Eltern ist ein Fehler"); }
            this._runde = runde;
            if (doppels.Count % 2 != 0) { doppels.Add(Doppel.Freilos); }
            anzSpiele = doppels.Count / 2;
            for (int i = 0; i < anzSpiele * 2; i += 2)
            {
                Games.Add(new GameViewModel(doppels[i], doppels[i + 1]));
            }
        }

        private bool checkDuplicates(List<Spieler> lst)
        {
            var query = lst.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();
            if (query.Count == 0) return true;
            else
            {
                return false;
            }
        }
        #endregion
        #region Properties
        public Turnierprogramm2.Utilities.FullyObservableCollection<GameViewModel> _Games = new Turnierprogramm2.Utilities.FullyObservableCollection<GameViewModel>();
        public Turnierprogramm2.Utilities.FullyObservableCollection<GameViewModel> Games
        {
            get => _Games;
            set
            {
                _Games = value; NotifyPropertyChanged(nameof(Games));
            }
        }
        //public ObservableCollection<GameViewModel> _Games = new ObservableCollection<GameViewModel>();
        //public ObservableCollection<GameViewModel> Games
        //{
        //    get => _Games;
        //    set
        //    {
        //        _Games = value; NotifyPropertyChanged(nameof(Games));
        //    }
        //}
        bool _isFinished = false;
        public bool isFinished
        {
            get => _isFinished;
            set
            {
                _isFinished = value; NotifyPropertyChanged(nameof(isFinished));
            }
        }

        readonly int _runde=0;
        public int Runde
        {
            get => _runde;
         
        }
        [NonSerialized]
        private Action _nextRound;
        public Action nextRound
        {
            get => _nextRound;
            set => _nextRound = value;
        }
        [NonSerialized]
        private DataGridCellInfo _cellInfo;
        public DataGridCellInfo CellInfo
        {
            get { return _cellInfo; }
            set
            {
                _cellInfo = value;
                NotifyPropertyChanged("CellInfo");
                try
                {
                    //System.Windows.MessageBox.Show(string.Format("Column: {0}",
                    //            _cellInfo.Column.DisplayIndex != null ? _cellInfo.Column.DisplayIndex.ToString() : "Index out of range!"));
                }
                catch { }
            }
        }
        #endregion

        #region Commands


        ICommand _randomResultCmd;
        /// <summary>
        /// reads all Games in the Observable Collection for testing
        /// </summary>
        public ICommand randomResultCmd
        {
            get
            {
                if (_randomResultCmd == null)
                {
                    _randomResultCmd = new Turnierprogramm2.Utilities.RelayCommand(addRandomResults);
                }
                return _randomResultCmd;
            }
        }

        ICommand _checkGamesCmd;
        /// <summary>
        /// reads all Games in the Observable Collection for testing
        /// </summary>
        public ICommand checkGamesCmd
        {
            get
            {
                if (_checkGamesCmd == null)
                {
                    _checkGamesCmd = new Turnierprogramm2.Utilities.RelayCommand(checkGames);
                }
                return _checkGamesCmd;
            }
        }
        ICommand _readGamesCmd;
        /// <summary>
        /// reads all Games in the Observable Collection for testing
        /// </summary>
        public ICommand readGamesCmd
        {
            get
            {
                if (_readGamesCmd == null)
                {
                    _readGamesCmd = new Turnierprogramm2.Utilities.RelayCommand(readGames);
                }
                return _readGamesCmd;
            }
        }

        void readGames(object args)
        {
            StringBuilder sb = new StringBuilder();
            string[,] text = new string[Games.Count, 2];
            int i = 0;
            foreach (GameViewModel game in Games)
            {
                text[i, 0] = game.Doppel1.ToString();
                text[i, 1] = game.Doppel2.ToString();
                sb.AppendLine(game.ToDetailedString());
                i++;
            }
            DisplayWindow msg = new DisplayWindow(sb.ToString(), $"aktuell eingetragende Doppel {Runde}");
            //DisplayWindow msg = new DisplayWindow(text, "aktuell eingetragende Doppel");
            msg.ShowDialog();
// MessageBox.Show(sb.ToString(), $"aktuell eingetragene Doppel Badminton-Eltern Kind Turnier Runde {Runde}__________");
        }

        void checkGames(object args)
        {
            if (KontrolleAuslosung(Kinder.ToArray(), Adults.ToArray()))
            {
                MessageBox.Show($"Auslosung stimmt");
            }
            else
            {
                MessageBox.Show($"Auslosung fehlerhaft");
            }
        }
        #endregion

        public bool HasErrors => ALLGamesAreValid;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// prüft ob alle Spiele vollständig und korrekt eingetragen wurden
        /// </summary>
        public bool ALLGamesAreValid
        {
            get
            {
                int index = 0;
                if (!KontrolleAuslosung(Kinder.ToArray(), Adults.ToArray())) return false;
                foreach (GameViewModel game in Games)
                {
                    if (game.HasErrors)
                    {
                        MessageBox.Show($"Fehlerhaftes Ergebniss in Zeile {index}"); return false;
                    }
                    index++;
                }
                return true;
            }
        }


        void addRandomResults(object args)
        {
            TurnierEinstellungen turnierEinstellungen = (TurnierEinstellungen)Application.Current.Resources["Turniereinstellungen"];
            int anzSets = turnierEinstellungen.anzSets;
            int maxPkt = turnierEinstellungen.MaxPunkte;
            Random rnd = new Random();

            foreach (GameViewModel game in Games)
            {
                int[] res = game.Result;

                int winner = rnd.Next(0, 2);   // creates a number between 0 and 1
                int pktzahl = rnd.Next(0, maxPkt - 1);
                if (winner > 0)
                {
                    res[0] = pktzahl;
                    res[1] = maxPkt;
                }
                else
                {
                    res[1] = pktzahl;
                    res[0] = maxPkt;
                }
                for (int i = 0; i < anzSets; i++)
                {
                    Satz satz = game.Sets[i];
                    satz.Punkte1 = res[0];
                    satz.Punkte2 = res[1];
                }
            }
            MessageBox.Show("random Result added");
        }

        public void addNextRoundAction(Action finishRoundAction)
        {
            this.nextRound = finishRoundAction;
        }
        public bool KontrolleAuslosung()
        { return KontrolleAuslosung(Kinder.ToArray(), Adults.ToArray()); }

        public bool KontrolleAuslosung(Spieler[] kind, Spieler[] adult)
        {
            Dictionary<string, int>[] nichtgefundeneSpieler = new Dictionary<string, int>[2];
            nichtgefundeneSpieler[0] = Spieler.ToDict(kind);
            nichtgefundeneSpieler[1] = Spieler.ToDict(adult);

            if (nichtgefundeneSpieler[0].Count < Kinder.Count)
            {
                MessageBox.Show("Fehler bei der Kontrolle Kinder");
            }
            if (nichtgefundeneSpieler[1].Count < Adults.Count)
            {
                MessageBox.Show("Fehler bei der Kontrolle Eltern");
            }

            int zeilenIndex = 0;    //zählt in welcher Zeile der tblRunde sich die Schleife befindet
            bool check = true;

            void RemoveKeyIfExists(Dictionary<string, int> dict, string key)
            {
                if (dict.ContainsKey(key)) { dict.Remove(key); }
                else
                {
                    MessageBox.Show($"Fehlerhafte Eingabe in Zeile {zeilenIndex} -> {key}");
                    check = false;
                }
            }

            foreach (GameViewModel game in Games)
            {
                string kind1 = game.Doppel1.Kind.Name;
                string kind2 = game.Doppel2.Kind.Name;
                RemoveKeyIfExists(nichtgefundeneSpieler[0], kind1);
                RemoveKeyIfExists(nichtgefundeneSpieler[0], kind2);
                string adult1 = game.Doppel1.Adult.Name;
                string adult2 = game.Doppel2.Adult.Name;
                RemoveKeyIfExists(nichtgefundeneSpieler[1], adult1);
                RemoveKeyIfExists(nichtgefundeneSpieler[1], adult2);

                zeilenIndex++;
            }
            if (nichtgefundeneSpieler[0].Count>0)
            {
                //MessageBox.Show($"überprüfen sie die Kinder\n {nichtgefundeneSpieler[0].ToString()}");
                check = false;
            }
            else if (nichtgefundeneSpieler[1].Count > 0)
            {
                //MessageBox.Show($"überprüfen sie die Eltern\n {nichtgefundeneSpieler[1].ToString()}");
                check = false;
            }
            if (check == false)
            {
                
                List<string> Liste = nichtgefundeneSpieler[0].Keys.ToList();
                List<string> Liste2 = nichtgefundeneSpieler[1].Keys.ToList();

                string nichtgefundeneKinderTxt = "";
                foreach (string s in Liste)
                {
                    nichtgefundeneKinderTxt += s + "\t";
                }
                string nichtgefundeneElternTxt = "";
                foreach (string s in Liste2)
                {
                    nichtgefundeneElternTxt += s + "\t";
                }
                string msg = "Bitte überprüfen sie die eingetragenen Spielernamen\n"
                    + $"fehlende KInder: {nichtgefundeneKinderTxt}\n"
                    + $"fehlende Eltern: {nichtgefundeneElternTxt}\n";
                MessageBox.Show(msg);

                //nichtGefundeneSpielerListe wndw = new nichtGefundeneSpielerListe(Liste, Liste2);
                //wndw.Show();
                return false;
            }
            else
            {
                //MessageBox.Show("Auslosung ist in Ordnung");
                return true;
            }

        }

        public override string ToString()
        {
            return $"Runde {Runde}";
        }
    }

    [Serializable]
    public class RoundCollectionViewModel : BaseViewModel, IPageViewModel
    {
        ObservableCollection<RoundViewModel> _rounds = new ObservableCollection<RoundViewModel>();
        public ObservableCollection<RoundViewModel> Runden
        {
            get => _rounds;
            set
            {
                _rounds = value;
                NotifyPropertyChanged(nameof(Runden));
            }
        }

        public string Name => "Runden";

    }
}
