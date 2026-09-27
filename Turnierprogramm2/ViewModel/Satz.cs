using System;
using System.Windows;
using Turnierprogramm2.Models;

namespace Turnierprogramm2.ViewModel
{
    [Serializable]
    public class Satz : BaseViewModel
    {
        public Satz()
        {
            TurnierEinstellungen turniereinstellungen = (TurnierEinstellungen)Application.Current.Resources["Turniereinstellungen"];
            this.maxpkt = turniereinstellungen.MaxPunkte;
        }
        readonly int maxpkt;
        int? _Punkte1 = null;
        int? _Punkte2 = null;

        public int? Punkte1
        {
            get => _Punkte1;
            set
            {
                if (value != _Punkte1)
                {
                    _Punkte1 = value;
                    calcResult(); NotifyPropertyChanged(nameof(Punkte1));
                }
            }
        }
        public int? Punkte2
        {
            get => _Punkte2;
            set
            {
                if (value != _Punkte2)
                {
                    _Punkte2 = value;
                    calcResult(); NotifyPropertyChanged(nameof(Punkte2));
                }



            }
        }

        public int[] result
        {
            get
            {
                int[] res = new int[2];
                if (ValidInput)
                {
                    res[0] = Punkte1 > Punkte2 ? 1 : 0;
                    res[1] = Punkte2 > Punkte1 ? 1 : 0;
                }
                return res;
            }
        }
        void calcResult()
        {
            int[] res = new int[2];
            if (ValidInput)
            {
                res[0] = Punkte1 > Punkte2 ? 1 : 0;
                res[1] = Punkte2 > Punkte1 ? 1 : 0;
            }
            this.Result = res;
        }
        private int[] _Res = new int[2];
        public int[] Result
        {
            get => _Res;
            set
            {
                _Res = value;
                NotifyPropertyChanged(nameof(Result));
                resultString = $"{Result[0]} : {Result[1]}";
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

        public bool ValidInput
        {
            get
            {
                if (Punkte1 == null && Punkte2 == null) return false;

                    if (Punkte1 == maxpkt && Punkte2 < maxpkt || Punkte2 == maxpkt && Punkte1 < maxpkt)
                    {
                        return true;
                    }
                   else return false;
                
            }
        }
        public string ResultText()
        {
            return $"{Punkte1} : {Punkte2}";
        }
    }
}
