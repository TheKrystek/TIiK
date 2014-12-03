using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TIiK
{
    [Serializable]
    public class Header
    {
        public UInt16 liczbaTrojek;
        public Trojka[] trojki;

        public byte[] ToBytes()
        {
            byte[] bytes = new byte[4*liczbaTrojek + 2];


            byte[] tmp = new byte[2];
            tmp = BitConverter.GetBytes(liczbaTrojek);
            bytes[0] = tmp[0];
            bytes[1] = tmp[1];

            for (int i = 0; i < liczbaTrojek; i++)
            {
                int pos = 4*i + 2;
                byte[] t = trojki[i].ToBytes();
                bytes[pos] = t[0];
                bytes[pos+1] = t[1];
                bytes[pos+2] = t[2];
                bytes[pos+3] = t[3];
            }
            return bytes;
        }
    }
}
