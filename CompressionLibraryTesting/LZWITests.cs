using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using CompressionLibrary;
using CompressionLibrary.LZW;
using System.Collections.Generic;

namespace CompressionLibraryTesting
{
    public class LZWITests
    {
        protected byte[] WriterOutput;
        protected void TestCompression(IEnumerable<byte> inputData, DataCompression compression, string message)
        {
            var WriterInput = new MemoryStream();
            var ReaderInput = new MemoryStream();
            var ReaderOutput = new MemoryStream();

            var InputData = inputData.ToArray();

            WriterInput.Write(InputData, 0, InputData.Length);
            WriterInput.Seek(0, SeekOrigin.Begin);

            compression.Compressor.Use(WriterInput, ReaderInput);


            WriterOutput = ReaderInput.ToArray();
            ReaderInput.Seek(0, SeekOrigin.Begin);
            compression.Decompressor.Use(ReaderInput, ReaderOutput);

            var result = ReaderOutput.ToArray();

            WriterInput.Close();
            ReaderInput.Close();
            ReaderOutput.Close();

            CollectionAssert.AreEqual(inputData, result);
        }
        [SetUp]
        public void Setup()
        {
            Limit = LIMIT;
        }

        const ushort LIMIT = ushort.MaxValue;
        ushort Limit = LIMIT;
        private void TestCompression(Dataset data, string message) => TestCompression(data.Data, new LZWCompression(Limit), message);


        [Test]
        public void LZWCompressorTesting()
        {
            TestCompression(LZW_WriteDataset.Data_1, "0 Should be encoded as 0");
            TestCompression(LZW_WriteDataset.Data_2, "0 1 Should be encoded as 0 and 1");
            TestCompression(LZW_WriteDataset.Data_3, "0 1 0 1 Should be encoded as 0 1 258");
            TestCompression(LZW_WriteDataset.Data_4, "0, 1, 0, 1, 0, 1 Should be encoded as 0 1 258 258");
        }
        [Test]
        public void LZWCompressorLimitedTesting()
        {
            Limit = 259;
            TestCompression(LZW_WriteDataset.Data_5, "Limit overflow should use some clear codes");
            TestCompression(LZW_WriteDataset.Data_6, "Limit overflow x6");
            Limit = 260;
            TestCompression(LZW_WriteDataset.Data_7, "Limit overflow 260 x6");
        }
        [Test]
        public void TestVariousReads()
        {
            TestCompression(LZW_ReadDataset.Data_1.Invert, "256 10 257 should result in a 10");
            TestCompression(LZW_ReadDataset.Data_2.Invert, "256 10 11 257 should result in a 10 11");
            TestCompression(LZW_ReadDataset.Data_3.Invert, "Part of example should be read");
        }

        [Test]
        public void WriteTestsRead()
        {
            TestCompression(LZW_WriteDataset.Data_1, "0 Should be encoded as 0");
            TestCompression(LZW_WriteDataset.Data_2, "0 1 Should be encoded as 0 and 1");
            TestCompression(LZW_WriteDataset.Data_3, "0 1 0 1 Should be encoded as 0 1 258");
            TestCompression(LZW_WriteDataset.Data_4, "0, 1, 0, 1, 0, 1 Should be encoded as 0 1 258 258");
        }

        [Test]
        public void WriteTestsReadLimited()
        {
            Limit = 259;
            TestCompression(LZW_WriteDataset.Data_5, "Limit overflow should use some clear codes");
            TestCompression(LZW_WriteDataset.Data_6, "Limit overflow x6");
            Limit = 260;
            TestCompression(LZW_WriteDataset.Data_7, "Limit overflow 260 x6");
        }
        [Test]
        public void Write10x10WithLimits()
        {
            Limit = 259;
            TestCompression(LZWFileDataset.Whitenoise_10x10, "Whitenoise 10x10 Limit 259");
            Limit = 510;
            TestCompression(LZWFileDataset.Whitenoise_10x10, "Whitenoise 10x10 Limit 510");
            Limit = 1022;
            TestCompression(LZWFileDataset.Whitenoise_10x10, "Whitenoise 10x10 Limit 1022");
            Limit = 2046;
            TestCompression(LZWFileDataset.Whitenoise_10x10, "Whitenoise 10x10 Limit 2046");
        }
        [Test]
        public void WriteWhitenoise_smallWithLimits()
        {
            Limit = 259;
            TestCompression(LZWFileDataset.Whitenoise_small, "Whitenoise Whitenoise_small Limit 259");
            Limit = 510;
            TestCompression(LZWFileDataset.Whitenoise_small, "Whitenoise Whitenoise_small Limit 510");
            Limit = 1022;
            TestCompression(LZWFileDataset.Whitenoise_small, "Whitenoise Whitenoise_small Limit 1022");
            Limit = 2046;
            TestCompression(LZWFileDataset.Whitenoise_small, "Whitenoise Whitenoise_small Limit 2046");
            Limit = 4093;
            TestCompression(LZWFileDataset.Whitenoise_small, "Whitenoise Whitenoise_small Limit 4093");
        }
        [Test]
        public void WriteMosaicWithLimits()
        {
            Limit = 259;
            TestCompression(LZWFileDataset.Mosaic, "Mosaic Limit 259");
            Limit = 510;
            TestCompression(LZWFileDataset.Mosaic, "Mosaic Limit 510");
            Limit = 1022;
            TestCompression(LZWFileDataset.Mosaic, "Mosaic Limit 1022");
            Limit = 2046;
            TestCompression(LZWFileDataset.Mosaic, "Mosaic Limit 2046");
            Limit = 4094;
            TestCompression(LZWFileDataset.Mosaic, "Mosaic Limit 4094");
            Limit = 16381;
            TestCompression(LZWFileDataset.Mosaic, "Mosaic Limit 16000");
        }
        [Test]
        public void WriteWhitenoiseWithLimits()
        {
            Limit = 259;
            TestCompression(LZWFileDataset.Whitenoise, "Whitenoise Whitenoise Limit 259");
            Limit = 510;
            TestCompression(LZWFileDataset.Whitenoise, "Whitenoise Whitenoise Limit 510");
            Limit = 1022;
            TestCompression(LZWFileDataset.Whitenoise, "Whitenoise Whitenoise Limit 1022");
            Limit = 2046;
            TestCompression(LZWFileDataset.Whitenoise, "Whitenoise Whitenoise Limit 2046");
            Limit = 4092;
            TestCompression(LZWFileDataset.Whitenoise, "Whitenoise Whitenoise Limit 4092");
        }

    }
}
