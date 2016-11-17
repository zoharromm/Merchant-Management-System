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
            this.Go = new System.Windows.Forms.Button();
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
            this.panel1.Controls.Add(this.totalrecord);
            this.panel1.Controls.Add(this._lblerror);
            this.panel1.Controls.Add(this._percent);
            this.panel1.Controls.Add(this._Bar1);
            this.panel1.Controls.Add(this.Go);
            this.panel1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Location = new System.Drawing.Point(47, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(496, 176);
            this.panel1.TabIndex = 0;
            // 
            // totalrecord
            // 
            this.totalrecord.AutoSize = true;
            this.totalrecord.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalrecord.ForeColor = System.Drawing.Color.DarkOrange;
            this.totalrecord.Location = new System.Drawing.Point(38, 120);
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
            this._lblerror.Location = new System.Drawing.Point(38, 0);
            this._lblerror.Name = "_lblerror";
            this._lblerror.Size = new System.Drawing.Size(52, 17);
            this._lblerror.TabIndex = 6;
            this._lblerror.Text = "label2";
            // 
            // _percent
            // 
            this._percent.AutoSize = true;
            this._percent.Location = new System.Drawing.Point(245, 84);
            this._percent.Name = "_percent";
            this._percent.Size = new System.Drawing.Size(35, 13);
            this._percent.TabIndex = 5;
            this._percent.Text = "label1";
            // 
            // _Bar1
            // 
            this._Bar1.Location = new System.Drawing.Point(41, 84);
            this._Bar1.Name = "_Bar1";
            this._Bar1.Size = new System.Drawing.Size(198, 22);
            this._Bar1.TabIndex = 4;
            // 
            // Go
            // 
            this.Go.BackColor = System.Drawing.Color.LightSalmon;
            this.Go.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Go.ForeColor = System.Drawing.Color.White;
            this.Go.Location = new System.Drawing.Point(41, 38);
            this.Go.Name = "Go";
            this.Go.Size = new System.Drawing.Size(75, 31);
            this.Go.TabIndex = 0;
            this.Go.Text = "Go";
            this.Go.UseVisualStyleBackColor = false;
            this.Go.Click += new System.EventHandler(this.Go_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Salmon;
            this.ClientSize = new System.Drawing.Size(580, 224);
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
        private System.Windows.Forms.Button Go;
        private System.Windows.Forms.Label totalrecord;
    }
}

