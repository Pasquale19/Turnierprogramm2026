using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Turnierprogramm2.Models;
using Turnierprogramm2.Utilities;
using Turnierprogramm2.Views;

namespace Turnierprogramm2.ViewModel
{
    [Serializable]
    public class MainViewModel : BaseViewModel
    {

        int Runde => Runden.Runden.Count;
        int anzRunden => turnierEinstellungen.AnzRunden;
        TurnierEinstellungen turnierEinstellungen = TurnierEinstellungen.Instance();
        SpielerListeViewModel SpielerListeLinder;
        SpielerListeViewModel SpielerListeAdults;
        
        #region Konstruktor
        public MainViewModel()
        {
        }
        public MainViewModel(List<Spieler> kinder, List<Spieler> adult, List<Doppel> doppels)
        {
            turnierEinstellungen = (TurnierEinstellungen)Application.Current.Resources["Turniereinstellungen"];

            this.Kinder = new ObservableCollection<Spieler>(kinder);
            this.adults = new ObservableCollection<Spieler>(adult);
            Runden.Runden.Add(new RoundViewModel(doppels, Runde + 1, nextRound));

            // Add available pages
            SpielerListeLinder = new SpielerListeViewModel(new ObservableCollection<Spieler>(kinder));
            PageViewModels.Add(SpielerListeLinder);
            SpielerListeAdults = new SpielerListeViewModel(new ObservableCollection<Spieler>(adult));
            PageViewModels.Add(SpielerListeAdults);
            PageViewModels.Add(Runden);

            // Set starting page
            currentPageViewModel = PageViewModels[2];
        }
        #endregion

        #region Properties

        public ObservableCollection<Spieler> Kinder = new ObservableCollection<Spieler>();
        public ObservableCollection<Spieler> adults = new ObservableCollection<Spieler>();


        RoundCollectionViewModel Runden = new RoundCollectionViewModel();
        #endregion




        #region Fields
        [NonSerialized]
        private ICommand _changePageCommand;
        [NonSerialized]
        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;

        #endregion



