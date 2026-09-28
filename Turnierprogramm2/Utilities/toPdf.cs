using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows;
using System.Windows.Controls;
using Turnierprogramm2.Models;

namespace Turnierprogramm2.Utilities
{
    public class toPDF
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="dateiPfad">Dateipfad ohne Endung .pdf angeben</param>
        /// <param name="title">Überschrift auf der Pdf</param>
        /// <param name="Cellbackground"></param>
        public static void DataGrid(DataGrid dgv, string dateiPfad, string title, bool Cellbackground) //dateiPfad enthält Pfad und Name ohne Endung .pdf
        {
            int anzCol = 0;
            foreach (DataGridColumn col in dgv.Columns)
            {
                if (col.Visibility == Visibility.Visible) anzCol++;

            }

            //Creating iTextSharp Table from the DataTable data
            PdfPTable pdfTable = new PdfPTable(anzCol);
            pdfTable.DefaultCell.Padding = 3;
            //pdfTable.WidthPercentage = 30;
            //pdfTable.TotalWidth = 500;

            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable.DefaultCell.BorderWidth = 1;
            float[] ColumnWidth = new float[dgv.Columns.Count];

            //Adding Header row
            int ColumnsCount = 0;
            foreach (DataGridColumn column in dgv.Columns)
            {
                if (column.Visibility == System.Windows.Visibility.Visible)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.Header.ToString()));
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(209, 209, 209);    //Grundfarbe
                    pdfTable.AddCell(cell);

