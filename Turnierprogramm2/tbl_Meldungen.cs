using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Extensions;
using MatheExtras;
//öffnen der XML
//using static  Turnierprogramm.data;

namespace Turnierprogramm
{
     partial class tbl_Meldungen : Form
    {
        public tbl_Meldungen()
        {
            InitializeComponent();

        }
      
            int z = 0;
            DataSet dataset = new DataSet();
            public int anzRunden = 5;
            DataTable tblM = new DataTable();
            public DataTable[] tblSpieler = new DataTable[2];
        public int maxPkt = konst.maxPoints;     //maximale Punktzahl
            DataRow row;
            private Spieler[] kind;
        private Spieler[] adult;



            int anzSpieler;    //Anzahl der Kinder bzw. Eltern
            string dateiName = "Meldungen/Meldungen.txt";   //txt Dateipfad
                                  
            
            private void tbl_Meldungen_Load(object sender, EventArgs e)
            {
            
            /*
             Tabelle für Meldungen
             */

            tblSpieler[0] = TabelleSpieler.Build("ErgebnisseKinder");
            tblSpieler[1] = TabelleSpieler.Build("ErgebnisseErwachsene");
            for (int i = 0; i < 2; i++)
            {
              
                for (int runde=1;runde<=anzRunden;runde++)
                {
                    string partnerName = "partnername " + runde;
                    string partnerID = "partnerID " + runde;
                    tblSpieler[i].Columns.Add(partnerID, typeof(int));
                    tblSpieler[i].Columns.Add(partnerName, typeof(string));
                    
                }
            }
            
            row = tblM.NewRow();
            tblM.Columns.Add("ID", typeof(int));
            tblM.Columns.Add("Name Kind", typeof(string));
            tblM.Columns.Add("Level Kind", typeof(int));
            tblM.Columns.Add("Name Erwachsener", typeof(string));
            tblM.Columns.Add("Level Erwachsener", typeof(int));
            tblM.Rows.Add(row);

            dataset.Tables.Add(tblM);

            datagridview1.DataSource = tblM;      //Tabelleninhalt mit Datagridview verknüpfen
  
            //ändern der Spaltenfarbe und erste Spalte auf nicht sichtbar stellen

            datagridview1.Columns["ID"].Visible = false;
            datagridview1.Columns[3].DefaultCellStyle.BackColor = Color.BlanchedAlmond;
            datagridview1.Columns[4].DefaultCellStyle.BackColor = Color.BlanchedAlmond;

            //Spaltenbreite           
            int width_level = 110;
            datagridview1.Columns["Level Kind"].Width = width_level;
            datagridview1.Columns["Level Erwachsener"].Width = width_level;
            datagridview1.Columns["Name Kind"].Width = konst.col_width;
            datagridview1.Columns["Name Erwachsener"].Width = konst.col_width;
            datagridview1.Size = new Size(konst.col_width*2+width_level*2+40,800);
            //datagridview1.AutoSizeColumnsMode =DataGridViewAutoSizeColumnsMode.AllCells;
            //datagridview1.AutoResizeColumns();
            DataGridViewCellStyle style = datagridview1.ColumnHeadersDefaultCellStyle;
            style.Font = new Font(datagridview1.Font, FontStyle.Bold);
            int dgvWidth = 0;
            foreach (DataGridViewColumn col in datagridview1.Columns)
            {
                dgvWidth += col.Width;
            }

            datagridview1.CellValidating += dgv_CellValidatingNumber;
            textBox_r.Text = anzRunden.ToString();
            textBox_r.Validating += textBox_r_Validating;
            lbl_maxPkt.Text = "maximale Punktzahl" + Environment.NewLine + "in einem Satz:";
            textBoxPkt.Text = konst.maxPoints.ToString();
            textBoxPkt.Validating +=textBoxPkt_Validating;
            
        }

        
        


        //Zeile ergänzen
        private void btnAddRow_Click(object sender, EventArgs e)
        {
            row= tblM.NewRow();
            row[0] = z;
            tblM.Rows.Add(row);
            z++;
        }

        private void Del_row_Click(object sender, EventArgs e)
        {
            if (datagridview1.RowCount > 1)
            {
                tblM.Rows.RemoveAt(datagridview1.CurrentCell.RowIndex);
                z--;
            }
            
        }

