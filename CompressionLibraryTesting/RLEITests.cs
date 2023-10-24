using System;
using System.IO;
using NUnit.Framework;
using CompressionLibrary;
using CompressionLibrary.RLEI;

namespace CompressionLibraryTesting
{
    public class RLEITests : DataCompressorTests
    {
        [SetUp]
        public void Setup()
        {
        }

        protected override void TestCompressor(Dataset data, string message) => TestCompressor(data.Data, data.ResultData, new RLEICompressor(), message);
        protected override void TestDecompressor(Dataset data, string message) => TestCompressor(data.ResultData, data.Data, new RLEIDecompressor(), message);

        [Test]
        public void RLECompressorTesting()
        {
            TestCompressor(RLEI_Dataset.Data_1, "Test 0 0 0");
            TestCompressor(RLEI_Dataset.Data_2, "Test 1 2 3");
            TestCompressor(RLEI_Dataset.Data_3, "Test group of bytes len 2");
            TestCompressor(RLEI_Dataset.Data_4, "Test group of bytes len 1");
            TestCompressor(RLEI_Dataset.Data_5, "Test all same 1 2 3");
            TestCompressor(RLEI_Dataset.Data_6, "Test same, different");
            TestCompressor(RLEI_Dataset.Data_7, "Test different, same");
            TestCompressor(RLEI_Dataset.Data_8, "Test 129 same");
            TestCompressor(RLEI_Dataset.Data_9, "Test 129 different");
            TestCompressor(RLEI_Dataset.Data_10, "Test 129 same, 127 different");
        }
        [Test]
        public void RLEDecompressorTesting()
        {
            TestDecompressor(RLEI_Dataset.Data_1, "Test 0 0 0");
            TestDecompressor(RLEI_Dataset.Data_2, "Test 1 2 3");
            TestDecompressor(RLEI_Dataset.Data_3, "Test group of bytes len 2");
            TestDecompressor(RLEI_Dataset.Data_4, "Test group of bytes len 1");
            TestDecompressor(RLEI_Dataset.Data_5, "Test all same 1 2 3");
            TestDecompressor(RLEI_Dataset.Data_6, "Test same, different");
            TestDecompressor(RLEI_Dataset.Data_7, "Test different, same");
            TestDecompressor(RLEI_Dataset.Data_8, "Test 129 same");
            TestDecompressor(RLEI_Dataset.Data_9, "Test 129 different");
            TestDecompressor(RLEI_Dataset.Data_10, "Test 129 same, 127 different");
            Assert.Pass();
        }
    }
}