using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibrary.Huffman
{
    public class HuffmanNodeData 
    {
        public ulong Count { get; set; }
        public byte Byte { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as HuffmanNodeData;

            return Count == other.Count && Byte == other.Byte;
        }
    }
}