        /// <summary>
        /// kontrolliert ob alle sichtbaren Felder !=""
        /// </summary>
        private Boolean checkEingaben(int anzRows)
        {
            Boolean checkEingabe = true;
            
            for (int i = 0; i < anzRows && checkEingabe; i++)
            {
                int c = 0;
                foreach (DataGridViewCell cell in datagridview1.Rows[i].Cells)
                {
                    if (cell.Visible == true)
                    {
                        if (cell.Value.ToString() == "")
                        {
                            datagridview1.CurrentCell = datagridview1.Rows[i].Cells[c];
                            datagridview1.BeginEdit(true);
                            //MessageBox.Show("leere Zelle","Fehler");
                            checkEingabe = false;
                        }
                    }
                    c++;
                }
            }
            return checkEingabe;
        }

        private void startFirstRound_Click(object sender, EventArgs e)    //startet die nächste Runde
        {
            
            datagridview1.MultiSelect = true;
            
            anzSpieler = datagridview1.Rows.Count-1;
            bool checkEingabe = checkEingaben(anzSpieler);
            if (anzSpieler > 1&&checkEingabe)
            {
                CreateSpieler();              

                foreach (DataTable tbl in tblSpieler) { tbl.Clear(); }  //löschen aller Einträge in tblSpieler    */

                tblSpieler[0] = kind.ToTable("ErgebnisseKinder");
                tblSpieler[1] = adult.ToTable("ErgebnisseErwachsene");
                
                tblSpieler[0].ToTxt("Ergebnisse/",tblSpieler[0].TableName+"Runde0",true);
                tblSpieler[1].ToTxt("Ergebnisse/", tblSpieler[1].TableName + "Runde0",true);
                Doppel[] doppel = new Doppel[anzSpieler];
                for (int i = 0; i < anzSpieler; i++)
                {
                    doppel[i] = DoppelExt.buildDoppel(kind[i], adult[i]);
                }
                //Bubblesort
                doppel = DoppelExt.SortDoppel(doppel);
                maxPkt = Int32.Parse(textBoxPkt.Text);

                TurnierEinstellungen turnierData = TurnierEinstellungen.Instance();
                turnierData.SetProperties(anzRunden, maxPkt, FreilosMethods.DecideIfFreilosVorhanden(kind.Length));
                Turnier turnier = Turnier.Instance();
                turnier.SetTurnier(turnierData, kind, adult);
               
                // Runde FormsRunde = new Runde(doppel, 1, anzRunden, dataset, tblM, tblSpieler, maxPkt);
                Runde runde1 = new Runde(1);
                RundeFrm FormsRunde = new RundeFrm(runde1,doppel);
                //RundeFrm FormsRunde = new RundeFrm(doppel,kind,adult,maxPkt,1,anzRunden);
                FormsRunde.Show();    // möglich zwischen zwei Fenstern zu wechseln
                //Form.Close();
                //FormsRunde.ShowDialog();// kann nur noch das neu geöffnete Windows Forms genutzt werden
            }
            else
            {
                MessageBox.Show("Überprüfen sie ihre Eingaben");
            };
          

        }

        /// <summary>
        /// erstellt aus der Tabelle Meldungen die Spieler[] kind,adult;
        /// </summary>
        private void CreateSpieler()
        {
            kind = new Spieler[anzSpieler];
            adult = new Spieler[anzSpieler];
            for (int i = 0; i < anzSpieler; i++)
            {
                tblM.Rows[i]["ID"] = i;
                kind[i] = new Spieler();
                
                kind[i].ID = i;
                kind[i].Name = tblM.Rows[i]["Name Kind"].ToString();
                kind[i].Level = Convert.ToInt32(tblM.Rows[i]["Level Kind"]);

                adult[i] = new Spieler();
                adult[i].ID = i;
                adult[i].Name = tblM.Rows[i]["Name Erwachsener"].ToString();
                adult[i].Level = Convert.ToInt32(tblM.Rows[i]["Level Erwachsener"]);
            }
        }

        /// <summary>
        /// einlesen der gespeicherten Tabelle der Meldungen
        /// </summary>
        private void read_xml_Click(object sender, EventArgs e)
        {
            if (File.Exists(dateiName))
            {
                tblM = Extensions.Import.ImportTbl(tblM, dateiName, false);
            }
            else
            { MessageBox.Show("keine Daten vorhanden"); }
           
        }

        /// <summary>
        /// speichert die aktuelle Tabelle der Meldungen in eine txt
        /// </summary>
        private void write_txt_Click(object sender, EventArgs e)
        {
            anzSpieler = datagridview1.Rows.Count - 1;
            for (int i = 0; i < anzSpieler; i++)
            {
                tblM.Rows[i]["ID"] = i.ToString();
            }
            tblM.ToTxt("Meldungen/", "Meldungen",false, ".txt");
        }


