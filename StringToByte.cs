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
           uint r = 0;
           int l = str.Length;
           for (int i = 0; i < l; i++)
           {
               if (str[i]=='0') continue;
               r += (uint)(1 << (l - i - 1));
           }
           return BitConverter.GetBytes(r);
       }

      
   }
}
