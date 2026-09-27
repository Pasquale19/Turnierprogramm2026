using System;

namespace Turnierprogramm2.Models
{
    [Serializable]
    /// <summary>
    /// SingletonKlasse
    /// </summary>
    public class TurnierEinstellungen
    {
        public Boolean FreilosVorhanden;
        private int anzRunden;
        private int maxPunkte;
        private int _GewinnSätze;

        private TurnierEinstellungen()
        {
        }
        private static TurnierEinstellungen turnierProp;

        public static TurnierEinstellungen Instance()
        {
            if (turnierProp == null)
            {
                turnierProp = new TurnierEinstellungen();
            }
            return turnierProp;
        }
        public int GewinnSätze => _GewinnSätze;
        public int MaxPunkte { get { return turnierProp.maxPunkte; } }
        public int AnzRunden { get { return turnierProp.anzRunden; } }

        public int anzSets => GewinnSätze * 2 - 1;
        /// <summary>
        /// gibt true zurück wenn ein Freilos vorhanden ist
        /// </summary>
        public bool Freilos { get { return turnierProp.FreilosVorhanden; } }

        public void SetProperties(int anzRunden, int maxPunkte, bool FreilosVorhanden, int Sets)
        {
            setFreilos(FreilosVorhanden);
            setMaxPunkte(maxPunkte);
            setRundenAnzahl(anzRunden);
            _GewinnSätze = Sets;

        }
        public void setRundenAnzahl(int anzRunden)
        {
            turnierProp.anzRunden = anzRunden;
        }
        public void setFreilos(bool FreilosVorhanden)
        {
            turnierProp.FreilosVorhanden = FreilosVorhanden;
        }
        public void setMaxPunkte(int maxPunkte) { turnierProp.maxPunkte = maxPunkte; }
    }
}
