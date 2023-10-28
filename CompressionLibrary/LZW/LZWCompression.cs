using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibrary.LZW
{
    public class LZWCompression : DataCompression
    {

        public LZWCompression(ushort Limit = 65535) : base(new LZWCompressor(Limit), new LZWDecompressor(Limit))
        {
           
        }
    }
}
