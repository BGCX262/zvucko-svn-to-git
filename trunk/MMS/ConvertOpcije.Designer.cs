namespace MMS
{
    partial class ConvertOpcije
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConvertOpcije));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.wav = new System.Windows.Forms.RadioButton();
            this.mp3 = new System.Windows.Forms.RadioButton();
            this.Konvertuj = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.Naziv = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.wav);
            this.groupBox1.Controls.Add(this.mp3);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(128, 79);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // wav
            // 
            this.wav.AutoSize = true;
            this.wav.Checked = true;
            this.wav.Location = new System.Drawing.Point(6, 19);
            this.wav.Name = "wav";
            this.wav.Size = new System.Drawing.Size(50, 17);
            this.wav.TabIndex = 4;
            this.wav.TabStop = true;
            this.wav.Text = "WAV";
            this.wav.UseVisualStyleBackColor = true;
            // 
            // mp3
            // 
            this.mp3.AutoSize = true;
            this.mp3.Location = new System.Drawing.Point(6, 42);
            this.mp3.Name = "mp3";
            this.mp3.Size = new System.Drawing.Size(47, 17);
            this.mp3.TabIndex = 5;
            this.mp3.Text = "MP3";
            this.mp3.UseVisualStyleBackColor = true;
            // 
            // Konvertuj
            // 
            this.Konvertuj.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Konvertuj.ForeColor = System.Drawing.Color.White;
            this.Konvertuj.Location = new System.Drawing.Point(13, 114);
            this.Konvertuj.Name = "Konvertuj";
            this.Konvertuj.Size = new System.Drawing.Size(128, 23);
            this.Konvertuj.TabIndex = 1;
            this.Konvertuj.Text = "Konvertuj";
            this.Konvertuj.UseVisualStyleBackColor = true;
            this.Konvertuj.Click += new System.EventHandler(this.Konvertuj_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(13, 141);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(128, 14);
            this.progressBar1.TabIndex = 2;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // Naziv
            // 
            this.Naziv.AutoSize = true;
            this.Naziv.ForeColor = System.Drawing.Color.White;
            this.Naziv.Location = new System.Drawing.Point(13, 13);
            this.Naziv.Name = "Naziv";
            this.Naziv.Size = new System.Drawing.Size(34, 13);
            this.Naziv.TabIndex = 3;
            this.Naziv.Text = "Naziv";
            // 
            // ConvertOpcije
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.IndianRed;
            this.ClientSize = new System.Drawing.Size(153, 165);
            this.Controls.Add(this.Naziv);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.Konvertuj);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConvertOpcije";
            this.ShowInTaskbar = false;
            this.Text = "Konvertovanje";
            this.Load += new System.EventHandler(this.ConvertOpcije_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton wav;
        private System.Windows.Forms.RadioButton mp3;
        private System.Windows.Forms.Button Konvertuj;
        public System.Windows.Forms.ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label Naziv;
    }
}