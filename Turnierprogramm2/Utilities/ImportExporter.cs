using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Turnierprogramm2.Models;
using Turnierprogramm2.ViewModel;

namespace Turnierprogramm2.Utilities
{
    public class ImportExporter
    {
        public static List<Spieler> ImportSpieler(string path, out int runden)
        {
            List<Spieler> list = new List<Spieler>();
            runden = 0;
            using (StreamReader SR = new StreamReader(path))
            {
                while (SR.Peek() != -1)
                {
                    string line = SR.ReadLine();
                    string[] parts = line.Split('\t', ';', ',');
                    Int32.TryParse(parts[0], out Int32 ID);
                    Int32.TryParse(parts[2], out Int32 Level);
                    Int32.TryParse(parts[3], out Int32 Siege);
                    Int32.TryParse(parts[4], out Int32 Sets);
                    Int32.TryParse(parts[5], out Int32 Punkte);
                    Spieler spieler = new Spieler(ID, parts[1], Level, Siege, Sets, Punkte);
                    int length = parts.Length;
                    runden = (length - 5) / 2;
                    for (int i = 6; i < length; i += 2)
                    {
                        Int32.TryParse(parts[i], out Int32 IDPartner);
                        spieler.AddPartner(new Spieler(IDPartner, parts[i + 1]));
                    }
                    list.Add(spieler);
                }
            }
            return list;
        }

        public static List<GameViewModel> ImportRunde(string path, out int sets)
        {
            List<GameViewModel> list = new List<GameViewModel>();
            sets = 1;
            using (StreamReader SR = new StreamReader(path))
            {
                while (SR.Peek() != -1)
                {
                    string line = SR.ReadLine();
                    string[] parts = line.Split('\t', ';', ',');
                    Int32.TryParse(parts[0], out Int32 IDK1);
                    Int32.TryParse(parts[2], out Int32 IDA1);
                    Doppel doppel1 = new Doppel(new Spieler(IDK1, parts[1]), new Spieler(IDA1, parts[3]));
                    Int32.TryParse(parts[4], out Int32 IDK2);
                    Int32.TryParse(parts[6], out Int32 IDA2);
                    Doppel doppel2 = new Doppel(new Spieler(IDK2, parts[5]), new Spieler(IDA2, parts[7]));
                    GameViewModel game = new GameViewModel(doppel1, doppel2);
                    int length = parts.Length;
                    sets = (length - 8) / 2;
                    ObservableCollection<Satz> list2 = new ObservableCollection<Satz>();
                    for (int i = 8; i < length; i += 2)
                    {
                        if (Int32.TryParse(parts[i], out Int32 Punkte1)) { Punkte1 = 0; }
                        if (Int32.TryParse(parts[i + 1], out Int32 Punkte2)) { Punkte2 = 0; }
                        Satz Set = new Satz(); Set.Punkte1 = Punkte1; Set.Punkte2 = Punkte2; list2.Add(Set);
                    }
                    game.Sets = list2;
                    list.Add(game);
                }
            }
            return list;
        }
    }
}
