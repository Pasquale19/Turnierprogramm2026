namespace Turnierprogramm
{
    partial class tbl_Meldungen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tbl_Meldungen));
            this.datagridview1 = new System.Windows.Forms.DataGridView();
            this.btn_AddRow = new System.Windows.Forms.Button();
            this.Del_row = new System.Windows.Forms.Button();
            this.startFirstRound = new System.Windows.Forms.Button();
            this.read_txt = new System.Windows.Forms.Button();
            this.write_txt = new System.Windows.Forms.Button();
            this.textBox_r = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_maxPkt = new System.Windows.Forms.Label();
            this.textBoxPkt = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.datagridview1)).BeginInit();
            this.SuspendLayout();
            // 
            // datagridview1
            // 
            this.datagridview1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.datagridview1.Location = new System.Drawing.Point(16, 119);
            this.datagridview1.Name = "datagridview1";
            this.datagridview1.Size = new System.Drawing.Size(599, 328);
            this.datagridview1.TabIndex = 0;
            // 
            // btn_AddRow
            // 
            this.btn_AddRow.Location = new System.Drawing.Point(175, 12);
            this.btn_AddRow.Name = "btn_AddRow";
            this.btn_AddRow.Size = new System.Drawing.Size(121, 23);
            this.btn_AddRow.TabIndex = 1;
            this.btn_AddRow.Text = "Spieler hinzufügen";
            this.btn_AddRow.UseVisualStyleBackColor = true;
            this.btn_AddRow.Click += new System.EventHandler(this.btnAddRow_Click);
            // 
            // Del_row
            // 
            this.Del_row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Del_row.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Del_row.ForeColor = System.Drawing.Color.Black;
            this.Del_row.Location = new System.Drawing.Point(175, 41);
            this.Del_row.Name = "Del_row";
            this.Del_row.Size = new System.Drawing.Size(121, 23);
            this.Del_row.TabIndex = 2;
            this.Del_row.Text = "Zeile löschen";
            this.Del_row.UseVisualStyleBackColor = false;
            this.Del_row.Click += new System.EventHandler(this.Del_row_Click);
            // 
            // startFirstRound
            // 
            this.startFirstRound.Location = new System.Drawing.Point(543, 12);
            this.startFirstRound.Name = "startFirstRound";
            this.startFirstRound.Size = new System.Drawing.Size(111, 52);
            this.startFirstRound.TabIndex = 4;
            this.startFirstRound.Text = "1. Runde starten";
            this.startFirstRound.UseVisualStyleBackColor = true;
            this.startFirstRound.Click += new System.EventHandler(this.startFirstRound_Click);
            // 
            // read_txt
            // 
            this.read_txt.Location = new System.Drawing.Point(12, 12);
            this.read_txt.Name = "read_txt";
            this.read_txt.Size = new System.Drawing.Size(141, 23);
            this.read_txt.TabIndex = 5;
            this.read_txt.Text = "Meldungen einlesen";
            this.read_txt.UseVisualStyleBackColor = true;
            this.read_txt.Click += new System.EventHandler(this.read_xml_Click);
            // 
            // write_txt
            // 
            this.write_txt.Location = new System.Drawing.Point(12, 41);
            this.write_txt.Name = "write_txt";
            this.write_txt.Size = new System.Drawing.Size(141, 23);
            this.write_txt.TabIndex = 6;
            this.write_txt.Text = "Meldungen speichern";
            this.write_txt.UseVisualStyleBackColor = true;
            this.write_txt.Click += new System.EventHandler(this.write_txt_Click);
            // 
            // textBox_r
            // 
            this.textBox_r.Location = new System.Drawing.Point(424, 29);
            this.textBox_r.Name = "textBox_r";
            this.textBox_r.Size = new System.Drawing.Size(100, 20);
            this.textBox_r.TabIndex = 7;
            this.textBox_r.TextChanged += new System.EventHandler(this.textBox_r_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(317, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Anzahl der Runden:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 24);
            this.label2.TabIndex = 9;
            this.label2.Text = "Meldungen";
            // 
            // lbl_maxPkt
            // 
            this.lbl_maxPkt.AutoSize = true;
            this.lbl_maxPkt.Location = new System.Drawing.Point(317, 73);
            this.lbl_maxPkt.Name = "lbl_maxPkt";
            this.lbl_maxPkt.Size = new System.Drawing.Size(100, 13);
            this.lbl_maxPkt.TabIndex = 10;
            this.lbl_maxPkt.Text = "maximale Punktzahl";
            // 
            // textBoxPkt
            // 
            this.textBoxPkt.Location = new System.Drawing.Point(424, 70);
            this.textBoxPkt.Name = "textBoxPkt";
            this.textBoxPkt.Size = new System.Drawing.Size(100, 20);
            this.textBoxPkt.TabIndex = 11;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(543, 73);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(111, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "btn_loadZwischenstand";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tbl_Meldungen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 527);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBoxPkt);
            this.Controls.Add(this.lbl_maxPkt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_r);
            this.Controls.Add(this.write_txt);
            this.Controls.Add(this.read_txt);
            this.Controls.Add(this.startFirstRound);
            this.Controls.Add(this.Del_row);
            this.Controls.Add(this.btn_AddRow);
            this.Controls.Add(this.datagridview1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "tbl_Meldungen";
            this.Text = "tbl_Meldungen";
            this.Load += new System.EventHandler(this.tbl_Meldungen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.datagridview1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView datagridview1;
        private System.Windows.Forms.Button btn_AddRow;
        private System.Windows.Forms.Button Del_row;
        private System.Windows.Forms.Button startFirstRound;
        private System.Windows.Forms.Button read_txt;
        private System.Windows.Forms.Button write_txt;
        private System.Windows.Forms.TextBox textBox_r;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_maxPkt;
        private System.Windows.Forms.TextBox textBoxPkt;
        private System.Windows.Forms.Button button2;
    }
}