                    ColumnWidth[ColumnsCount] = (float)column.ActualWidth;
                    ColumnsCount++;
                }
            }

            //setting columnWidth
            Array.Resize(ref ColumnWidth, ColumnsCount);
            pdfTable.SetWidths(ColumnWidth);
            var rows = GetDataGridRows(dgv);

            foreach (DataGridRow row in rows)
            {
                //DataRowView rowView = (DataRowView)row.Item;
                foreach (DataGridColumn column in dgv.Columns)
                {
                    if (column.GetCellContent(row) is TextBlock && column.Visibility == Visibility.Visible)
                    {
                        TextBlock cellContent = column.GetCellContent(row) as TextBlock;
                        //MessageBox.Show(cellContent.Text);

                        PdfPCell zelle = new PdfPCell(new Phrase(cellContent.Text));
                        //HintergrundFarbe
                        int[] RGB = new int[3] { 255, 255, 255 };

                        //if (Cellbackground)
                        //{
                        //column.CellStyle.
                        //    DataGridViewColumn col = cell.OwningColumn;
                        //    RGB = RGBConvert(col.DefaultCellStyle.BackColor);
                        //}
                        //else
                        //{
                        //    RGB = RGBConvert(row.DefaultCellStyle.BackColor);
                        //}

                        //MessageBox.Show("RGB: " + RGB[0].ToString() + "\t" + RGB[1].ToString() + konst.seperator + RGB[2].ToString());
                        zelle.BackgroundColor = new BaseColor(RGB[0], RGB[1], RGB[2]);
                        pdfTable.AddCell(zelle);
                    }
                    else
                    {
                        pdfTable.AddCell("");
                    }
                }

            }

            //Adding DataRow
            //    foreach (System.Data.DataRowView row in dgv.ItemsSource)
            //{
            //    DataRow Row2 = row.Row;

            //    int colIndex = 0;
            //    foreach (DataGridCell cell in Row2.)
            //    {
            //        if (cell.Visible)
            //        {
            //            if (cell.Value != null)
            //            {
            //                PdfPCell zelle = new PdfPCell(new Phrase(cell.Value.ToString()));
            //                //HintergrundFarbe
            //                int[] RGB = new int[3];

            //                if (Cellbackground)
            //                {
            //                    DataGridViewColumn col = cell.OwningColumn;
            //                    RGB = RGBConvert(col.DefaultCellStyle.BackColor);
            //                }
            //                else
            //                {
            //                    RGB = RGBConvert(row.DefaultCellStyle.BackColor);
            //                }

            //                //MessageBox.Show("RGB: " + RGB[0].ToString() + "\t" + RGB[1].ToString() + konst.seperator + RGB[2].ToString());
            //                zelle.BackgroundColor = new BaseColor(RGB[0], RGB[1], RGB[2]);
            //                pdfTable.AddCell(zelle);
            //            }
            //            else
            //            {
            //                pdfTable.AddCell("");
            //            }
            //        }
            //        colIndex++;
            //    }
            //}

            //Exporting to PDF
            string folderPath = dateiPfad;
            if (!File.Exists(folderPath))
            {

            }
            //if (!Directory.Exists(folderPath))
            //{
            //    Directory.CreateDirectory(folderPath);
            //}

            using (FileStream stream = new FileStream(folderPath + ".pdf", FileMode.OpenOrCreate))
            {
                Document pdfDoc = new Document(PageSize.A4.Rotate(), 24, 60, 30, 30);
                pdfDoc.AddTitle(title);
                PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();

                Phrase HeadLine = new Phrase(title);
                HeadLine.Font.Size = 16;
                HeadLine.Font.SetStyle(1);
                pdfDoc.Add(HeadLine);
                pdfDoc.Add(pdfTable);
                pdfDoc.Close();
                stream.Close();
            }
        }



        //private static int[] RGBConvert(System.Drawing.Color c)
        //{
        //    int[] RGB = new int[3];

        //    try
        //    {
        //        RGB[0] = Int32.Parse(c.R.ToString());
        //        RGB[1] = Convert.ToInt32(c.G);
        //        RGB[2] = Convert.ToInt32(c.B);

        //    }
        //    catch
        //    { }

        //    return RGB;
        //}

        public static IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            }
        }

        /// <summary>
        /// Dateipfad ohne Endung angeben
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="dateiPfad">Dateipfad ohne Endung .pdf angeben</param>
        /// <param name="title">Überschrift auf der Pdf</param>
        /// <param name="Cellbackground"></param>
        public static void DataGrid2(DataGrid dgv, string dateiPfad, string title, bool Cellbackground) //dateiPfad enthält Pfad und Name ohne Endung .pdf
        {
            List<int> VisibleColumnsIndex = new List<int>();
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                if (dgv.Columns[i].Visibility == Visibility.Visible) VisibleColumnsIndex.Add(i);
            }
            int anzCol = VisibleColumnsIndex.Count;
            //Creating iTextSharp Table from the DataTable data
            PdfPTable pdfTable = new PdfPTable(anzCol);
            pdfTable.DefaultCell.Padding = 3;
            //pdfTable.WidthPercentage = 30;
            //pdfTable.TotalWidth = 500;

            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable.DefaultCell.BorderWidth = 1;
            float[] ColumnWidth = new float[anzCol];

            //Adding Header row
            for (int i = 0; i < VisibleColumnsIndex.Count; i++)
            {
                DataGridColumn column = dgv.Columns[VisibleColumnsIndex[i]];
                PdfPCell cell = new PdfPCell(new Phrase(column.Header.ToString()));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(209, 209, 209);    //Grundfarbe
                pdfTable.AddCell(cell);
                ColumnWidth[i] = (float)column.ActualWidth*2;
            }

            //setting columnWidth
            pdfTable.SetWidths(ColumnWidth);
            var rows = GetDataGridRows(dgv);

            foreach (DataGridRow row in rows)
            {
                for (int i = 0; i < VisibleColumnsIndex.Count; i++)
                {
                    DataGridColumn column = dgv.Columns[VisibleColumnsIndex[i]];

                    if (column.GetCellContent(row) is TextBlock)
                    {
                        TextBlock cellContent = column.GetCellContent(row) as TextBlock;


                        PdfPCell zelle = new PdfPCell(new Phrase(cellContent.Text));
                        //HintergrundFarbe
                        int[] RGB = new int[3] { 255, 255, 255 };

                        //if (Cellbackground)
                        //{
                        //column.CellStyle.
                        //    DataGridViewColumn col = cell.OwningColumn;
                        //    RGB = RGBConvert(col.DefaultCellStyle.BackColor);
                        //}
                        //else
                        //{
                        //    RGB = RGBConvert(row.DefaultCellStyle.BackColor);
                        //}

                        //MessageBox.Show("RGB: " + RGB[0].ToString() + "\t" + RGB[1].ToString() + konst.seperator + RGB[2].ToString());
                        zelle.BackgroundColor = new BaseColor(RGB[0], RGB[1], RGB[2]);
                        pdfTable.AddCell(zelle);
                    }
                    else
                    {
                        //Type ContentPresenter
                        if (column.GetCellContent(row) is ContentPresenter)
                        {

                            ContentPresenter pres = (ContentPresenter)column.GetCellContent(row);
                            string text = pres.Content.ToString();
                            pdfTable.AddCell(text);
                            //MessageBox.Show($"{dgv.Columns[VisibleColumnsIndex[i]].Header.ToString()}: {pres.Content} and cellcontext to string {text}");
                        }
                        else { pdfTable.AddCell(""); }

                    }
                }

            }

            if (!File.Exists(dateiPfad)) { }
            //if (!Directory.Exists(folderPath))
            //{
            //    Directory.CreateDirectory(folderPath);//Fehler
            //}


            //remove Extension for safety and add Extension
            string exportPath = dateiPfad + (Path.HasExtension(dateiPfad) ? "" : ".pdf");
            try
            {
                using (FileStream stream = new FileStream(exportPath, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4.Rotate(), 24, 60, 30, 30);
                    pdfDoc.AddTitle(title);
                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    Phrase HeadLine = new Phrase(title);
                    HeadLine.Font.Size = 16;
                    HeadLine.Font.SetStyle(1);
                    pdfDoc.Add(HeadLine);
                    pdfDoc.Add(pdfTable);
                    pdfDoc.Close();
                    stream.Close();
                }
            }
            catch
            {
                MessageBox.Show("Export not succesfull");
            }

        }

        public static void ExportSpielerToPdf(
        List<Spieler> spielerListe,
        string fileName)
        {
            Document document = new Document(                PageSize.A4.Rotate(),                30f,                30f,                30f,                30f);

            using (FileStream stream = new FileStream(
                fileName,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None))
            {
                PdfWriter.GetInstance(document, stream);

                document.Open();

                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16f, BaseColor.BLACK);

                Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9f, BaseColor.WHITE);

                Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 8f, BaseColor.BLACK);

                Paragraph title = new Paragraph("Rangliste", titleFont);

                title.SpacingAfter = 12f;
                document.Add(title);

                // Columns:
                // #, Name, Siege, Sets, Punkte, Gegenpunkte, Partner
                PdfPTable table = new PdfPTable(7)
                {
                    WidthPercentage = 100f
                };

                table.SetWidths(new float[]
                {
            0.6f, // #
            2.5f, // Name
            1.0f, // Siege
            1.0f, // Sets
            1.0f, // Punkte
            1.5f, // Gegenpunkte
            3.4f  // Partner
                });

                AddHeaderCell(table, "#", headerFont);
                AddHeaderCell(table, "Name", headerFont);
                AddHeaderCell(table, "Siege", headerFont);
                AddHeaderCell(table, "Sätze", headerFont);
                AddHeaderCell(table, "Punkte", headerFont);
                AddHeaderCell(table, "Gegenpunkte", headerFont);
                AddHeaderCell(table, "Partner", headerFont);

                int index = 1;

                foreach (Spieler spieler in spielerListe)
                {
                    AddBodyCell(table, index.ToString(), cellFont);
                    AddBodyCell(table, spieler.Name, cellFont);
                    AddBodyCell(table, spieler.Siege.ToString(), cellFont);
                    AddBodyCell(table, spieler.Sets.ToString(), cellFont);
                    AddBodyCell(table, spieler.Punkte.ToString(), cellFont);
                    AddBodyCell(table, spieler.Gegenpunkte.ToString(), cellFont);

                    string partnerText = ConvertPartnerToString(spieler.Partner);

                    AddBodyCell(table, partnerText, cellFont);

                    index++;
                }

                document.Add(table);
                document.Close();
            }
        }
        private static string ConvertPartnerToString(
    IEnumerable<Spieler> partnerList)
        {
            if (partnerList == null)
                return string.Empty;

            return string.Join(
                "  |\t",
                partnerList
                    .Where(p => p != null)
                    .Select(p => p.Name ?? string.Empty));
        }

        private static void AddHeaderCell(
    PdfPTable table,
    string text,
    Font font)
        {
            PdfPCell cell = new PdfPCell(
                new Phrase(text ?? string.Empty, font));

            cell.BackgroundColor = new BaseColor(70, 110, 160);
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 5f;

            table.AddCell(cell);
        }

        private static void AddBodyCell(
            PdfPTable table,
            string text,
            Font font)
        {
            PdfPCell cell = new PdfPCell(
                new Phrase(text ?? string.Empty, font));

            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 4f;

            table.AddCell(cell);
        }


    }
}
