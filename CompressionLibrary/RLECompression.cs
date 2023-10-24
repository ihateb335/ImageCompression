using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CompressionLibrary.RLE;

namespace CompressionLibrary
{
    public class RLECompression : DataCompression
    {
        public RLECompression(): base(new RLECompressor(), new RLEDecompressor()) { }
    }
}
