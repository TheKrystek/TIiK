using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIiK
{
   static class StringToByte
   {

       static public byte[] ToByte(string str)
       {
           byte r = 0;
           int l = str.Length;
           int size = (int) Math.Ceiling((double) (str.Length/8));
           byte[] bytes = new byte[size];
           int index = 0;
           for (int i = 0; i < l; i++)
           {
               if (str[i] != '0')
               {
                   r += (byte) (1 << (8 - (i%8) - 1));
               }

               if (i%8 == 7)
               {
                   bytes[index] = r;
                   r = 0;
                   index++;
               }
           }
           return bytes;
       }

      
   }
}
