using System;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;

namespace Turnierprogramm2.Models
{
    [Serializable]
    /// <summary>
    /// master
    /// </summary>
    public class Doppel : IEquatable<Doppel>, INotifyPropertyChanged
    {
        #region NotifyPropertyChanged
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Doppel objAsDoppel = obj as Doppel;
            if (objAsDoppel == null) return false;
            else return Equals(objAsDoppel);
        }

        public bool Equals(Doppel other)
        {
            if (other == null) return false;
            bool equal = false;
            if (this.IDK == other.IDK && this.IDE == other.IDE) { equal = true; }
            return (equal);
        }

        public Doppel()
        { }
        public Doppel(Spieler kind, Spieler adult)
        {
            //this.kind = kind;
            //this._adult = adult;
            this.kind = kind.DeepCopy();
            this._adult = adult.DeepCopy();
            SetSiege(kind, adult);
            this.sumLevel = calcLevelSumme(kind, adult);
        }


        private Spieler kind = new Spieler(); private Spieler _adult = new adult();
        private int siege;
        private int sumLevel;
        public static Doppel Freilos => new Doppel(Spieler.Freilos, adult.Freilos);
        #region public Properties
        public string nameK { get { return kind.Name; } }
        public int SumLevel { get { return sumLevel; } }
        public string nameE { get { return _adult.Name; } }
        public int LevelKind { get { return kind.ID; } set { kind.ID = value; } }
        public int LevelAdult { get { return _adult.Level; } set { _adult.Level = value; } }

        public int IDK { get { return kind.ID; } }
        public int IDE { get { return _adult.ID; } }
        public int Siege
        {
            get
            {
                if (kind.Siege > Adult.Siege) return kind.Siege;
                else return Adult.Siege;
            }
        }  //es entscheidet die Anzahl der SIege von dem der mehr gewonnen hat
        #endregion
        private int calcLevelSumme(Spieler kind, Spieler adult)
        {
            return kind.Level + adult.Level;
        }

        private void SetSiege(Spieler kind, Spieler adult)
        {
            if (kind.Siege > adult.Siege)
            {
                siege = kind.Siege;
            }
            else
            {
                siege = adult.Siege;
            }
        }

        public Spieler Kind
        {
            get { return kind; }
            set
            {
                kind = value; this.sumLevel = calcLevelSumme(kind, _adult);
                NotifyPropertyChanged(nameof(Kind));
            }
        }
        public Spieler Adult
        {
            get { return _adult; }
            set
            {
                _adult = value; this.sumLevel = calcLevelSumme(kind, _adult);
                NotifyPropertyChanged(nameof(Adult));
            }
        }

        public bool isFreilos
        {
            get
            {
                bool isFreilos = false;
                if (Kind.Name=="Freilos" ^ Adult.Name=="Freilos")
                {
                    return true;
                }

                    return isFreilos;
            }
        }

        /// <summary>
        /// erstellt aus dem Array der IDs die Doppel und sortiert die Doppel zusätzlich nach Spielstaerke
        /// </summary>
        public static Doppel[] buildDoppel(Spieler[] kind, Spieler[] adult, int[,] Paarung)
        {
            Doppel[] doppel = new Doppel[Paarung.GetLength(0)];
            for (int i = 0; i < doppel.GetLength(0); i++)
            {
                doppel[i] = new Doppel(kind[Paarung[i, 0]], adult[Paarung[i, 1]]);
            }
            return DoppelExt.SortDoppel(doppel);
        }
    }

    public static class DoppelExt

    {
        public static Doppel buildDoppel(DataRow rowK, DataRow rowA)
        {
            Spieler kind = Spieler.fromDataRow(rowK);
            Spieler adult = Spieler.fromDataRow(rowA);
            return new Doppel(kind, (adult)adult);
        }
        public static Doppel buildDoppel(Spieler kind, Spieler adult)
        {
            Doppel doppel = new Doppel(kind, (adult)adult);
            return doppel;
        }



        /// <summary>
        /// Sortiert die Doppel nach der Anzahl der Siege danach nach der Summe der Level beider Partner
        /// </summary>
        /// <param name="doppel"></param>
        /// <returns></returns>
        public static Doppel[] SortDoppel(Doppel[] doppel)
        {

            //int end = doppel.Length;
            int end;
            end = 0;
            foreach (Doppel Paar in doppel)
            {
                if (Paar != null) { end++; }
            }
            Array.Resize(ref doppel, end);
            bool swapped = true;
            while (swapped == true)
            {
                swapped = false;
                for (int i = 0; i < end - 1; i++)
                {

                    if (doppel[i].Siege < doppel[i + 1].Siege)
                    {

                        //MessageBox.Show("Es wird die ID getauscht: " +doppel[doppel[i]].SumLevel.ToString() + " <> " + doppel[doppel[i + 1]].SumLevel.ToString(), "Vergleich der Levelsumme");
                        Doppel tempDoppel = new Doppel();
                        tempDoppel = doppel[i];
                        doppel[i] = doppel[i + 1];
                        doppel[i + 1] = tempDoppel;
                        swapped = true;

                    }
                    else
                    {
                        if (doppel[i].Siege == doppel[i + 1].Siege) //bei gleicher ANzahl der Siege entscheidet das Level des Doppels SumLevel
                        {
                            if (doppel[i].SumLevel < doppel[i + 1].SumLevel)
                            {

                                Doppel tempDoppel = new Doppel();
                                tempDoppel = doppel[i];
                                doppel[i] = doppel[i + 1];
                                doppel[i + 1] = tempDoppel;
                                swapped = true;
                            }
                        }
                    }
                }
                end--;
            }
            return doppel;
        }

        public static Doppel Freilos => new Doppel();
    }
}
