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
            int size = (int)Math.Ceiling(((double)str.Length / 8));
            byte[] bytes = new byte[size];
            int index = 0;
            for (int i = 0; i < l; i++)
            {
                if (str[i] != '0')
                {
                    r += (byte)(1 << (8 - (i % 8) - 1));
                }

                if (i % 8 == 7 || i==l-1)
                {
                    bytes[index] = r;
                    r = 0;
                    index++;
                }
            }
            return bytes;
        }

        public static byte[] OnTwoBytes(string str,int dl=0)
        {
            byte[] result = new byte[2];
            byte[] bytes = ToByte(str);
            result[0] = bytes[0];
            if (bytes.Length == 1)
                result[1] = 0x00;
            else
                result[1] = bytes[1];

            if (dl != 0)
                result[0] = BinaryStream.Read(result[0], 0, dl);

            return result;
        }

    }
}
