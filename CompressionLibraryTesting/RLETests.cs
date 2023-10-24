using System;
using System.IO;
using NUnit.Framework;
using CompressionLibrary;
using CompressionLibrary.RLE;

namespace CompressionLibraryTesting
{
    public class RLETests : DataCompressorTests
    {
        [SetUp]
        public void Setup()
        {
        }

        protected override void TestCompressor(Dataset data, string message) => TestCompressor(data.Data, data.ResultData, new RLECompressor(), message);
        protected override void TestDecompressor(Dataset data, string message) => TestCompressor(data.ResultData, data.Data, new RLEDecompressor(), message);

        [Test]
        public void RLECompressorTesting()
        {
            TestCompressor(RLE_Dataset.Data_1, "Test same");
            TestCompressor(RLE_Dataset.Data_2, "Test different");
            TestCompressor(RLE_Dataset.Data_3, "Test same, different");
            TestCompressor(RLE_Dataset.Data_4, "Test different, same");
            TestCompressor(RLE_Dataset.Data_5, "Test zero");
            TestCompressor(RLE_Dataset.Data_6, "Test 129 same");
            TestCompressor(RLE_Dataset.Data_7, "Test 129 different");
            TestCompressor(RLE_Dataset.Data_7, "Test 129 same, 127 different");
        }
        [Test]
        public void RLEDecompressorTesting()
        {
            TestDecompressor(RLE_Dataset.Data_1, "Test same");
            TestDecompressor(RLE_Dataset.Data_2, "Test different");
            TestDecompressor(RLE_Dataset.Data_3, "Test same, different");
            TestDecompressor(RLE_Dataset.Data_4, "Test different, same");
            TestDecompressor(RLE_Dataset.Data_5, "Test zero");
            TestDecompressor(RLE_Dataset.Data_6, "Test 129 same");
            TestDecompressor(RLE_Dataset.Data_7, "Test 129 different");
            TestDecompressor(RLE_Dataset.Data_7, "Test 129 same, 127 different");
        }
    }
}