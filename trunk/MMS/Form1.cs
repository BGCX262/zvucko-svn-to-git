using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace MMS
{
    public partial class Form1 : Form
    {        
        protected IrrKlang.ISoundEngine irrKlangEngine;
        protected IrrKlang.ISound currentlyPlayingSound;
        private double vrijeme;
        private int interval;
        private System.Collections.Generic.List<String> playlista = new List<String>();
        public string aktuelnaPjesma;
        public string formatPjesme;
        private int indeksPjesme;
        private bool reprodukcija;
        private bool shuffle_ukljucen;
        private bool repeat_ukljucen;
        private int xPozicija, yPozicija;
        private int xPozicijaP, yPozicijaP;
        
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        public Form1()
        {
            InitializeComponent();                        
            vrijeme = 0;
            interval = 500;
            reprodukcija = false;
            shuffle_ukljucen = false;
            repeat_ukljucen = false;
            irrKlangEngine = new IrrKlang.ISoundEngine();
        }

        //Poèetak reproduciranja
        private void button1_Click(object sender, EventArgs e)
        {
            if (lista.Items.Count > 0 && lista.SelectedIndex >= 0)
                PokreniReprodukciju();
            else if (lista.Items.Count > 0)
            {
                lista.SelectedIndex = 0;
                pokreniIzListe();
            }
        }

        private void PokreniReprodukciju()
        {
            try
            {
                if (!reprodukcija)
                {
                    currentlyPlayingSound = irrKlangEngine.Play2D(aktuelnaPjesma, false, true);
                    currentlyPlayingSound.PlaybackSpeed = 1;
                                        
                    brzina.Text = "Brzina: 1";
                    interval = 500;
                    jacinaZvuka();                    
                    reprodukcija = true;
                }

                format.Text = "Format: " + vratiFormat(aktuelnaPjesma);
                formatPjesme = vratiFormat(aktuelnaPjesma);
                play.Visible = false;
                pauza.Visible = true;
                currentlyPlayingSound.Paused = false;
                timer.Start();                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ne postoji datoteka \n\nError: " + ex.Message.ToString(),
                                "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Dugme Otvori
        private void otvori_Click(object sender, EventArgs e)
        {   
            System.Windows.Forms.OpenFileDialog dialog = new
                System.Windows.Forms.OpenFileDialog();

            dialog.Filter = "All playable files (*.mp3, *.ogg;*.wav;*.mod;*.it;*.xm;*.it;*.s3d)|*.mp3;*.ogg;*.wav;*.mod;*.it;*.xm;*.it;*.s3d";
            dialog.FilterIndex = 0;
                        
            if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                Pjesma.Text = vratiIme(dialog.FileName);
                aktuelnaPjesma = dialog.FileName;
                indeksPjesme = 0;
                ocistiListu(); //Èišæenje liste                
                button3_Click(sender, e); //Zaustavljanje trenutne reprodukcije
                playlista.Clear(); //Èišæenje kolekcije putanja
                dodajListu(dialog.FileNames); //Dodavanje u listu
                xPozicija = MousePosition.X;
                yPozicija = MousePosition.Y;
            }

            panel4_MouseEnter(sender, e); //povlaèenje pokretnog menija
            button1_Click(sender, e); //Poèetak reprodukcije                        
        }

        private void ZaustaviReprodukciju()
        {
            if (currentlyPlayingSound != null)
            {
                reprodukcija = false;
                currentlyPlayingSound.Stop();
                resetTimer();
                play.Visible = true;
                pauza.Visible = false;                
            }
        }

        //Pauziranje reprodukcije
        private void button7_Click(object sender, EventArgs e)
        {
            if (currentlyPlayingSound != null && !currentlyPlayingSound.Paused)
            {
                currentlyPlayingSound.Paused = true;
                timer.Stop();
                pauza.Visible = false;
                play.Visible = true;                
            }            
        }
        
        //Reset timera
        private void resetTimer()
        {
            timer.Stop();
            vrijeme = 0;
            Vrijeme.Text = "Vrijeme: 00:00";
            play.Visible = true;
            pauza.Visible = false;            
            Pomjeri(0);
        }

        //Dodaj u listu
        private void button10_Click(object sender, EventArgs e)
        {   
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();

            dialog.Filter = "All playable files (*.mp3, *.ogg;*.wav;*.mod;*.it;*.xm;*.it;*.s3d)|*.mp3;*.ogg;*.wav;*.mod;*.it;*.xm;*.it;*.s3d";
            dialog.FilterIndex = 0;
            dialog.Multiselect = true;

            if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                dodajListu(dialog.FileNames);                                
            }

            xPozicija = MousePosition.X;
            yPozicija = MousePosition.Y;
            panel4_MouseEnter(sender, e);
        }

        private void lista_DoubleClick(object sender, EventArgs e)
        {
            pokreniIzListe();
        }

        private void pokreniIzListe()
        {
            ZaustaviReprodukciju();

            if (lista.SelectedIndex >= 0)
            {
                aktuelnaPjesma = playlista[lista.SelectedIndex];
                Pjesma.Text = lista.Items[lista.SelectedIndex].ToString();
                indeksPjesme = lista.SelectedIndex;
                PokreniReprodukciju();
            }            
        }        
        
        private void Slijedeca_Click(object sender, EventArgs e)
        {
            SlijedecaPjesma();
        }

        private void SlijedecaPjesma()
        {
            if (lista.Items.Count > 0)
            {
                if (!repeat_ukljucen)
                {
                    int i;
                    if (shuffle_ukljucen)
                    {
                        Random slucajni = new Random();
                        do
                        {
                            i = slucajni.Next(0, lista.Items.Count);
                        } while (i == indeksPjesme && lista.Items.Count > 1);
                    }
                    else
                    {
                        i = indeksPjesme + 1;
                    }

                    if (i < lista.Items.Count)
                        lista.SelectedIndex = i;
                    else
                        lista.SelectedIndex = 0;
                }
                else
                {
                    lista.SelectedIndex = indeksPjesme;
                }

                pokreniIzListe();
            }
        }

        private void Prethodna_Click(object sender, EventArgs e)
        {
            PrethodnaPjesma();
        }

        private void PrethodnaPjesma()
        {
            if (lista.Items.Count > 0)
            {
                if (!repeat_ukljucen)
                {
                    int i;
                    if (shuffle_ukljucen)
                    {
                        Random slucajni = new Random();
                        do
                        {
                            i = slucajni.Next(0, lista.Items.Count);
                        } while (i == indeksPjesme && lista.Items.Count > 1);
                    }
                    else
                    {
                        i = indeksPjesme - 1;
                    }

                    if (i >= 0)
                        lista.SelectedIndex = i;
                    else
                        lista.SelectedIndex = lista.Items.Count - 1;
                }
                else
                {
                    lista.SelectedIndex = indeksPjesme;
                }

                pokreniIzListe();
            }
        }


        //Otvaranje liste
        private void button12_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog otvoriListu = new
           System.Windows.Forms.OpenFileDialog();

            otvoriListu.Filter = "Text files (*.txt)|*.txt";
            ZaustaviReprodukciju();

            if (otvoriListu.ShowDialog() == DialogResult.OK)
            {
                StreamReader myStream = new StreamReader(otvoriListu.OpenFile());
                ocistiListu();
                
                while (!myStream.EndOfStream)
                {
                    playlista.Add(myStream.ReadLine());
                }
                myStream.Close();

                string[] putanje = new string[playlista.Count];
                for (int i = 0; i < playlista.Count; i++)
                    putanje[i] = playlista[i];

                playlista.Clear();
                dodajListu(putanje);

                if (playlista.Count > 0)
                {
                    lista.SelectedIndex = 0;
                    pokreniIzListe();
                }

                xPozicija = MousePosition.X;
                yPozicija = MousePosition.Y;
                panel4_MouseEnter(sender, e); //Zatvaranje pokretnog menija
            }

        }

        private void dodajListu(string[] pjesma)
        {
            for (int i = 0; i < pjesma.Length; i++)
            {
                lista.Items.Add(vratiIme(pjesma[i]));
                playlista.Add(pjesma[i]);
            }

            if (lista.Items.Count > 6)
                lista.Height = 91;
            else
                lista.Height = (lista.Items.Count + 1) * 13;
        }

        //Podešavanje kursora za poziciju pjesme, klikom na crni panel
        private void panel2_Click(object sender, EventArgs e)
        {
            int x;

            if (Form1.ActiveForm.FormBorderStyle == FormBorderStyle.Fixed3D)
                x = 5;
            else
                x = 0;

            kursor.Location = new Point(MousePosition.X - Form1.ActiveForm.Location.X - x, kursor.Location.Y);


            if (currentlyPlayingSound != null)
            {
                int pozicija = ((kursor.Location.X - 32) * (int)currentlyPlayingSound.PlayLength) / 500;
                currentlyPlayingSound.PlayPosition = (uint)pozicija;
                vrijeme = pozicija;
            }
        }

        // -----------------------------------------------------------------------------
        // -----------------------------------------------------------------------------
        // -----------------------------------------------------------------------------
        // -----------------------------------------------------------------------------
        /*
         * Upotpunosti
         * završene
         * metode
         */ 
        // -----------------------------------------------------------------------------
        // -----------------------------------------------------------------------------
        // -----------------------------------------------------------------------------
        // -----------------------------------------------------------------------------

        // Ubrzavanje reprodukcije
        private void button2_Click(object sender, EventArgs e)
        {
            if (currentlyPlayingSound != null)
            {
                currentlyPlayingSound.PlaybackSpeed += 0.1F;                
                brzina.Text = "Brzina: " + Math.Round(currentlyPlayingSound.PlaybackSpeed, 2).ToString();
                interval += 50;
            }
        }

        //Usporavanje reprodukcije
        private void button8_Click(object sender, EventArgs e)
        {
            if (currentlyPlayingSound != null)
            {
                currentlyPlayingSound.PlaybackSpeed -= 0.1F;
                brzina.Text = "Brzina: " + Math.Round(currentlyPlayingSound.PlaybackSpeed, 2).ToString();
                interval -= 50;
            }
        }

        //Metoda koja postavlja jaèinu zvuka
        private void jacinaZvuka()
        {
            if (currentlyPlayingSound != null)
                currentlyPlayingSound.Volume = (190 - kursor2.Location.Y) / 50.0f;
        }

        //Pomjeranje kursora za zvuk klikom na crni panel
        private void panel3_Click(object sender, EventArgs e)
        {
            kursor2.Location = new Point(kursor2.Location.X, MousePosition.Y - Form1.ActiveForm.Location.Y);
            jacinaZvuka();
        }


        //Metoda za pomjeranje kursora za jaèinu zvuka
        private void panel4_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            if (MousePosition.Y - Form1.ActiveForm.Location.Y < (140)) kursor2.Location = new Point(kursor2.Location.X, 140);
            else if (MousePosition.Y - Form1.ActiveForm.Location.Y > (190)) kursor2.Location = new Point(kursor2.Location.X, 190);
            else kursor2.Location = new Point(kursor2.Location.X, MousePosition.Y - Form1.ActiveForm.Location.Y);

            jacinaZvuka();
        }

        //Otkucaj sistemskog sata
        private void timer_Tick(object sender, EventArgs e)
        {
            vrijeme += interval;            
            if (currentlyPlayingSound != null)
            {
                if (currentlyPlayingSound.Finished)
                {
                    resetTimer();
                    if ((indeksPjesme + 1 < lista.Items.Count) || repeat_ukljucen)
                        SlijedecaPjesma();
                }
                else
                    Vrijeme.Text = "Vrijeme: " + vratiVrijeme(vrijeme);
            }
            else
                Vrijeme.Text = "Vrijeme: " + vratiVrijeme(vrijeme);
        }

        //Dugme Minimize
        private void button4_Click(object sender, EventArgs e)
        {
            Form1.ActiveForm.WindowState = FormWindowState.Minimized;
        }

        //Opcija Zatvori iz Menija
        private void zatvoriToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        //Otvaranje pokretnog menija
        private void panel4_MouseEnter(object sender, EventArgs e)
        {
            povuciMeni();
        }

        private void povuciMeni()
        {
            Application.DoEvents();

            if (opcije.Left == -130)
            {
                opcije.Left = 0;
                strelica.Text = "<";
            }
            else
            {
                opcije.Left = -130;
                strelica.Text = ">";
            }
        }

        //Ikona za X
        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Zatvaranje forme
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (currentlyPlayingSound != null)
                currentlyPlayingSound.Stop();
        }

        //Funkcija koja vraca string vrijeme u obliku 00:00:00
        private string vratiVrijeme(double vrijeme)
        {
            string v = "";
            int h, m, s;

            if (vrijeme >= 3600000)
            {
                h = (int)vrijeme / 3600000;
                v = h.ToString() + ":";
            }

            m = ((int)vrijeme % 3600000) / 60000;
            if (m < 10) v += "0";
            v += m.ToString() + ":";

            s = (((int)vrijeme % 3600000) % 60000) / 1000;
            if (s < 10) v += "0";
            v += s.ToString();

            if (currentlyPlayingSound != null)
                Pomjeri((int)(500 * vrijeme) / (int)currentlyPlayingSound.PlayLength);

            return v;
        }


        //Funkcija za èišæenje playliste
        private void ocistiListu()
        {
            playlista.Clear();
            lista.Items.Clear();
        }

        //Funkcija koja od putanje vraæa naziv datoteke
        public string vratiIme(string putanja)
        {
            if (putanja == null) return "";
            
            int pocetnaPozicija = -1, krajnjaPozicija = putanja.Length;

            //Trazi se pozicija posljednjeg znaka \
            for (int i = 0; i < putanja.Length; i++)
                if (putanja[i].ToString() == "\\")
                    pocetnaPozicija = i;

            //Trazi se pozicija posljednje taèke
            int k = putanja.Length - 1;
            while (k > pocetnaPozicija && putanja[k].ToString() != ".")
                k--;

            krajnjaPozicija = k;

            //Kopiranje dijela putanje od znaka "\" do znaka "." 
            //što predstavlja naziv datoteke
            string ime = "";
            for (int j = pocetnaPozicija + 1; j < krajnjaPozicija; j++)
                ime += putanja[j];

           
            
            return ime;
        }

        private string vratiFormat (string putanja)
        {
            //Trazi se pozicija posljednje taèke
            int k = putanja.Length - 1;
            while (k > 0 && putanja[k].ToString() != ".")
                k--;
            
            //Postavljanje formata pjesme
            string format = "";
            for (int i = k + 1; i < putanja.Length; i++)
                format += putanja[i];

            return format;
        }

        //Spašavanje liste
        private void button11_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.SaveFileDialog spasiListu = new
           System.Windows.Forms.SaveFileDialog();

            spasiListu.Filter = "Text files (*.txt)|*.txt";

            if (spasiListu.ShowDialog() == DialogResult.OK)
            {
                StreamWriter myStream = new StreamWriter(spasiListu.OpenFile());
                for (int i = 0; i < lista.Items.Count; i++)
                {
                    myStream.WriteLine(playlista[i].ToString());
                }
                myStream.Close();
            }

            xPozicija = MousePosition.X;
            yPozicija = MousePosition.Y;
            panel4_MouseEnter(sender, e);
        }

        //Prikaz liste
        private void button9_Click(object sender, EventArgs e)
        {
            if (Form1.ActiveForm.Height < 250)
            {
                Form1.ActiveForm.Height += 105;
                strelica.Height += 105;
                opcije.Height += 105;
            }
            else
            {
                Form1.ActiveForm.Height -= 105;
                strelica.Height -= 105;
                opcije.Height -= 105;
            }
        }

        //Pomjeranje kursora
        private void Pomjeri(int pomjeri)
        {            
            kursor.Location = new Point(32 + pomjeri, kursor.Location.Y);
        }

        // -----------------------------------------------------------------------------
        // -----------------------------------------------------------------------------
        // -----------------------------------------------------------------------------
        // -----------------------------------------------------------------------------
        /*
         * Nezavršene / Neimplementirane
         * metode
         */
        // -----------------------------------------------------------------------------
        // -----------------------------------------------------------------------------
        // -----------------------------------------------------------------------------
        // -----------------------------------------------------------------------------

        //Dugme Saèuvaj
        private void sacuvaj_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "WAV (*.wav)|*.wav";
            
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                
                System.IO.FileInfo FileInfo1 = new System.IO.FileInfo(@saveFile.FileName); 
                using (FileStream fs = FileInfo1.Create()) {}
                string ime = saveFile.FileName;
                      
                string shortPath = this.shortPathName(ime);
                
                FileInfo1.Delete();
                string formatShortPath = string.Format("save recsound \"{0}\"", shortPath);
                mciSendString(string.Format("{0}", formatShortPath), "", 0, 0);
                mciSendString("close recsound ", "", 0, 0);               
            }
        }
                        
        private string shortPathName(string putanja)
        {   
            string shortPath = string.Empty;
            long length = 0;
            StringBuilder buffer = new StringBuilder(256);
                        
            length = GetShortPathName(putanja, buffer, 256);
            shortPath = buffer.ToString();

            return shortPath;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetShortPathName(
            [MarshalAs(UnmanagedType.LPTStr)] 
            string longPath,
            [MarshalAs(UnmanagedType.LPTStr)] 
            StringBuilder shortPath,
            int length);
        
        //Dugme Konvertuj
        private void konvertuj_Click(object sender, EventArgs e)
        {
            panel4_MouseEnter(sender, e);
            ConvertOpcije con = new ConvertOpcije();
            con.roditelj = this;
            con.Show(); 
        }
                               
        //Zaustavljanje reprodukcije
        private void button3_Click(object sender, EventArgs e)
        {
            ZaustaviReprodukciju();
        }
               

        /*
         * 
         * 
         * Nove metode
         * 
         * 
         * 
         * 
         * 
         * 
         */
        
        //Oèistiti listu
        private void clear_Click(object sender, EventArgs e)
        {
            if (currentlyPlayingSound != null)
                ZaustaviReprodukciju();

            ocistiListu();
            Pjesma.Text = "";
            format.Text = "Format: ";
        }

        //Ukljuèivanje shuffle
        private void shuffle_Click(object sender, EventArgs e)
        {
            if (shuffle.BackColor == Color.Firebrick)
            {
                shuffle.BackColor = Color.Maroon;
                shuffle_ukljucen = true;
            }
            else
            {
                shuffle.BackColor = Color.Firebrick;
                shuffle_ukljucen = false;
            }
        }

        private void repeat_Click(object sender, EventArgs e)
        {
            if (repeat.BackColor == Color.Firebrick)
            {
                repeat.BackColor = Color.Maroon;
                repeat_ukljucen = true;
            }
            else
            {
                repeat.BackColor = Color.Firebrick;
                repeat_ukljucen = false;
            }
        }

        private void lista_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void kursor_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            int x;
            if (Form1.ActiveForm.FormBorderStyle == FormBorderStyle.Fixed3D)
                x = 5;
            else
                x = 0;

            if (MousePosition.X - Form1.ActiveForm.Location.X < (32 + x)) kursor.Location = new Point(32, kursor.Location.Y);
            else if (MousePosition.X - Form1.ActiveForm.Location.X > (532 + x)) kursor.Location = new Point(532, kursor.Location.Y);
            else kursor.Location = new Point(MousePosition.X - Form1.ActiveForm.Location.X - x, kursor.Location.Y);

            
        }

        private void kursor_MouseUp(object sender, MouseEventArgs e)
        {
            if (currentlyPlayingSound != null)
            {
                int pozicija = ((kursor.Location.X - 32) * (int)currentlyPlayingSound.PlayLength) / 500;
                currentlyPlayingSound.PlayPosition = (uint)pozicija;
                vrijeme = pozicija;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            repr_Click(sender, e);
            panel4_MouseEnter(sender, e);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
        
            if (e.Button != MouseButtons.Left)
            {
                //Racuna poziciju miša prije klika
                xPozicija = MousePosition.X; 
                yPozicija = MousePosition.Y;
                //Racuna poziciju prozora prije klika
                xPozicijaP = this.Location.X;
                yPozicijaP = this.Location.Y;
            }
            else
            {
                //Racuna nove koordinate X i Y tako sto na vrijednost posljednje pozicije
                //prozora (prije klika) dodaje razliku izmedju trenutne pozicije miša
                //i posljednje pozicije miša (prije klika)
                int noviX = xPozicijaP + MousePosition.X - xPozicija;
                int noviY = yPozicijaP + MousePosition.Y - yPozicija;
                this.Location = new Point(noviX, noviY);
            }
        
        }

        //Prebacivanje u mod "Snimanje"
        private void button13_Click(object sender, EventArgs e)
        {
            ZaustaviReprodukciju(); //Zaustavljanje reprodukcije
            clear_Click(sender, e); //Èišæenje liste
            postaviDugme(false);
            panel4_MouseEnter(sender, e); //povlaèenje pokretnog menija
            currentlyPlayingSound = null;
        }

        //Prebacivanje u mod "Reprodukcija"
        private void repr_Click(object sender, EventArgs e)
        {
            postaviDugme(true);
            panel4_MouseEnter(sender, e); //povlaèenje pokretnog menija
            resetTimer();
        }

        private void postaviDugme(bool omoguci)
        {
            play.Visible = omoguci;
            pauza.Visible = false;
            stop.Visible = omoguci;
            Slijedeca.Visible = omoguci;
            Prethodna.Visible = omoguci;
            fast.Visible = omoguci;
            slow.Visible = omoguci;

            recStart.Visible = !omoguci;
            recStop.Visible = !omoguci;

            otvori.Enabled = omoguci;
            dodajuListu.Enabled = omoguci;
            otvoriListu.Enabled = omoguci;
            sacuvajListu.Enabled = omoguci;
            konvertuj.Enabled = omoguci;
            sacuvaj.Enabled = false;

            repr.Enabled = !omoguci;
            snimanje.Enabled = omoguci;
        }

        private void recStart_Click(object sender, EventArgs e)
        {
            mciSendString("close recsound ", "", 0, 0);
            mciSendString("open new Type waveaudio Alias recsound", "", 0, 0);
            mciSendString("record recsound", "", 0, 0);
            vrijeme = 0;
            interval = 500;
            timer.Start();
            recStart.Enabled = false;
            recStop.Enabled = true;
            sacuvaj.Enabled = false;
        }

        private void recStop_Click(object sender, EventArgs e)
        {            
            timer.Stop();
            vrijeme = 0;
            mciSendString("stop recsound", "", 0, 0);            
            recStart.Enabled = true;
            recStop.Enabled = false;
            sacuvaj.Enabled = true;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (opcije.Location.X == 0)
                povuciMeni();
        }
    }
}