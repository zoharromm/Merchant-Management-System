namespace Crawler_WithouSizes_Part2
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
            this.btnsubmit = new System.Windows.Forms.Button();
            this.lblstore = new System.Windows.Forms.Label();
            this.chkstorelist = new System.Windows.Forms.CheckedListBox();
            this.totalrecord = new System.Windows.Forms.Label();
            this._lblerror = new System.Windows.Forms.Label();
            this._percent = new System.Windows.Forms.Label();
            this._Bar1 = new System.Windows.Forms.ProgressBar();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btnsubmit);
            this.panel1.Controls.Add(this.lblstore);
            this.panel1.Controls.Add(this.chkstorelist);
            this.panel1.Controls.Add(this.totalrecord);
            this.panel1.Controls.Add(this._lblerror);
            this.panel1.Controls.Add(this._percent);
            this.panel1.Controls.Add(this._Bar1);
            this.panel1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Location = new System.Drawing.Point(24, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(544, 229);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // btnsubmit
            // 
            this.btnsubmit.Location = new System.Drawing.Point(383, 61);
            this.btnsubmit.Name = "btnsubmit";
            this.btnsubmit.Size = new System.Drawing.Size(75, 23);
            this.btnsubmit.TabIndex = 10;
            this.btnsubmit.Text = "button1";
            this.btnsubmit.UseVisualStyleBackColor = true;
            this.btnsubmit.Click += new System.EventHandler(this.btnsubmit_Click);
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
            // chkstorelist
            // 
            this.chkstorelist.FormattingEnabled = true;
            this.chkstorelist.Items.AddRange(new object[] {
            "401games"});
            this.chkstorelist.Location = new System.Drawing.Point(188, 38);
            this.chkstorelist.Name = "chkstorelist";
            this.chkstorelist.Size = new System.Drawing.Size(155, 79);
            this.chkstorelist.TabIndex = 8;
            // 
            // totalrecord
            // 
            this.totalrecord.AutoSize = true;
            this.totalrecord.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalrecord.ForeColor = System.Drawing.Color.DarkOrange;
            this.totalrecord.Location = new System.Drawing.Point(14, 166);
            this.totalrecord.Name = "totalrecord";
            this.totalrecord.Size = new System.Drawing.Size(52, 17);
            this.totalrecord.TabIndex = 7;
            this.totalrecord.Text = "label2";
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
            this._percent.Location = new System.Drawing.Point(380, 135);
            this._percent.Name = "_percent";
            this._percent.Size = new System.Drawing.Size(35, 13);
            this._percent.TabIndex = 5;
            this._percent.Text = "label1";
            // 
            // _Bar1
            // 
            this._Bar1.Location = new System.Drawing.Point(17, 126);
            this._Bar1.Name = "_Bar1";
            this._Bar1.Size = new System.Drawing.Size(326, 22);
            this._Bar1.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Salmon;
            this.ClientSize = new System.Drawing.Size(597, 278);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Crawler";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label _lblerror;
        private System.Windows.Forms.Label _percent;
        private System.Windows.Forms.ProgressBar _Bar1;
        private System.Windows.Forms.Label totalrecord;
        private System.Windows.Forms.CheckedListBox chkstorelist;
        private System.Windows.Forms.Label lblstore;
        private System.Windows.Forms.Button btnsubmit;
    }
}

