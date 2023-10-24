using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CompressionLibrary.RLEI;

namespace CompressionLibrary
{
    public class RLEICompression : DataCompression
    {
        public RLEICompression(int N = 3): base(new RLEICompressor(N), new RLEIDecompressor(N) ) { } 
    }
}
