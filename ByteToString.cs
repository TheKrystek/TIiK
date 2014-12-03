using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIiK
{
   static  class ByteToString
    {

       public static string ToString(byte[] bytes, int dl = 0)
       {
           string result = "";
           for (int i = 0; i < bytes.Length; i++)
           {
               result += Convert.ToString(bytes[i], 2);
           }
           if (dl != 0)
               result = result.Substring(0, dl);

           return result;
       }

    }
}
