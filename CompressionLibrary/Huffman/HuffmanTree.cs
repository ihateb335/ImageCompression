using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Priority_Queue;

namespace CompressionLibrary.Huffman
{
    using HuffmanTreeNode = BinaryTreeNode<HuffmanNodeData>;
    public class HuffmanTree
    {
        private Stream InputFileStream;
        internal static readonly int BYTES = 256;

        public HuffmanTree(Stream stream, bool dataFromStream = true)
        {
            InputFileStream = stream;
            base2Codes = new string[BYTES].Select(x => "").ToArray();
            if (dataFromStream) AvailableNodes = CountBytes();
            else AvailableNodes = BytesFromHeader();

            BuildTree(AvailableNodes);
        }

        private string[] base2Codes;
        public HuffmanTreeNode TreeRoot { get; private set; }
        private IEnumerable<HuffmanNodeData> AvailableNodes;
        public string GetCode(byte code) => base2Codes[code];

        /// <summary>
        /// Count bytes in the input stream. Returns stream to the beginning.
        /// </summary>
        /// <returns>Bytes with their frequency</returns>
        public IEnumerable<HuffmanNodeData> CountBytes()
        {
            var ByteCount = new ulong[BYTES];
            var reader = new BinaryReader(InputFileStream);

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var readed = reader.ReadByte();
                ByteCount[readed]++;
            }

            InputFileStream.Seek(0, SeekOrigin.Begin);
            return ByteCount.Select((x, i) => new HuffmanNodeData { Byte = (byte)i, Count = x}).Where(x => x.Count > 0);
        }

        public long CodeCount { get; private set; }

        public IEnumerable<HuffmanNodeData> BytesFromHeader()
        {
            var reader = new BinaryReader(InputFileStream);
            CodeCount = reader.ReadInt64();

            var toRead = reader.ReadByte();
            var nodes = new HuffmanNodeData[toRead];
            for (int i = 0; i < toRead; i++)
            {
                nodes[i] = new HuffmanNodeData { Byte = reader.ReadByte(), Count = reader.ReadUInt64()};
            }

            return nodes.AsEnumerable();
        }


        /// <summary>
        /// Builds tree from data
        /// </summary>
        /// <param name="data">The data for the huffman tree</param>
        private void BuildTree(IEnumerable<HuffmanNodeData> data)
        {
            var nodes = new Dictionary<byte, HuffmanTreeNode>();
            var queue = new SimplePriorityQueue<HuffmanTreeNode>();

            foreach (var item in data)
            {
                var node = new HuffmanTreeNode(item);
                nodes[item.Byte] = node;
                queue.Enqueue(node, item.Count);
            }

            for (int i = 1; i <= nodes.Count - 1; i++)
            {
                var leftNode = queue.Dequeue();
                var rightNode = queue.Dequeue();
                var node = new HuffmanTreeNode(new HuffmanNodeData { Count = leftNode.Data.Count + rightNode.Data.Count }) { LeftNode = leftNode, RightNode = rightNode};
                leftNode.ParentNode = node;
                rightNode.ParentNode = node;
                queue.Enqueue(node, node.Data.Count);
            }
            if (queue.Count > 0) TreeRoot = queue.Dequeue();

            foreach (var node in nodes)
            {
                var currentNode = node.Value;
                int currentCode = node.Key;

                while (currentNode.ParentNode != null)
                {
                    base2Codes[currentCode] = (currentNode.NodeSide == Side.Left ? "0" : "1") + base2Codes[currentCode];
                    currentNode = currentNode.ParentNode;
                }
            }
        }
        public void WriteTree(Stream output)
        {
            var writer = new BinaryWriter(output);
            writer.Write(InputFileStream.Length);
            writer.Write((byte)AvailableNodes.Count());
            foreach (var item in AvailableNodes)
            {
                writer.Write(item.Byte);
                writer.Write(item.Count);
            }
        }
    }
}