        #region Properties / Commands
        ICommand _openRanglisteCmd;
        public ICommand openRanglisteCmd
        {
            get
            {
                if (_openRanglisteCmd == null)
                {
                    _openRanglisteCmd = new RelayCommand(openRangliste);
                }
                return _openRanglisteCmd;

            }
        }
        void openRangliste(object sender)
        {
            RanglisteView wndw = new RanglisteView(Kinder, adults);
            wndw.Show();
        }
        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand(
                        p => ChangeViewModel((IPageViewModel)p),
                        p => p is IPageViewModel);
                }
                return _changePageCommand;
            }
        }

        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }
        }

        public IPageViewModel currentPageViewModel
        {
            get => _currentPageViewModel;
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;

                    NotifyPropertyChanged(nameof(currentPageViewModel));
                }
            }
        }

        #endregion

        #region Methods

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            currentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }

        void nextRound()
        {
            RoundViewModel aktRunde = Runden.Runden[Runde - 1];
            if (!aktRunde.ALLGamesAreValid) return;
            addRoundResult(aktRunde.Games);
            Runden.Runden[Runde - 1].isFinished = true;
            if (Runde < anzRunden)
            {
                List<Doppel> list = Auslosung.BerechneDoppel(Kinder.ToArray(), adults.ToArray());
                Application.Current.Resources["Runde"] = Runde + 1;
                Runden.Runden.Add(new RoundViewModel(list, Runde + 1, nextRound));
            }
            else
            {
                MessageBox.Show("Turnier beendet");
                openRangliste(new object());
            }

        }

        void addRoundResult(IEnumerable<GameViewModel> games)
        {
            //changed on 26.05.22
            //foreach (GameViewModel model in games)
            //{

            //    int IDA1 = (adults.Where(x => x.Name == model.Doppel1.Adult.Name).ToList())[0].ID;
            //    int IDA2 = (adults.Where(x => x.Name == model.Doppel2.Adult.Name).ToList())[0].ID;
            //    int IDkind1 = (Kinder.Where(x => x.Name == model.Doppel1.Kind.Name).ToList())[0].ID;
            //    int IDkind2 = (Kinder.Where(x => x.Name == model.Doppel2.Kind.Name).ToList())[0].ID;

            //    Kinder[IDkind1].AddPartner(adults[IDA1]);
            //    Kinder[IDkind2].AddPartner(adults[IDA2]);
            //    adults[IDA1].AddPartner(Kinder[IDkind1]);
            //    adults[IDA2].AddPartner(Kinder[IDkind2]);

            //    int[] pkt = model.Punkte;
            //    Kinder[IDkind1].AddPunkte(pkt[0]);
            //    adults[IDA1].AddPunkte(pkt[0]);
            //    Kinder[IDkind2].AddPunkte(pkt[1]);
            //    adults[IDA2].AddPunkte(pkt[1]);


            //    int[] sets = model.Result;
            //    Kinder[IDkind1].Sets += (sets[0]);
            //    adults[IDA1].Sets += sets[0];
            //    Kinder[IDkind2].Sets += (sets[1]);
            //    adults[IDA2].Sets += sets[1];

            //    int[] siege = model.SiegPunkte;
            //    Kinder[IDkind1].Siege += (siege[0]);
            //    adults[IDA1].Siege += siege[0];
            //    Kinder[IDkind2].Siege += (siege[1]);
            //    adults[IDA2].Siege += siege[1];
            //}
            foreach (GameViewModel model in games)
            {
                
                Spieler Adult1 = (adults.Where(x => x.Name == model.Doppel1.Adult.Name).ToList())[0];
                Spieler Adult2= (adults.Where(x => x.Name == model.Doppel2.Adult.Name).ToList())[0];
                Spieler Kind1 = (Kinder.Where(x => x.Name == model.Doppel1.Kind.Name).ToList())[0];
                Spieler Kind2 = (Kinder.Where(x => x.Name == model.Doppel2.Kind.Name).ToList())[0];
                int IDA1 = Adult1.ID;
                int IDA2 = Adult2.ID;
                int IDkind1 = Kind1.ID;
                int IDkind2 = Kind2.ID;

                //if (adults[IDA1].Name != Adult1.Name) throw new Exception("Name des ersten Adults und Index im Array stimmen nicht überein");
                //if (adults[IDA2].Name != Adult2.Name) throw new Exception("Name des zweiten Adults und Index im Array stimmen nicht überein");
                //if (Kinder[IDkind1].Name != Kind1.Name) throw new Exception($"{Kinder[IDkind1].Name} != {Kind1.Name} kind1");
                //if (Kinder[IDkind2].Name != Kind2.Name) throw new Exception($"{Kinder[IDkind2].Name} != {Kind2.Name} kind2");
                if (adults[IDA1].Name != Adult1.Name) MessageBox.Show("Name des ersten Adults und Index im Array stimmen nicht überein");
                if (adults[IDA2].Name != Adult2.Name) MessageBox.Show("Name des zweiten Adults und Index im Array stimmen nicht überein");
                if (Kinder[IDkind1].Name != Kind1.Name) MessageBox.Show($"{Kinder[IDkind1].Name} != {Kind1.Name} kind1");
                if (Kinder[IDkind2].Name != Kind2.Name) MessageBox.Show($"{Kinder[IDkind2].Name} != {Kind2.Name} kind2");

                //   Kind1.AddPartner(Adult1);
                Kinder[IDkind1].AddPartner(adults[IDA1]);
                Kinder[IDkind2].AddPartner(adults[IDA2]);
                adults[IDA1].AddPartner(Kinder[IDkind1]);
                adults[IDA2].AddPartner(Kinder[IDkind2]);

                int[] pkt = model.Punkte;
                Kinder[IDkind1].AddPunkte(pkt[0]);
                adults[IDA1].AddPunkte(pkt[0]);
                Kinder[IDkind2].AddPunkte(pkt[1]);
                adults[IDA2].AddPunkte(pkt[1]);

                Kinder[IDkind1].AddGegenpunkte(pkt[1]);
                adults[IDA1].AddGegenpunkte(pkt[1]);
                Kinder[IDkind2].AddGegenpunkte(pkt[0]);
                adults[IDA2].AddGegenpunkte(pkt[0]);


                int[] sets = model.Result;
                Kinder[IDkind1].Sets += (sets[0]);
                adults[IDA1].Sets += sets[0];
                Kinder[IDkind2].Sets += (sets[1]);
                adults[IDA2].Sets += sets[1];

                int[] siege = model.SiegPunkte;
                Kinder[IDkind1].Siege += (siege[0]);
                adults[IDA1].Siege += siege[0];
                Kinder[IDkind2].Siege += (siege[1]);
                adults[IDA2].Siege += siege[1];
            }


            #endregion
        }

        #region import/export
        ICommand _saveTurnierCmd;
        public ICommand saveTurnierCmd
        {
            get
            {
                if (_saveTurnierCmd == null)
                {
                    _saveTurnierCmd = new RelayCommand(saveTurnier);
                }
                return _saveTurnierCmd;
            }
        }
        void saveTurnier(object sender)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = "tur";
            dlg.Filter = "Tunier Files|*.tur";
            if (dlg.ShowDialog() != true) return;
            string file = dlg.FileName;

            ArrayList listToBeSerialized = new ArrayList();
            using (var fs = new FileStream(file, FileMode.Create))
            {
                listToBeSerialized.Add(Kinder);
                listToBeSerialized.Add(adults);
                listToBeSerialized.Add(turnierEinstellungen);
                listToBeSerialized.Add(Runden.Runden);
                //foreach (RoundViewModel VM in Runden.Runden)
                //{
                //    listToBeSerialized.Add(VM);
                //    //listToBeSerialized.Add(Image.FromFile(VM.filePath));
                //}
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, listToBeSerialized);
            };

        }
        ICommand _loadTurnierCmd;
        public ICommand LoadTurnierCmd
        {
            get
            {
                if (_loadTurnierCmd == null)
                {
                    _loadTurnierCmd = new RelayCommand(loadTurnier);
                }
                return _loadTurnierCmd;
            }
        }
        void loadTurnier(object sender)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = "tur";
            dlg.Filter = "Tunier Files|*.tur";
            if (dlg.ShowDialog() != true) return;
            string file = dlg.FileName;
            Kinder.Clear();
            adults.Clear();
            Runden.Runden.Clear();

            // Deserialize.
            using (FileStream fs = new FileStream(
                file, FileMode.Open))
            {
                ArrayList deserializedList = (ArrayList)new BinaryFormatter().Deserialize(fs);
                int l = deserializedList.Count;
                Kinder.AddRange( (ObservableCollection<Spieler>)deserializedList[0]);
                adults.AddRange((ObservableCollection<Spieler>)deserializedList[1]);

                Application.Current.Resources["Kinder"] = Kinder;
                Application.Current.Resources["Adults"] = adults;
                SpielerListeLinder.Spielerliste = Kinder;
                SpielerListeAdults.Spielerliste = adults;

                turnierEinstellungen = (TurnierEinstellungen)deserializedList[2];
                Application.Current.Resources["TurnierEinstellungen"] = turnierEinstellungen;
                Runden.Runden = (ObservableCollection<RoundViewModel>)deserializedList[3];
                RoundViewModel lastRound = Runden.Runden.Last();
                lastRound.addNextRoundAction(nextRound); //notwendig für serializierung
                Application.Current.Resources["Runde"] =lastRound.Runde;
                for (int i = 2; i < l; i += 1)
                {
                    //Image img =(Image) deserializedList[i + 1];
                    //string Imgfile = @$"temp\Screenshot{i / 2}" + ".png";
                    //img.Save(Imgfile);
                    //RoundV
                    //ScreenShotVM VM = (ScreenShotVM)deserializedList[i];
                    //VM.filePath = Imgfile;


                }

                string info = $"Zwischenstand eines Turniers geladen. Turnier befindet sich in Runde: {Application.Current.Resources["Runde"]}";
                MessageBox.Show(info);
            }

        }
        #endregion
    }
    public static class Extension{
        public static void AddRange<T>(this ObservableCollection<T> list, IEnumerable<T> added)
        {
            foreach (T item in added)
            {
                list.Add(item);
            }
        }
    }
}