        private void textBox_r_TextChanged(object sender, EventArgs e)
            {

                if (Int32.TryParse(textBox_r.Text.ToString(), out int maxPkt))
                {
                    int runden = (int)Convert.ToInt64(textBox_r.Text);
                    if (runden > 0)
                    {
                        anzRunden = runden;
                    }
                    else
                    {
                        MessageBox.Show("Fehlerhafte Eingabe");
                    }

                }
                else
                {
                    MessageBox.Show("Fehlerhafte Eingabe");
                }
            }

        private void dgv_CellValidatingNumber(Object sender, DataGridViewCellValidatingEventArgs e)
            {
                if (Convert.ToString(e.FormattedValue) != "")
                {
                    if (e.ColumnIndex == 2 || e.ColumnIndex == 4)
                    {

                        int number;
                        if (!int.TryParse(Convert.ToString(e.FormattedValue), out number))
                        {
                            e.Cancel = true;
                            MessageBox.Show("keine Zahl");
                        }
                        else
                        {
                            number = int.Parse(Convert.ToString(e.FormattedValue));
                            if (number < 0 || number > 10)
                            {
                                e.Cancel = true;
                                MessageBox.Show("ungültige Zahl");
                            }
                        }
                    }
                }
            }

        /// <summary>
    /// überprüft die maximal eingegebene Punktzahl
    /// </summary>
       private void textBoxPkt_Validating(Object sender, CancelEventArgs e)
        {    
            if (Int32.TryParse(textBoxPkt.Text, out int maxPkt))
            {
                int Punkte = (int)Convert.ToInt64(textBoxPkt.Text);
                if (Punkte > 0)
                {
                    maxPkt = Punkte;
                }
                else
                {
                    MessageBox.Show("Fehlerhafte Eingabe");
                    e.Cancel = true;
                }
            }
            else
            {
                MessageBox.Show("Fehlerhafte Eingabe");
                e.Cancel = true;
            }
        }

        /// <summary>
        /// überprüft die eingegebene maximale Rundenanzahl
        /// </summary>
        private void textBox_r_Validating(Object sender, CancelEventArgs e)
        {
            if (Int32.TryParse(textBox_r.Text, out int maxPkt))
            {
                int runden = (int)Convert.ToInt64(textBox_r.Text);
                if (runden > 0)
                {
                   anzRunden = runden;
                }
                else
                {
                    MessageBox.Show("Fehlerhafte Eingabe");
                    e.Cancel = true;
                }
            }
            else
            {
                MessageBox.Show("Fehlerhafte Eingabe");
                e.Cancel = true;
            }
        }

       private void button2_Click(object sender, EventArgs e)
        {
            int Runde = 4;
            string Ordner;
            string Dateiname;
            foreach (DataTable tbl in tblSpieler)
            {
                 Ordner = "Ergebnisse/";
                 Dateiname=tbl.TableName + "Runde" + (Runde-1).ToString() + ".txt";
                Dateiname = Ordner + Dateiname;
                tbl.ImportTbl(Dateiname, false, false);
            }
            show.Table(tblSpieler[0]);
            DataTable tblRunde = new DataTable();
            tblRunde.Columns.Add("IDK1", typeof(int));
            tblRunde.Columns.Add("Name Kind1", typeof(string));
            tblRunde.Columns.Add("IDE1", typeof(int));
            tblRunde.Columns.Add("Name Erwachsener1", typeof(string));

            tblRunde.Columns.Add("Punkte 1", typeof(int));
            tblRunde.Columns.Add("vs.", typeof(string));
            tblRunde.Columns.Add("Punkte 2", typeof(int));

            tblRunde.Columns.Add("IDK2", typeof(int));
            tblRunde.Columns.Add("Name Kind2", typeof(string));
            tblRunde.Columns.Add("IDE2", typeof(int));
            tblRunde.Columns.Add("Name Erwachsener2", typeof(string));
            Ordner = "Rundenergebnisse/";
            Dateiname =Ordner+ "Ergebnisse" + "Runde" + (Runde).ToString() + ".txt";
            dateiName = "EgebnisseRunde" + Runde.ToString() + ".txt";
            string dateiPfad = "Rundenergebnisse/" + dateiName;
            tblRunde.ImportTbl(dateiPfad, false, false);
            /*
            Form Runde_frm = new RundeFrm(tblSpieler, tblRunde, maxPkt, Runde, anzRunden);
            Runde_frm.ShowDialog();*/

        }
    }
}
