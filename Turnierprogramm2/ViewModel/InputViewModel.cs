using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Turnierprogramm2.Models;
using Turnierprogramm2.Utilities;

namespace Turnierprogramm2.ViewModel
{
    public class InputViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        public InputViewModel()
        {
            startTurnier = new RelayCommand(new Action<object>(StartTurnier), canStartTurnier);
            exportMeldungenCmd = new RelayCommand(new Action<object>(ExportMeldungen));
            importMeldungenCmd = new RelayCommand(new Action<object>(ImportMeldungen));
            browseMeldungenCmd = new RelayCommand(new Action<object>(browseMeldungen));
           
            ImportMeldungen(new object());
        }

       

        #region Propertys
        private ICommand _startTurnier;
        public ICommand startTurnier
        {
            get { return _startTurnier; }
            set { _startTurnier = value; }
        }
        int _anzRunden = 4;
        public int anzRunden
        {
            get => _anzRunden;
            set
            {
                _anzRunden = value;
                if (_anzRunden <= 0)
                {
                    AddError(nameof(anzRunden), "die Rundenanzahl muss größer als 0 sein");
                }
                NotifyPropertyChanged(nameof(anzRunden));


            }
        }

        int _anzSets = 1;
        public int anzGewinnsätze
        {
            get => _anzSets;
            set
            {
                _anzSets = value;
                if (_anzSets <= 0)
                {
                    AddError(nameof(anzGewinnsätze), "die Satzanzahl muss größer als 0 sein");
                }
                NotifyPropertyChanged(nameof(anzGewinnsätze));
            }
        }

        int _maxPkt = 21;
        public int maxPkt
        {
            get => _maxPkt;
            set
            {
                _maxPkt = value;
                if (_maxPkt <= 0)
                {
                    AddError(nameof(maxPkt), "die max Punktzahl pro Satz muss größer als 0 sein");
                }
                NotifyPropertyChanged(nameof(maxPkt));
            }
        }

        public ObservableCollection<Doppel> _Doppels = new ObservableCollection<Doppel>();

        public ObservableCollection<Doppel> Doppels { get => _Doppels; set { _Doppels = value; NotifyPropertyChanged(nameof(Doppels)); } }
        #endregion

        #region methods
        const string MeldungenPath = @"Meldungen/meldungen.txt";
        protected void StartTurnier(object sender)
        {
            if (!CanCreate) return;
            //Application.Current.Resources["maxPkt"] = maxPkt;
            //Application.Current.Resources["anzGewinnsätze"] = anzGewinnsätze;
            //Application.Current.Resources["anzSets"] = anzGewinnsätze * 2 - 1;
            //Application.Current.Resources["anzRunden"] = anzRunden;
            Application.Current.Resources["Runde"] = 1;
            if (Doppels.Count % 2 != 0)
            {
                Adults.Add(Spieler.Freilos);
                Kinder.Add(Spieler.Freilos);
                Kinder.Last().ID = Doppels.Count;
                Adults.Last().ID = Doppels.Count;
                Doppels.Add(new Doppel(Kinder.Last(), Adults.Last()));
            }
            Application.Current.Resources.Add("Kinder", Kinder);
            Application.Current.Resources.Add("Adults", Adults);
            TurnierEinstellungen turnierEinstellungen = TurnierEinstellungen.Instance();
            turnierEinstellungen.SetProperties(anzRunden, maxPkt, !(Kinder.Count % 2 == 0), anzGewinnsätze);
            Application.Current.Resources.Add("Turniereinstellungen", turnierEinstellungen);
            List<Doppel> list = Doppels.OrderByDescending(x => x.SumLevel).ToList();
            MainViewModel viewModel = new MainViewModel(Kinder, Adults, list);
            MainWindow wndw = new MainWindow(viewModel);
            wndw.ShowDialog();
            closeInputWndw();
        }
        public void closeInputWndw() { closeInputWndwAction(); }

        public Action closeInputWndwAction;
public ICommand importMeldungenCmd { get; set; }
        public ICommand exportMeldungenCmd { get; set; }

        public ICommand browseMeldungenCmd { get; set; }
        public ICommand browseMeldungenExportCmd
        {
            get
            {
                if (_browseMeldungenExportCmd is null)
                {
                    _browseMeldungenExportCmd = new RelayCommand(new Action<object>(browseMeldungenExport));
                }
                return _browseMeldungenExportCmd;
            }
        }
        private ICommand _browseMeldungenExportCmd;

        public ICommand showErrorsCmd
        {
            get
            {
                if (_showErrorsCmd is null)
                {
                    _showErrorsCmd = new RelayCommand(new Action<object>(showErrors));
                }
                return _showErrorsCmd;
            }
        }
        private ICommand _showErrorsCmd;

