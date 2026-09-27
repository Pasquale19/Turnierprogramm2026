using System;

namespace Turnierprogramm2.Models
{
    public class Turnier
    {
        private Boolean freilosVorhanden;
        private int anzSpieler_Flos;
        private TurnierEinstellungen turnierData;
        private Spieler[] kind, adult;

        private Turnier()
        {
        }
        private static Turnier turnier;

        public static Turnier Instance()
        {
            if (turnier == null)
            {
                turnier = new Turnier();
            }
            return turnier;
        }


        public void SetTurnier(TurnierEinstellungen tunierData, Spieler[] kind, Spieler[] adult)
        {
            turnier.kind = kind; turnier.adult = adult;
            turnier.turnierData = tunierData;

            anzSpieler_Flos = turnierData.Freilos ? kind.Length + 1 : kind.Length;
        }
        /// <summary>
        /// gibt die ANzahl der Spieler mit dem Freilos an
        /// </summary>
        public int anzSpielerFLos { get { return anzSpieler_Flos; } }
        public Spieler[] Kinder { get { return kind; } }
        public Spieler[] Adult { get { return adult; } }
        public Spieler[] SetKinder { set { value = kind; } }
        public int maxPkt { get { return turnierData.MaxPunkte; } }


        public Boolean FreilosVorhanden { get { return turnierData.Freilos; } }
        public Boolean SetFreilos { set { turnierData.FreilosVorhanden = value; } }
        public int anzRunden { get { return turnierData.AnzRunden; } }

        public int anzSätze(int Gewinnsätze)
        {
            return Gewinnsätze * 2 - 1;
        }


    }
}
