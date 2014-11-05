using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace TIiK
{


  public  class Znak
    {
        public string Wartosc;

        private double entropia;
        public double Entropia
        {
            set { entropia = value; }
            get
            {
                return Math.Abs(entropia);
            }

        }
        public int Wystapien;
        public double Prawdopodobienstwo;

        public Znak(string wartosc)
        {
            Wartosc = wartosc;
            entropia = 0;
            Wystapien = 1;
            Prawdopodobienstwo = 0;

        }


        public override string ToString()
        {
            return string.Format("{0}\t{1}\t{2}\t{3}", Wartosc, Wystapien, Math.Round(Entropia,2), Math.Ceiling(Entropia));
        }
    }

  public  class Statystyka
    {
        public Dictionary<string, Znak> slownik;
        public double Entropia = 0;

        public Statystyka()
        {
            slownik = new Dictionary<string, Znak>();

        }


        public void ZbudujSlownik(string tekst = "")
        {
            foreach (var znak in tekst)
            {
                string sZnak = znak.ToString();
                // Slownik nie zawiera
                if (!slownik.Any(x => x.Key == sZnak))
                {
                    slownik.Add(sZnak, new Znak(sZnak));
                }
                else
                {
                    slownik[sZnak].Wystapien++;
                }

            }

        }


        public void UaktualnijPrawdopodobienstwo(int podstawa = 2)
        {

            foreach (var znak in slownik)
            {
                znak.Value.Prawdopodobienstwo = (znak.Value.Wystapien / LiczbaZnakow());
                znak.Value.Entropia = znak.Value.Prawdopodobienstwo * Math.Log(znak.Value.Prawdopodobienstwo, podstawa);
                Entropia += znak.Value.Entropia;
            }
        }

        public void UaktualnijEntropie(int podstawa = 2)
        {

            foreach (var znak in slownik)
            {
                znak.Value.Entropia = znak.Value.Prawdopodobienstwo * Math.Log(znak.Value.Prawdopodobienstwo, podstawa);
            }
        }

        public void ObliczEntropie()
        {
            Entropia = 0;
            foreach (var znak in slownik)
            {
                Entropia += znak.Value.Entropia;
            }
        }


        public void OliczWszystko(int podstawa = 2)
        {
            Entropia = 0;
            foreach (var znak in slownik)
            {
                znak.Value.Prawdopodobienstwo = (znak.Value.Wystapien / (double)LiczbaZnakow());
                znak.Value.Entropia = Math.Log(1/znak.Value.Prawdopodobienstwo, podstawa);
                Entropia += znak.Value.Prawdopodobienstwo * znak.Value.Entropia;
            }
        }
        public int LiczbaZnakow() {
            int liczba = 0;
            foreach (var item in slownik)
            {
                liczba += item.Value.Wystapien;
            }
            return liczba;
        }

        public double Srednia() { 
            double srednia = 0;

            foreach (var znak in slownik){
                srednia += Math.Ceiling(znak.Value.Entropia);
            }
            return srednia / slownik.Count();
        }


        public int NaIluZnakach() { 
            double suma = 0;
            foreach (var znak in slownik) {
                suma += Math.Ceiling(znak.Value.Entropia) * znak.Value.Wystapien;
            }

            return (int)suma;
        }
    }
}