        /// <summary>
        /// browse for a file to load Meldungen
        /// </summary>
        /// <param name="sender"></param>
        protected void browseMeldungen(object sender)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = "meldungen"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    loadMeldungen(dlg.FileName);
                }
                catch (Exception ex) { MessageBox.Show($"Fehler beim Laden der Meldungen {ex}"); }
            }
        }
        /// <summary>
        /// browse for a file to save Meldungen
        /// </summary>
        /// <param name="sender"></param>
        protected void browseMeldungenExport(object sender)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "meldungen"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    saveMeldungen(dlg.FileName);
                    MessageBox.Show("Speichern Erfolgreich");
                }
                catch (Exception ex) { MessageBox.Show($"Fehler beim Speichern der Meldungen {ex}"); }
            }
        }
        protected void ImportMeldungen(object sender) => loadMeldungen(MeldungenPath);
        public void ExportMeldungen(object sender) => saveMeldungen(MeldungenPath);
        
        /// <summary>
        /// loads the Meldungen from the 
        /// </summary>
        /// <param name="dateiPfad">Pfad zur Meldungen .txt Datei !!!=>.txt angeben</param>
        protected void loadMeldungen(string dateiPfad)
        {
            Doppels = new ObservableCollection<Doppel>();
            try
            {
                using (StreamReader SR = new StreamReader(dateiPfad))
                {
                    while (SR.Peek() != -1)
                    {
                        string line = SR.ReadLine();
                        string[] parts = line.Split('\t', ',', ';');
                        string NameKind = parts[0];
                        if (!Int32.TryParse(parts[1], out int LevelKind) || !Int32.TryParse(parts[3], out int LevelAdult)) { throw new Exception("invalid File;Level is incorrect"); }
                        string NameAdult = parts[2];
                        kind child = new kind(0, NameKind, LevelKind);
                        adult adult = new adult(0, NameAdult, LevelAdult);
                        Doppels.Add(new Doppel(child, adult));
                    }
                }
                MessageBox.Show("laden abgeschlossen");
            }
            catch (Exception ex) { MessageBox.Show($"Es konnten keine Meldungen geladen werden {ex}"); }
        }
        protected void saveMeldungen(string dateiPfad)
        {
             
            using (StreamWriter SR = new StreamWriter(dateiPfad))
            {
                foreach (Doppel doppel in Doppels)
                {
                    StringBuilder SB = new StringBuilder(doppel.Kind.Name);
                    SB.Append($"\t{doppel.Kind.Level}");
                    SB.Append($"\t{doppel.Adult.Name}");
                    SB.Append($"\t{doppel.Adult.Level}");
                    string line = SB.ToString();
                    SR.WriteLine(line);
                }
            }
        }
        

        List<Spieler> Kinder = new List<Spieler>();
        List<Spieler> Adults = new List<Spieler>();

        bool canStartTurnier(object sender) => CanCreate;
        public bool CanCreate
        {
            get
            {
                _errorsByPropertyName.Clear();
                List<string> childNames = new List<string>();
                List<string> adultNames = new List<string>();

                Kinder = new List<Spieler>();
                Adults = new List<Spieler>();
                int anz = Doppels.Count;
                for (int i = 0; i < anz; i++)
                {
                    Doppel doppel = Doppels[i];
                    Spieler child = doppel.Kind;
                    Spieler parent = doppel.Adult;
                    if (doppel.Kind.Name == "" || doppel.Adult.Name == "")
                    {
                        //AddError(nameof(CanCreate), $"in Zeile{i} wurde kein Name angegeben");
                        AddError($"Zeile{i} leer", $"in Zeile{i} wurde kein Name angegeben");
                        return false;
                    }
                    if (childNames.Contains(child.Name))
                    {
                        AddError($"Kind{child.Name}", $"der kindername {child.Name} existiert bereits");
                       //AddError(nameof(CanCreate), $"der Name {child.Name} existiert bereits");
                        return false;
                    }
                    else childNames.Add(child.Name);
                    if (adultNames.Contains(parent.Name))
                    {
                        AddError($"eltern{parent.Name}", $"der elternName {parent.Name} existiert bereits\"");
                        //AddError(nameof(CanCreate), $"der Name {parent.Name} existiert bereits");
                        return false;
                    }
                    else adultNames.Add(parent.Name);
                    kind k = new kind(i, child.Name, child.Level);
                    adult a = new adult(i, parent.Name, parent.Level);
                    Kinder.Add(k);
                    Adults.Add(a);
                    child.ID = i;
                    parent.ID = i;// necessary otherwise Id isnt in Doppel in the first round

                }

                return true;

            }
        }

        protected string errorList()
        {
            if (CanCreate)
            { return "keine Fehler"; }
            string errors = "";

            foreach(KeyValuePair<string,List<string>> entry in _errorsByPropertyName)
            {
                errors += entry.Key + "\n";
                List<string> errors2 = new List<string>();
                foreach (string error in entry.Value)
                {
                    errors += "\t" + error + "\n";
                }
            }
            return errors;
        }
        protected void showErrors(object sender)
        {
            MessageBox.Show($"{errorList()}");

        }
        #endregion

            #region INotifyDataError
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();
        public bool HasErrors => _errorsByPropertyName.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public ObservableCollection<string> Errors
        {
            get
            {
                ObservableCollection<string> errors = new ObservableCollection<string>();
                return errors;
                //foreach (string mes in )
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName) ?
                _errorsByPropertyName[propertyName] : null;
        }

        public void AddError(string propertyName, string errorMessage)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Add(propertyName, new List<string>());
            }
            _errorsByPropertyName[propertyName].Add(errorMessage); OnErrorsChanged(propertyName);
        }
        #endregion
    }
}
