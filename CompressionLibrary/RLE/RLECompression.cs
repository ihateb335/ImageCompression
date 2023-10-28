using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibrary.RLE
{
    public class RLECompression : DataCompression
    {
        public RLECompression(): base(new RLECompressor(), new RLEDecompressor()) { }
    }
}
