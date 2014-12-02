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
        public UInt16 rozmiarPliku;
        public UInt16 liczbaTrojek;
        public Trojka[] trojki;

        public byte[] ToBytes()
        {
            byte[] bytes;
            IFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, this);
                bytes = stream.ToArray();
            }
            return bytes;
        }
    }
}
