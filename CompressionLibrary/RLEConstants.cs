using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibrary
{
    public static class RLEConstants
    {
        public static readonly byte RLE_FLAG = 0x80;
        public static byte RLEFlaggedByte(this int value, bool differencing)
        {
            byte result = value == 0 ? (byte)0 : (byte)(value  - 1);
            if (!differencing) result += RLE_FLAG; //IF SAME +Flag
            return result;
        }
        public static int RLECount(this int value)
        {
            if (value > 127) return value - RLE_FLAG + 1;
            return value + 1;
        }
    }
}
