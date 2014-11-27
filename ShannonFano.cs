using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIiK
{
    static class ShannonFano
    {


        public static void Kompresuj(List<KeyValuePair<string, Znak>> input)
        {
            ZnajdzMinimalnaRoznice(input, '0' ,1);
        }



        public static void ZnajdzMinimalnaRoznice(List<KeyValuePair<string, Znak>> input, char znak, double pr)
        {
            // Jezeli lista ma jeden element to dopisz znak i wyjdz
            if (input.Count < 2)
            {
                return;
            }

            // Posortuj liste wg prawdopodobienstwa
            //var posortowane = input.OrderByDescending(x => x.Value.Prawdopodobienstwo);

            double sum_down = 0;
            double sum_up = pr;
            double min_value = 2;
            double min_pr = pr;//input.Sum(x => x.Value.Prawdopodobienstwo);
            int index = 0;

            int i = 0;
            foreach (KeyValuePair<string, Znak> pair in input)
            {
                sum_down += pair.Value.Prawdopodobienstwo;
                sum_up -= pair.Value.Prawdopodobienstwo;
                double diff = Math.Abs(sum_down - sum_up);
                if (diff < min_value)
                {
                    index = i;
                    min_value = diff;
                    min_pr = sum_up;
                    Debug.Print("min: " + min_value.ToString());
                }
                i++;
            }


            List<KeyValuePair<string, Znak>> lower = new List<KeyValuePair<string, Znak>>();
            List<KeyValuePair<string, Znak>> upper = new List<KeyValuePair<string, Znak>>();
            i = 0;
            foreach (KeyValuePair<string, Znak> pair in input)
            {
                if (i <= index)
                {
                    lower.Add(pair);
                }
                else
                {
                    upper.Add(pair);
                }
                i++;
            }

            if (lower.Count == 1) {
                lower.First().Value.Kod += znak;
                foreach (KeyValuePair<string, Znak> up in upper)
                {
                    up.Value.Kod += (znak == '1' ? '0' : '1');
                }
            }

            ZnajdzMinimalnaRoznice(lower,'1',min_pr);
            ZnajdzMinimalnaRoznice(upper,'0',min_pr);

        }
    }
}
