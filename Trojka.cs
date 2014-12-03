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
        public byte[] kod;


        public byte[] ToBytes()
        {
            byte[] bytes = new byte[4];
            bytes[0] = (byte) znak;
            bytes[1] = dlugosc;
            bytes[2] = kod[0];
            bytes[3] = kod[1];
            
            return bytes;
        }
    }
}
