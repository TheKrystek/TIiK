using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TIiK
{
    [Serializable]
    public class Trojka
    {
        public char znak;
        public byte dlugosc;
        public UInt16 kod;


        public byte[] ToBytes()
        {
            byte[] bytes = new byte[4];
            bytes[0] = (byte) znak;
            bytes[1] = dlugosc;
            
            byte[] tmp = new byte[2];
            tmp = BitConverter.GetBytes(kod);
            bytes[2] = tmp[0];
            bytes[3] = tmp[1];
            
            return bytes;
        }
    }
}
