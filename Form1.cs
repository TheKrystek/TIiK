using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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
                    ObliczEntropie(text);
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

            
            toolStripStatusLabel1.Text = string.Format("Entropia: {0}, czas obliczenia: {1}", s.Entropia.ToString(), watch.Elapsed.ToString());
            wyswietlWyniki(s);

        }

        private void oblicz_Click(object sender, EventArgs e)
        {
            ObliczEntropie(input.Text);
        }

    
        public void wyswietlWyniki(Statystyka s, bool nazwa = true, bool wystapien = false, bool entropia = false)
        {
            output.Items.Clear();
            if (s !=null)
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
            wyswietlWyniki(s,true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            wyswietlWyniki(s,false,true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            wyswietlWyniki(s,false,false,true);
        }

    }
}
