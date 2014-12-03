using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace TIiK
{
    public partial class Form1 : Form
    {
        private Statystyka s;
        public Form1()
        {
            InitializeComponent();
        }

        private bool nazwa = true;
        private bool wystapien = true;
        private bool entropia = true;



        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            if (!string.IsNullOrWhiteSpace(path.Text))
                openFileDialog.InitialDirectory = path.Text;

            DialogResult userClickedOK = openFileDialog.ShowDialog();
            if (userClickedOK == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName.ToString();
                if (File.Exists(filePath))
                {
                    string text = File.ReadAllText(filePath);
                    input.Text = text;

                }
            }
        }

        public void ObliczEntropie(string text)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            s = new Statystyka();
            s.ZbudujSlownik(text);
            s.OliczWszystko();
            watch.Stop();


            toolStripStatusLabel1.Text = string.Format("Czas obliczeń: {0}", watch.Elapsed);
            results.Text = "";
            results.Text = string.Format("Entropia: {0}\r\nRozmiar w bajtach: {1}\r\nWiadomość zakodowana na bitach: {2}\r\nŚrednia długość kodu: {3}\r\nTeoretyczny minimalny rozmiar pliku po kompresji: {4}B\r\n",
                s.Entropia, s.LiczbaZnakow(), s.NaIluZnakach(), s.Srednia(), s.TeoretycznyMinimalnyRozmiarplikupokompresji());
            wyswietlWyniki(s);

        }

        private void oblicz_Click(object sender, EventArgs e)
        {
            ObliczEntropie(input.Text);

            ShannonFano.Kompresuj(s.slownik.ToList());
            sf.Text = "";
            foreach (var sl in s.slownik)
            {
                sf.Text += string.Format("{0}\r\n", sl.Value.Kod);
            }
            results.Text += string.Format("Średnia długość kodu przy kowaniu Shannona-Fano: {0}\r\nEfektywność kodowania Shannona-Fano: {1}%", s.SredniaShannonaFano(), s.EfektywnoscKodowania());
        }


        public void wyswietlWyniki(Statystyka s, bool nazwa = true, bool wystapien = false, bool entropia = false)
        {
            output.Items.Clear();
            if (s != null)
            {
                if (nazwa)
                {

                    if (this.nazwa)
                    {
                        foreach (var item in s.slownik.OrderBy(x => x.Value.Wartosc))
                        {
                            output.Items.Add(item.Value);
                            Debug.Print(item.Value.ToString());
                        }
                    }
                    else
                    {
                        foreach (var item in s.slownik.OrderByDescending(x => x.Value.Wartosc))
                        {
                            output.Items.Add(item.Value);
                            Debug.Print(item.Value.ToString());
                        }
                    }
                    this.nazwa = !this.nazwa;
                }


                if (entropia)
                {

                    if (this.entropia)
                    {
                        foreach (var item in s.slownik.OrderBy(x => x.Value.Entropia))
                        {
                            output.Items.Add(item.Value);
                            Debug.Print(item.Value.ToString());
                        }
                    }
                    else
                    {
                        foreach (var item in s.slownik.OrderByDescending(x => x.Value.Entropia))
                        {
                            output.Items.Add(item.Value);
                            Debug.Print(item.Value.ToString());
                        }
                    }
                    this.entropia = !this.entropia;
                }


                if (wystapien)
                {

                    if (this.wystapien)
                    {
                        foreach (var item in s.slownik.OrderBy(x => x.Value.Wystapien))
                        {
                            output.Items.Add(item.Value);
                            Debug.Print(item.Value.ToString());
                        }
                    }
                    else
                    {
                        foreach (var item in s.slownik.OrderByDescending(x => x.Value.Wystapien))
                        {
                            output.Items.Add(item.Value);
                            Debug.Print(item.Value.ToString());
                        }
                    }
                    this.wystapien = !this.wystapien;
                }

            }


        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            wyswietlWyniki(s, true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            wyswietlWyniki(s, false, true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            wyswietlWyniki(s, false, false, true);
        }






        private string ZakodujWiadomosc()
        {

            string ret = "";
            foreach (var znak in input.Text)
            {
                ret += s.slownik[znak.ToString()].Kod;
            }
            return ret;
        }

        private Header konstrujNaglowek()
        {
            Header h = new Header();
            h.liczbaTrojek = (ushort)s.slownik.Count();
            h.trojki = new Trojka[h.liczbaTrojek];
            int i = 0;
            foreach (KeyValuePair<string, Znak> symbol in s.slownik)
            {
                h.trojki[i] = new Trojka();
                h.trojki[i].znak = symbol.Key[0];
                h.trojki[i].dlugosc = (byte)symbol.Value.Kod.Length;
                h.trojki[i].kod = StringToByte.OnTwoBytes(symbol.Value.Kod, symbol.Value.Kod.Length);
                i++;
            }

            return h;
        }

        private void zapiszDoPliku(string path = @"tmp.sfc")
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
            {
                Header h = konstrujNaglowek();
                byte[] headerBytes = h.ToBytes();
                writer.Write(headerBytes);
                string zakodowane = ZakodujWiadomosc();
                sfo.Text = zakodowane;
                byte[] toWrite = StringToByte.ToByte(zakodowane);
                writer.Write(toWrite);
            }

        }





        private void wczytajZPliku(string path = @"tmp.sfc")
        {
            Header h = new Header();
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            // wczytaj 2 bajty
            byte[] buffor = new byte[2];
            stream.Read(buffor, 0, 2);
            UInt16 trojek = BitConverter.ToUInt16(buffor, 0);

            h.liczbaTrojek = trojek;
            h.trojki = new Trojka[trojek];


            // wczytaj rozmiar liczbe trojek razy 4
            buffor = new byte[4 * trojek];
            stream.Read(buffor, 0, 4*trojek);
            for (int i = 0; i < trojek; i++)
            {
                int poz = 4*i;
                Trojka t = new Trojka();
                t.znak = (char) buffor[poz];
                t.dlugosc = buffor[poz + 1];
                t.kod = new byte[2];
                t.kod[0] = buffor[poz + 2];
                t.kod[1] = buffor[poz + 3];
                h.trojki[i] = t;
            }



            int pozostalo = (int)(stream.Length - stream.Position);
            byte[] bytes = new byte[pozostalo];
            stream.Read(bytes, 0, pozostalo);
            stream.Close();

            
            int minSize = -1;
            foreach (Trojka t in h.trojki)
            {
                if (t.dlugosc < minSize || minSize == -1)
                {
                    minSize = t.dlugosc;
                }
            }

            sfo.Text += Environment.NewLine;
            string zakodowaneBin = "";
            zakodowaneBin = ByteToString.ToString(bytes);
            sfo.Text += zakodowaneBin;


            Dictionary<char, Znak> znakDictionary = new Dictionary<char, Znak>();
            foreach (Trojka t in h.trojki)
            {
                Znak z = new Znak(t.znak.ToString());
                //todo
                z.Kod = ByteToString.ToString(t.kod,t.dlugosc);
                z.DlugoscKodu = t.dlugosc;
                znakDictionary.Add(t.znak, z);
            }

            string odkodowane = "";
            int pos = 0;
            while (pos < zakodowaneBin.Length)
            {
                int winSize = minSize;
                while (true)
                {
                    string kod = zakodowaneBin.Substring(pos, winSize);

                    bool found = false;
                    char zkod = '\0';
                    foreach (KeyValuePair<char, Znak> pair in znakDictionary)
                    {
                        if (pair.Value.Kod == kod)
                        {
                            zkod = pair.Key;
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        odkodowane += zkod;
                        pos += kod.Length;
                        break;
                    }
                    else
                    {
                        winSize++;
                    }
                }



            }
        
           sfo.Text += odkodowane;
  
 }

        private void button5_Click(object sender, EventArgs e)
        {
            byte inp = 0xAD;
            byte result = BinaryStream.Read(inp, 4, 4);
            //Debug.Print("{0}",result);

            string str = "00000000000000000000000000000001010101010101101010101010110110110110110110111111111111111";
            byte[] b = StringToByte.ToByte(sfo.Text);
            foreach (byte b1 in b)
            {
                Debug.Print("{0}", b1);
            }

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            zapiszDoPliku();
            wczytajZPliku();
        }


    }
}
