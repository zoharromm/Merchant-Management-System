namespace palyerborndate
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.totalrecord = new System.Windows.Forms.Label();
            this._lblerror = new System.Windows.Forms.Label();
            this._percent = new System.Windows.Forms.Label();
            this._Bar1 = new System.Windows.Forms.ProgressBar();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Pause = new System.Windows.Forms.Button();
            this.createcsvfile = new System.Windows.Forms.Button();
            this.Go = new System.Windows.Forms.Button();
            this.chkstorelist = new System.Windows.Forms.CheckedListBox();
            this.lblstore = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.lblstore);
            this.panel1.Controls.Add(this.chkstorelist);
            this.panel1.Controls.Add(this.totalrecord);
            this.panel1.Controls.Add(this._lblerror);
            this.panel1.Controls.Add(this._percent);
            this.panel1.Controls.Add(this._Bar1);
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Controls.Add(this.Pause);
            this.panel1.Controls.Add(this.createcsvfile);
            this.panel1.Controls.Add(this.Go);
            this.panel1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Location = new System.Drawing.Point(47, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(845, 401);
            this.panel1.TabIndex = 0;
            // 
            // totalrecord
            // 
            this.totalrecord.AutoSize = true;
            this.totalrecord.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalrecord.ForeColor = System.Drawing.Color.DarkOrange;
            this.totalrecord.Location = new System.Drawing.Point(643, 87);
            this.totalrecord.Name = "totalrecord";
            this.totalrecord.Size = new System.Drawing.Size(52, 17);
            this.totalrecord.TabIndex = 7;
            this.totalrecord.Text = "label2";
            this.totalrecord.Click += new System.EventHandler(this.totalrecord_Click);
            // 
            // _lblerror
            // 
            this._lblerror.AutoSize = true;
            this._lblerror.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblerror.ForeColor = System.Drawing.Color.DarkRed;
            this._lblerror.Location = new System.Drawing.Point(57, 10);
            this._lblerror.Name = "_lblerror";
            this._lblerror.Size = new System.Drawing.Size(52, 17);
            this._lblerror.TabIndex = 6;
            this._lblerror.Text = "label2";
            // 
            // _percent
            // 
            this._percent.AutoSize = true;
            this._percent.Location = new System.Drawing.Point(597, 89);
            this._percent.Name = "_percent";
            this._percent.Size = new System.Drawing.Size(35, 13);
            this._percent.TabIndex = 5;
            this._percent.Text = "label1";
            this._percent.Click += new System.EventHandler(this._percent_Click);
            // 
            // _Bar1
            // 
            this._Bar1.Location = new System.Drawing.Point(393, 82);
            this._Bar1.Name = "_Bar1";
            this._Bar1.Size = new System.Drawing.Size(198, 22);
            this._Bar1.TabIndex = 4;
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(14, 156);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(814, 234);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Pause
            // 
            this.Pause.BackColor = System.Drawing.Color.LightSalmon;
            this.Pause.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Pause.ForeColor = System.Drawing.Color.White;
            this.Pause.Location = new System.Drawing.Point(491, 38);
            this.Pause.Name = "Pause";
            this.Pause.Size = new System.Drawing.Size(85, 31);
            this.Pause.TabIndex = 2;
            this.Pause.Text = "Pause";
            this.Pause.UseVisualStyleBackColor = false;
            this.Pause.Click += new System.EventHandler(this.Pause_Click);
            // 
            // createcsvfile
            // 
            this.createcsvfile.BackColor = System.Drawing.Color.LightSalmon;
            this.createcsvfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createcsvfile.ForeColor = System.Drawing.Color.White;
            this.createcsvfile.Location = new System.Drawing.Point(600, 38);
            this.createcsvfile.Name = "createcsvfile";
            this.createcsvfile.Size = new System.Drawing.Size(216, 31);
            this.createcsvfile.TabIndex = 1;
            this.createcsvfile.Text = "Create Tab Delimated File";
            this.createcsvfile.UseVisualStyleBackColor = false;
            this.createcsvfile.Click += new System.EventHandler(this.createcsvfile_Click);
            // 
            // Go
            // 
            this.Go.BackColor = System.Drawing.Color.LightSalmon;
            this.Go.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Go.ForeColor = System.Drawing.Color.White;
            this.Go.Location = new System.Drawing.Point(393, 38);
            this.Go.Name = "Go";
            this.Go.Size = new System.Drawing.Size(75, 31);
            this.Go.TabIndex = 0;
            this.Go.Text = "Go";
            this.Go.UseVisualStyleBackColor = false;
            this.Go.Click += new System.EventHandler(this.Go_Click);
            // 
            // chkstorelist
            // 
            this.chkstorelist.FormattingEnabled = true;
            this.chkstorelist.Items.AddRange(new object[] {
            "Warrior",
            "Chilly Chiles",
            "Airsoft",
            "KnifeZone",
            "LiveOutThere"});
            this.chkstorelist.Location = new System.Drawing.Point(188, 38);
            this.chkstorelist.Name = "chkstorelist";
            this.chkstorelist.Size = new System.Drawing.Size(155, 79);
            this.chkstorelist.TabIndex = 8;
            // 
            // lblstore
            // 
            this.lblstore.AutoSize = true;
            this.lblstore.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblstore.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblstore.Location = new System.Drawing.Point(11, 61);
            this.lblstore.Name = "lblstore";
            this.lblstore.Size = new System.Drawing.Size(151, 17);
            this.lblstore.TabIndex = 9;
            this.lblstore.Text = "Please Select Store";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Salmon;
            this.ClientSize = new System.Drawing.Size(934, 449);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Crawler";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label _lblerror;
        private System.Windows.Forms.Label _percent;
        private System.Windows.Forms.ProgressBar _Bar1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button Pause;
        private System.Windows.Forms.Button createcsvfile;
        private System.Windows.Forms.Button Go;
        private System.Windows.Forms.Label totalrecord;
        private System.Windows.Forms.CheckedListBox chkstorelist;
        private System.Windows.Forms.Label lblstore;
    }
}

