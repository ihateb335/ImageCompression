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
    public class HuffmanCompressionTests : DataCompressionTests
    {
        private void TestCompression(IEnumerable<byte> bytes, string message = "") => TestCompression(bytes, new HuffmanCompression(), message);
        Random random = new Random();

        private byte[] GenerateRandomArray(int n)
        {
           var array = new byte[n];
            random.NextBytes(array);
            return array;
        }

        [Test]
        public void TestEmpty()
        {
            TestCompression(new byte[] { }, "Test empty");
        }
        [Test]
        public void Test1()
        {
            TestCompression(new byte[] { 1 }, "Test 1");
            TestCompression(new byte[50].Select(x => (byte)1), "Test 1 x50");
        }
        [Test]
        public void Test1_2_3()
        {
            TestCompression(new byte[] { 1, 2 }, "Test 1 2");
            TestCompression(new byte[] { 1, 2, 3 }, "Test 1 2 3");
            TestCompression(new byte[] { 1, 2, 3, 1, 2, 3 }, "Test 1 2 3 x2");
            TestCompression(new byte[3]
                .Select(x => new byte[] {1, 2, 3}
                .AsEnumerable())
                .Aggregate((cur, next) => cur.Concat(next)), "Test 1 2 3 x3");
            TestCompression(new byte[20]
                .Select(x => new byte[] {1, 2, 3}
                .AsEnumerable())
                .Aggregate((cur, next) => cur.Concat(next)), "Test 1 2 3 x20");

        }
        [Test]
        public void TestSomeCases()
        {
            TestCompression(new byte[] { 165, 39, 158, 231, 214, 185, 30, 160, 205, 83, 224, 190, 34, 206, 199, 209, 236, 57, 68, 222, 163, 30, 55, 232, 116, 57, 144, 208, 176, 90, 222, 97, 157, 104, 248, 46, 234, 5, 171, 4, 106, 14, 224, 80, 195, 230, 22, 210, 167, 147, 138, 171, 125, 118, 141, 134, 240, 181, 115, 157, 41, 77, 240, 114, 116, 127, 33, 185, 213, 152, 230, 231, 142, 63, 115, 149, 62, 231, 37, 142, 34, 189, 41, 28, 207, 51, 227, 38, 219, 113, 62, 52, 145, 13, 219, 28, 29, 110, 219, 67, 196, 92, 253, 14, 173, 163, 29, 61, 2, 247, 72, 8, 143, 229, 123, 108, 36, 212, 162, 65, 156, 250, 221, 99, 89, 178, 85, 128, 100, 86, 120, 207, 11, 226, 201, 197, 191, 26, 111, 44, 22, 166, 35, 228, 40, 53, 165, 172, 146, 110, 248, 72, 203, 154, 167, 202, 126, 154, 181, 251, 77, 156, 217, 171, 127, 120, 253, 173, 28, 181, 172, 9, 101, 118, 42, 246, 215, 249, 58, 36, 12, 169, 238, 245, 94, 47, 4, 113, 59, 255, 70, 37, 101, 116, 222, 121, 204, 119, 100, 175, 56, 247, 143, 221, 194, 239, 227, 85, 111, 177, 242, 132, 95, 144, 238, 164, 174, 227, 77, 79, 116, 140, 114, 29, 110, 134, 164, 241, 151, 176, 41, 95, 149, 138, 235, 21, 25, 16, 50, 111, 76, 175, 1, 137, 12, 193, 198, 212, 134, 58, 203, 232, 41, 20, 59, 172, 133, 114, 110, 59, 74, 241, 189, 191, 135, 147, 239, 213, 165, 216, 139, 157, 176, 222, 3, 196, 139, 232, 17, 173, 192, 207, 107, 93, 229, 64, 53, 128, 79, 63, 143, 166, 162, 246, 36, 90, 241, 66, 2, 120, 210, 240, 47, 173, 174, 46, 55, 75, 17, 118, 33, 156, 97, 193, 123, 122, 134, 96, 218, 71, 94, 111, 134, 101, 72, 228, 250, 185, 186, 169, 211, 73, 230, 152, 219, 207, 160, 190, 174, 182, 8, 234, 111, 216, 30, 243, 68, 225, 123, 169, 211, 145, 104, 187, 26, 98, 106, 201, 100, 202, 52, 125, 144, 103, 163, 216, 182, 201, 230, 171, 218, 200, 177, 35, 62, 115, 255, 173, 71, 85, 159, 25, 62, 16, 213, 66, 225, 42, 125, 120, 100, 215, 89, 228, 130, 139, 89, 7, 53, 70, 60, 123, 251, 208, 207, 11, 223, 68, 125, 166, 99, 188, 130, 14, 42, 26, 63, 127, 146, 97, 247, 140, 75, 110, 70, 2, 110, 205, 161, 179, 26, 247, 120, 0, 25, 36, 29, 110, 65, 202, 98, 156, 173, 214, 20, 167, 84, 74, 186, 103, 75, 217, 117, 212, 79, 175, 47, 141, 137, 204, 156, 18, 163, 201, 139, 107, 68, 129, 245, 6, 253, 209, 62, 199, 254, 90, 222, 116, 89, 158, 174, 36, 18, 57, 103, 65, 130, 164, 177, 105, 244, 144, 228, 117, 46, 80, 249, 227, 74, 169, },
                "Case 1");
        }

        [Test]
        public void TestRandom()
        {
            TestCompression(GenerateRandomArray(10), "Test Random x10");
            TestCompression(GenerateRandomArray(100), "Test Random x100");
            TestCompression(GenerateRandomArray(1000), "Test Random x1000");
            Assert.DoesNotThrow(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    TestCompression(GenerateRandomArray(1000), "Test Random x1000");
                }
            }, "Test Random x1000 100 times");
        }
    }
}
