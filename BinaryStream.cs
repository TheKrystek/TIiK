using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIiK
{
    static class BinaryStream
    {
        public static byte Read(byte input, int offset, int count)
        {
            int paddingRight = 8 - offset - count;

            byte mask = 0xFF;
            
            mask >>= offset;
            mask <<= paddingRight;

            byte result = (byte) (input & mask);
            return (byte) (result >> paddingRight);

        }

    }
}
