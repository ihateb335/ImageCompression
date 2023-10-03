using System;
using System.IO;
using NUnit.Framework;
using CompressionLibrary;
using CompressionLibrary.RLE;

namespace CompressionLibraryTesting
{
    public class RLETests
    {
        [SetUp]
        public void Setup()
        {
        }
        public string InputFile = "test_data.bin";
        public string OnputFile = "test_data_output.bin";
        void TestCompressor(byte[] input_data, byte[] output_data, FileCompressor Compressor , string message)
        {
            File.Delete(InputFile);
            File.Delete(OnputFile);

            var inputFile = File.Create(InputFile);
            inputFile.Write(input_data, 0, input_data.Length);
            inputFile.Close();

            Compressor.Use(InputFile, OnputFile);

            var result = File.ReadAllBytes(OnputFile); 

            
            CollectionAssert.AreEqual(output_data, result, message);
        }

        void TestCompressor(RLEData data, string message) => TestCompressor(data.Data, data.ResultData, new RLECompressor(), message);
        void TestDecompressor(RLEData data, string message) => TestCompressor(data.ResultData, data.Data, new RLEDecompressor(), message);

        [Test]
        public void RLECompressorTesting()
        {
            TestCompressor(RLEData.Data_1, "Test same");
            TestCompressor(RLEData.Data_2, "Test different");
            TestCompressor(RLEData.Data_3, "Test same, different");
            TestCompressor(RLEData.Data_4, "Test different, same");
            TestCompressor(RLEData.Data_5, "Test zero");
            TestCompressor(RLEData.Data_6, "Test 129 same");
            TestCompressor(RLEData.Data_7, "Test 129 different");
            TestCompressor(RLEData.Data_7, "Test 129 same, 127 different");
        }
        [Test]
        public void RLEDecompressorTesting()
        {
            TestDecompressor(RLEData.Data_1, "Test same");
            TestDecompressor(RLEData.Data_2, "Test different");
            TestDecompressor(RLEData.Data_3, "Test same, different");
            TestDecompressor(RLEData.Data_4, "Test different, same");
            TestDecompressor(RLEData.Data_5, "Test zero");
            TestDecompressor(RLEData.Data_6, "Test 129 same");
            TestDecompressor(RLEData.Data_7, "Test 129 different");
            TestDecompressor(RLEData.Data_7, "Test 129 same, 127 different");
        }
    }
}