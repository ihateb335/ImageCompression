using System;
using System.IO;
using NUnit.Framework;
using CompressionLibrary;
using CompressionLibrary.RLEI;

namespace CompressionLibraryTesting
{
    public class RLEITests
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

        void TestCompressor(RLEIData data, string message) => TestCompressor(data.Data, data.ResultData, new RLEICompressor(), message);
        void TestDecompressor(RLEIData data, string message) => TestCompressor(data.ResultData, data.Data, new RLEIDecompressor(), message);

        [Test]
        public void RLECompressorTesting()
        {
            TestCompressor(RLEIData.Data_1, "Test 0 0 0");
            TestCompressor(RLEIData.Data_2, "Test 1 2 3");
            TestCompressor(RLEIData.Data_3, "Test group of bytes len 2");
            TestCompressor(RLEIData.Data_4, "Test group of bytes len 1");
            TestCompressor(RLEIData.Data_5, "Test all same 1 2 3");
            TestCompressor(RLEIData.Data_6, "Test same, different");
            TestCompressor(RLEIData.Data_7, "Test different, same");
            TestCompressor(RLEIData.Data_8, "Test 129 same");
            TestCompressor(RLEIData.Data_9, "Test 129 different");
            TestCompressor(RLEIData.Data_10, "Test 129 same, 127 different");
        }
        [Test]
        public void RLEDecompressorTesting()
        {
            TestDecompressor(RLEIData.Data_1, "Test 0 0 0");
            TestDecompressor(RLEIData.Data_2, "Test 1 2 3");
            TestDecompressor(RLEIData.Data_3, "Test group of bytes len 2");
            TestDecompressor(RLEIData.Data_4, "Test group of bytes len 1");
            TestDecompressor(RLEIData.Data_5, "Test all same 1 2 3");
            TestDecompressor(RLEIData.Data_6, "Test same, different");
            TestDecompressor(RLEIData.Data_7, "Test different, same");
            TestDecompressor(RLEIData.Data_8, "Test 129 same");
            TestDecompressor(RLEIData.Data_9, "Test 129 different");
            TestDecompressor(RLEIData.Data_10, "Test 129 same, 127 different");
            Assert.Pass();
        }
    }
}