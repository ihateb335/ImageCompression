using NUnit.Framework;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CompressionLibrary.Huffman;

namespace CompressionLibraryTesting
{
    public class HuffmanTests
    {
        [SetUp]
        public void Setup()
        {
           
        }

        HuffmanTree InputTree;
        HuffmanTree OutputTree;
        MemoryStream InputMemory;
        MemoryStream OutputMemory;

        Random random = new Random();

        [Test]
        public void TestStreamAtBeginning()
        {
            InputMemory = new MemoryStream(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            InputTree = new HuffmanTree(InputMemory);

            InputTree.CountBytes();

            Assert.AreEqual(0, InputMemory.Position);
        }

        private void TestCollection(IEnumerable<byte> input, IEnumerable<(byte, ulong)> expected, string message = "")
        {
            InputMemory = new MemoryStream(input.ToArray());
            InputTree = new HuffmanTree(InputMemory);

            var result = InputTree.CountBytes();

            CollectionAssert.AreEqual(expected, result, message);
        }

        [Test]
        public void CountBytes()
        {
            TestCollection(new byte[] { }, new (byte, ulong)[] {}, "Test empty collection");
            TestCollection(new byte[] { 1, 1, 1, 0, 0, 0, 2, 2, 2 }, new (byte, ulong)[] {
                (0,3),
                (1,3),
                (2,3),
            }, "Test some collection"); 
        }

        private void TestHeader(IEnumerable<byte> data, string message = "")
        {
            InputMemory = new MemoryStream(data.ToArray());
            InputTree = new HuffmanTree(InputMemory);

            OutputMemory = new MemoryStream();
            InputTree.WriteTree(OutputMemory);
            OutputMemory.Seek(0, SeekOrigin.Begin);

            OutputTree = new HuffmanTree(OutputMemory, false);

            Assert.True( ( InputTree.TreeRoot == null && OutputTree.TreeRoot == null ) || InputTree.TreeRoot.Equals(OutputTree.TreeRoot), message );
        }
        [Test]
        public void TestHeader()
        {
            TestHeader(new byte[] { }, "Test empty");
            TestHeader(new byte[] { 1 }, "Test 1 element");
            TestHeader(new byte[] { 1, 2, 3, 1, 2, 3 }, "Test 1 2 3 x2");
            var buffer = new byte[15];
            random.NextBytes(buffer);
            TestHeader(buffer, "Test random 15");
            buffer = new byte[100];
            random.NextBytes(buffer);
            TestHeader(buffer, "Test random 100");
        }
    }
}
