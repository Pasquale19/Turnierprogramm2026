using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows;
using Turnierprogramm2.UserControls;

namespace Turnierprogramm2.Models
{
    [Serializable]
    public class Spieler : IEquatable<Spieler>, INotifyPropertyChanged
    {
        #region NotifyPropertyChanged
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
        #region Variables
        protected int _siege;
        private string name = "";
        private int level;
        protected int punkte;
        private int anzFreilose;
        private int id;
        private List<Spieler> partner = new List<Spieler>();
        private int _Gegenpunkte = 0;
        #endregion
        #region Property
        public List<Spieler> Partner => partner;
        protected virtual string Typename => "Sp";
        public string Name { get { return name; } set { name = value; NotifyPropertyChanged(nameof(Name)); } }
        public int Level { get { return level; } set { level = value; NotifyPropertyChanged(nameof(Level)); } }
        public int Siege { get { return _siege; } set { _siege = value; NotifyPropertyChanged(nameof(Siege)); } }
        public int Sets { get; set; } = 0;
        public int Punkte { get { return punkte; } }
        public int ID { get { return id; } set { id = value; NotifyPropertyChanged(nameof(ID)); } }
        public int Freilose { get { return anzFreilose; } }
        public int Gegenpunkte { get { return _Gegenpunkte; } set { _Gegenpunkte = value; NotifyPropertyChanged(nameof(Gegenpunkte)); } }
        #endregion
        #region IEQuatable
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Spieler objAsSpieler = obj as Spieler;
            if (objAsSpieler == null) return false;
            else return Equals(objAsSpieler);
        }
        public override int GetHashCode()
        {
            return ID;
        }
        public bool Equals(Spieler other)
        {
            if (other == null) return false;
            return (this.ID.Equals(other.ID));
        }
        #endregion
        #region Konstruktor
        public Spieler()
        {
            this.partner = new List<Spieler>();
        }
        public Spieler(int ID, string name, int level) : this(ID, name, level, 0, 0) { }
        public Spieler(int ID, string name) : this(ID, name, 0, 0, 0) { }
        public Spieler(int ID, string name, int level, int siege, int punkte, int Freilose = 0)
        {
            this.ID = ID;
            this.name = name;
            this.level = level;
            this._siege = siege;
            this.punkte = punkte;
            this.anzFreilose = Freilose;
            this.partner = new List<Spieler>();
        }

        #endregion

        public static Spieler Freilos => new Freilos(999, "Freilos");
        public static Spieler fromDataRow(DataRow row, Boolean TableHasHeader = true)
        {
            if (TableHasHeader)
            {
                int ID = row.Field<int>("ID");
                string name = row.Field<string>("Name");
                int level = row.Field<int>("Level");
                int siege = row.Field<int>("Siege");
                int anzFreilose = row.Field<int>("Freilose");
                int punkte = row.Field<int>("Punkte");
                Spieler spieler = new Spieler(ID, name, level, siege, punkte, anzFreilose);
                for (int i = 6; i < row.ItemArray.Length; i += 2)
                {
                    int PartnerID = Convert.ToInt32(row[i]);
                    string PartnerName = row[i + 1].ToString();
                    spieler.AddPartner(PartnerID, PartnerName);
                }
                return spieler;
            }
            else
            { return fromDataRowWithoutHeader(row); }

        }
        public static Spieler fromDataRowWithoutHeader(DataRow row)
        {
            int ID = Convert.ToInt32(row[0]);
            string name = row[1].ToString();
            int level = Convert.ToInt32(row[2]);
            int siege = Convert.ToInt32(row[3]);
            int anzFreilose = Convert.ToInt32(row[4]);
            int punkte = Convert.ToInt32(row[5]);
            Spieler spieler = new Spieler(ID, name, level, siege, punkte, anzFreilose);
            for (int i = 6; i < row.ItemArray.Length; i += 2)
            {
                int PartnerID = Convert.ToInt32(row[i]);
                string PartnerName = row[i + 1].ToString();
                spieler.AddPartner(PartnerID, PartnerName);
            }
            return spieler;
        }

        /// <summary>
        /// Dictionairy das (Namen,ID) enthält
        /// </summary>
        public static Dictionary<string, int> ToDict(Spieler[] player)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (Spieler spieler in player)
            {
                if (dict.ContainsKey(spieler.Name))
                {
                    MessageBox.Show($"{spieler.Name} ist bereits eingetragen Spieler.cs");
                }
                else
                {
                    dict.Add(spieler.Name, spieler.ID);
                }

            }
            return dict;
        }
        public override string ToString()
        {
            return $"{this.Typename}: {this.Name}";
            //return $"{this.GetType()} {this.Name}";
        }
        public string ToString(string tab = "\t")
        {
            string line = $"{id}{tab}{name}{tab}{level}{tab}{_siege}{tab}{punkte}{tab}{anzFreilose}";
            for (int i = 0; i < partner.Count; i++)
            {
                line += $"{tab}{partner[i].ID}{tab}{partner[i].name}";
            }
            return line;

        }

        public void AddPartner(Spieler Partner)
        {
            partner.Add(Partner);
            NotifyPropertyChanged(nameof(Partner));
        }
        public void AddPartner(int ID, string name)
       => new Spieler(ID, name);

        public List<Spieler> GetPartner
        { get { return partner; } }
        /// <summary>
        /// addiert ein Freilos und fügt das Freilos als Partner ein
        /// </summary>
        public void AddFreilos()
        {
            anzFreilose++;
            AddPartner(Freilos);
            NotifyPropertyChanged(nameof(Partner));
        }
        public void AddSieg() { _siege++; NotifyPropertyChanged(nameof(Siege)); }
        public void AddPunkte(int punkte) { this.punkte += punkte; NotifyPropertyChanged(nameof(Punkte)); }
        public void AddGegenpunkte(int punkte) { this.Gegenpunkte += punkte; NotifyPropertyChanged(nameof(Gegenpunkte)); }
        [NonSerialized]
        const int FreilosID = 999;

        public virtual Spieler DeepCopy()
        {
            return new Spieler(ID, Name, Level);
        }

    }


    [Serializable]
    public class kind : Spieler
    {

        public kind() { }
        public kind(int ID, string name, int level) : base(ID, name, level)
        {
        }

        //public static kind CastToChild(Spieler spieler)
        //{
        //    kind child = new kind();
        //    child.ID = spieler.ID;
        //    ch
        //}
        protected override string Typename => "Ki";

        public override Spieler DeepCopy()
        {
            return new kind(ID, Name, Level);
        }
    }
    [Serializable]
    public class adult : Spieler
    {

        public adult() { }
        public adult(int ID, string name) : this(ID, name, 0) { }
        public adult(int ID, string name, int level) : base(ID, name, level)
        {
        }
        public static new adult Freilos => new adult(Spieler.Freilos.ID, Spieler.Freilos.Name);
        protected override string Typename => "E";

        public override Spieler DeepCopy()
        {
            return new adult(ID, Name, Level);
        }
    }

    [Serializable]
    /// <summary>
    /// Dummy der immer 0 Punkte,Siege und Sätze hat
    /// </summary>
    public class Freilos : Spieler
    {
        public Freilos(int ID, string name) : base(ID, name)
        {

        }
        public new int Siege
        {
            get => 0;
            set => _siege = 0;
        }

        public new int Sets
        {
            get => 0;
            set => _siege = 0;
        }

        public new int Punkte
        {
            get => 0;
            set => punkte = 0;
        }
    }
}
