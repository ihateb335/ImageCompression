using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibrary.Huffman
{
    public class HuffmanCompression : DataCompression
    {
        public HuffmanCompression() : base(new HuffmanCompressor(), new HuffmanDecompressor()) { }
    }
}
