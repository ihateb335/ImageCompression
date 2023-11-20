using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CompressionLibrary.BitOperations;

namespace CompressionLibrary.Huffman
{
    public class HuffmanCompressor : DataCompressor
    {
        protected override void Algorithm(Stream InputFile, Stream OutputFile)
        {
            var Tree = new HuffmanTree(InputFile);
            Tree.WriteTree(OutputFile);

            var reader = new BinaryReader(InputFile);
            var writer = new BitWriter(OutputFile);

            uint code;

            if (Tree.TreeRoot.LeftNode == null && Tree.TreeRoot.RightNode == null) return;

            while (InputFile.Position != InputFile.Length)
            {
                foreach (var item in Tree.GetCode(reader.ReadByte()))
                {
                    writer.Write8(item == '0' ? (byte) 0 : (byte)1, 1);
                }
            }
            writer.Flush();
        }
    }
}
