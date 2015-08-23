using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MMS
{
    public partial class ConvertOpcije : Form
    {
        internal Form1 roditelj;
        private Aumpel audioConverter = new Aumpel();
        private Aumpel.soundFormat inputFileFormat;
        private Aumpel.soundFormat outputFileFormat;        
        public static int soundFileSize = 0;

        public ConvertOpcije()
        {
            InitializeComponent();          
        }
                
        private void Konvertuj_Click(object sender, EventArgs e)
        {
            Konvertuj.Enabled = false;            
            backgroundWorker1.RunWorkerAsync();
        }
                
        private void ReportStatus(int totalBytes, int processedBytes, Aumpel aumpelObj)
        {
            SetControlPropertyValue(progressBar1, "value", (int)(((float)processedBytes / (float)totalBytes) * 100));
        }

        private bool ReportStatusMad(uint frameCount, uint byteCount, ref MadlldlibWrapper.mad_header mh)
        {
            SetControlPropertyValue(progressBar1, "value", (int)(((float)byteCount / (float)soundFileSize) * 100));
            return true;
        }

        private void ConvertOpcije_Load(object sender, EventArgs e)
        {
            Naziv.Text = roditelj.vratiIme(roditelj.aktuelnaPjesma);

            if (roditelj.formatPjesme == "mp3")
            {
                inputFileFormat = Aumpel.soundFormat.MP3;
                mp3.Enabled = false;
                wav.Checked = true;
                outputFileFormat = Aumpel.soundFormat.WAV;
            }
            else if (roditelj.formatPjesme == "wav")
            {
                inputFileFormat = Aumpel.soundFormat.WAV;
                wav.Enabled = false;
                mp3.Checked = true;
                outputFileFormat = Aumpel.soundFormat.MP3;
            }
            else
            {
                wav.Enabled = false;
                mp3.Enabled = false;
                Konvertuj.Enabled = false;
            }
        }

        
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {            
            Konvertovanje();
        }

        //konvertovanje        
        private void Konvertovanje()
        {
            if ((int)outputFileFormat == (int)Aumpel.soundFormat.MP3)
            {
                try
                {
                    Aumpel.Reporter defaultCallback = new Aumpel.Reporter(ReportStatus);

                    FileInfo fi = new FileInfo(roditelj.aktuelnaPjesma);
                    soundFileSize = (int)fi.Length;


                    SaveFileDialog saveFile = new SaveFileDialog();
                    saveFile.Filter = "MP3 (*.mp3)|*.mp3";


                    if (saveFile.ShowDialog() == DialogResult.OK)
                    {
                        audioConverter.Convert(roditelj.aktuelnaPjesma,
                            (int)inputFileFormat, saveFile.FileName,
                            (int)outputFileFormat, defaultCallback);
                        SetControlPropertyValue(progressBar1, "value", 0);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }

            }
            else if ((int)inputFileFormat == (int)Aumpel.soundFormat.MP3)
            {

                try
                {
                    
                    MadlldlibWrapper.Callback defaultCallback =
                        new MadlldlibWrapper.Callback(ReportStatusMad);

                    
                    FileInfo fi = new FileInfo(roditelj.aktuelnaPjesma);
                    soundFileSize = (int)fi.Length;

                    
                    SaveFileDialog saveFile = new SaveFileDialog();
                    saveFile.Filter = "WAV (*.wav)|*.wav";

                    
                    if (saveFile.ShowDialog() == DialogResult.OK)
                    {
                        audioConverter.Convert(roditelj.aktuelnaPjesma,
                            saveFile.FileName, outputFileFormat, defaultCallback);
                        SetControlPropertyValue(progressBar1, "value", 0);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }

            }           
        }

        delegate void SetControlValueCallback(Control oControl, string propName, object propValue);

        private void SetControlPropertyValue(Control oControl, string propName, object propValue)
        {
            if (oControl.InvokeRequired)
            {
                SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
                oControl.Invoke(d, new object[] { oControl, propName, propValue });
            }
            else
            {
                Type t = oControl.GetType();
                System.Reflection.PropertyInfo[] props = t.GetProperties();                
                foreach (System.Reflection.PropertyInfo p in props)
                {
                    if (p.Name.ToUpper() == propName.ToUpper())
                    {
                        p.SetValue(oControl, propValue, null);
                    }
                }
            }
        }
    }
}