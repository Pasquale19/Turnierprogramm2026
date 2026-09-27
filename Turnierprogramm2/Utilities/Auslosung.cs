using PathFindingAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using Turnierprogramm2.Models;

namespace Turnierprogramm2
{
    public class Auslosung
    {
        bool FreilosVorhanden = false;
        /// <summary>
        /// Berechnet die Wichtungsmatrix; bereits gebildete Paarg erhalten null; war ein Freilos bereits da 99Pkt; ansonsten wird die Siegdifferenz *3+1 genommen.
        /// </summary>
        protected static int?[,] WichtungsMatrix(Spieler[] kinder, Spieler[] adult, Boolean ExportMatrixAsTxt = false)
        {

            bool FreilosVorhanden = kinder.Last().Name == Spieler.Freilos.Name;
            /* speichert welche Paarungen bereits bestand und wer bereits ein Freilos hatte; die letzte Zeile steht für die Freilose; ein Freilos ist die ID 99*/
            int zeilen = kinder.Length;
            int spalten = adult.Length;

            //prüfen ob ein Freilos vorhanden ist => gegebenfalls wird die Anzahl der Zeilen und/oder Spalten erhöht
            //if (zeilen % 2 != 0 || spalten % 2 != 0)
            //{
            //    FreilosVorhanden = true;
            //    if (zeilen % 2 != 0 && spalten % 2 != 0)
            //    {
            //        zeilen++;
            //        spalten++;
            //    }
            //    else
            //    {
            //        if (zeilen % 2 != 0)
            //        {
            //            zeilen++;
            //        }
            //        else { spalten++; }
            //    }
            //}
            int?[,] A = ArrayWithZeros(zeilen, spalten);
            if (FreilosVorhanden) { A[A.GetLength(0) - 1, A.GetLength(1) - 1] = null; }  //legt fest das Freilos zu Freilos kommt

            /*es werden die Spieler Kinder von oben nach unten durchlaufen*/
            int PtFreilosGehabt = 99;
            foreach (Spieler kind in kinder)
            {
                // bereits vorhandene Paarungen erhalten 100 Punkte und werden in die Matrix keineLösung geschrieben
                foreach (Spieler partner in kind.GetPartner)
                {
                    if (partner.Name == "Freilos")    //bei einem Freilos wird in die letzte Spalte 99 gespeichert
                    {
                        int letzteSpalte = A.GetLength(1) - 1;
                        A[kind.ID, letzteSpalte] = PtFreilosGehabt;
                    }
                    else                            //bei bereits gebildetetn Paarungen wird null  ergänzt
                    {
                        A[kind.ID, partner.ID] = null;
                    }

                }
                int siege = kind.Siege;

                for (int j = 0; j < adult.GetLength(0); j++)
                {
                    if (A[kind.ID, adult[j].ID] == null || FreilosVorhanden && j == (A.GetLength(1) - 1)) { }//Doppel gab es schon => Eintrag wird ignoriert
                    else
                    {
                        if (adult[j].Freilose > 0) { A[A.GetLength(0) - 1, adult[j].ID] = PtFreilosGehabt; }
                        int diff = Math.Abs(siege - adult[j].Siege);    //Siegesdifferenz
                        A[kind.ID, adult[j].ID] = diff * 3 + 1;
                        if (FreilosVorhanden) { A[kind.ID, adult[j].ID]--; }
                    }
                }
            }
            return A;
        }
        private static int?[,] ArrayWithZeros(int zeilen, int spalten)
        {
            int?[,] arr = new int?[zeilen, spalten];
            for (int i = 0; i < zeilen; i++)
            {
                for (int j = 0; j < spalten; j++)
                {
                    arr[i, j] = 0;
                }
            }
            return arr;
        }

        protected static List<Doppel> BerechneDoppel(int?[,] graph, Spieler[] Kinder, Spieler[] Adult)
        {
            bool FreilosVorhanden = Kinder.Last().Name == Spieler.Freilos.Name;
            bipGraphSolver Solver = new bipGraphSolver(graph, true);
            int[,] sol = Solver.bestMatching();
            List<Doppel> doppellist = new List<Doppel>();
            if (FreilosVorhanden)
            {
                int[,] Paar = extractFreilos(sol, Kinder.Length - 1);
            }
            doppellist.AddRange((Doppel.buildDoppel(Kinder, Adult, sol)));
            return doppellist;
        }

        protected static int[,] extractFreilos(int[,] Paarg, out int[,] FreilosPaarg)
        {
            int[,] FreilosPaar = new int[,] { { 99, 99 } };

            int[,] Paar = new int[Paarg.GetLength(0) - 2, 2];
            int counter = 0;
            for (int i = 0; i < Paarg.GetLength(0); i++)
            {
                if (Paarg[i, 1] == Paarg.GetLength(0) - 1 || Paarg[i, 0] == Paarg.GetLength(0) - 1)
                {
                    if (Paarg[i, 1] == Paarg.GetLength(0) - 1)
                    {
                        FreilosPaar[0, 0] = Paarg[i, 0];
                    }
                    if (Paarg[i, 0] == Paarg.GetLength(0) - 1)
                    {
                        FreilosPaar[0, 1] = Paarg[i, 1];
                    }
                }
                else
                {
                    Paar[counter, 0] = Paarg[i, 0];
                    Paar[counter, 1] = Paarg[i, 1];
                    counter++;
                }
            }
            FreilosPaarg = FreilosPaar;
            return Paar;
        }

        public static List<Doppel> BerechneDoppel(Spieler[] kinder, Spieler[] adult)
        {
            int?[,] W = WichtungsMatrix(kinder, adult);

            return BerechneDoppel(W, kinder, adult);
        }
        public static List<Doppel> BerechneDoppel(List<Spieler> kinder, List<Spieler> adults)
        {
            return BerechneDoppel(kinder.ToArray(), adults.ToArray());
        }
         
        /// <summary>
        /// Kind und Erwachsener der ein Freilos erhalten hat wird zusammen geordnet; das Freilos wird ans Ende der Liste geschrieben
        /// </summary>
        /// <param name="Paarg"></param>
        /// <param name="FreilosPaarg"></param>
        /// <param name="FreilosID"></param>
        /// <returns></returns>
        protected static int[,] extractFreilos(int[,] Paarg, int FreilosID = 99)
        {
            int[,] FreilosPaar = new int[,] { { FreilosID, FreilosID } };

            int[,] Paar = new int[Paarg.GetLength(0), 2];
            int[] indexFreilos = new int[2];
            int[] indexSpieler = new int[2];


            for (int i = 0; i < Paarg.GetLength(0); i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (Paarg[i, j] == FreilosID) indexFreilos[j] = i;
                    int p = j == 0 ? 1 : 0;
                    if (Paarg[i, p] == FreilosID) indexSpieler[j] = i;
                }
            }


            //swap

            for (int i = 1; i < 2; i++)
            {
                int temp = Paarg[indexFreilos[i], i];
                Paarg[indexFreilos[i], i] = Paarg[indexSpieler[i], i];
                Paarg[indexSpieler[i], i] = temp;
            }

            return Paarg;
        }
    }
}
