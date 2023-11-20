using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CompressionLibrary.BitOperations;

namespace CompressionLibrary.Huffman
{
    using HuffmanTreeNode = BinaryTreeNode<HuffmanNodeData>;
    public class HuffmanDecompressor : DataCompressor
    {
        protected override void Algorithm(Stream InputFile, Stream OutputFile)
        {
            var Tree = new HuffmanTree(InputFile, false);

            long CodeRead = 0;

            var reader = new BitReader(InputFile);
            var writer = new BinaryWriter(OutputFile);

            byte bit;

            HuffmanTreeNode currentNode = Tree.TreeRoot, nextNode;

            if (currentNode == null) return;

            if (currentNode.LeftNode == null && currentNode.RightNode == null)
            {
                for (int i = 0; i < Tree.CodeCount; i++)
                {
                    writer.Write(currentNode.Data.Byte);
                }
                return;
            }

            string debug = "";

            while (CodeRead < Tree.CodeCount)
            {
                bit = reader.Read8(1);
                debug += $"{bit} ";
                if (bit == 0) nextNode = currentNode.LeftNode;
                else nextNode = currentNode.RightNode;
                System.Diagnostics.Debug.WriteLine(debug);
                if (nextNode.IsLeaf)
                {
                    debug = "";
                    writer.Write(nextNode.Data.Byte);
                    ++CodeRead;
                    currentNode = Tree.TreeRoot;
                } else
                {
                    currentNode = nextNode;
                }

            }
        }
    }
}
