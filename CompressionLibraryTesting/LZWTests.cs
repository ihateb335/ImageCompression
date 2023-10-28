using NUnit.Framework;
using System;
using System.Linq;
using CompressionLibrary.LZW;
using System.Collections.Generic;

namespace CompressionLibraryTesting
{
    public class LZWTests : DataCompressorTests
    {
        [SetUp]
        public void Setup()
        {
            Limit = LIMIT;
        }

        public LZWTests()
        {
            //DebugAction = (input, output, result) =>
            //{
            //    var Output = output.ToArray();
            //    var Result = result.ToArray();

            //    var OutputArr = new ushort[Output.Length / 2];
            //    var ResultArr = new ushort[Result.Length / 2];

            //    for (int i = 0; i < Output.Length; i += 2)
            //    {
            //        OutputArr[i / 2] = BitConverter.ToUInt16(Output, i);
            //    }
            //    for (int i = 0; i < Result.Length; i += 2)
            //    {
            //        ResultArr[i / 2] = BitConverter.ToUInt16(Result, i);
            //    }

            //};
        }

        private const ushort LIMIT = 65535;
        private ushort Limit;
        protected override void TestCompressor(Dataset data, string message) => TestCompressor(data.Data, data.ResultData, new LZWCompressor(Limit), message);
        protected override void TestDecompressor(Dataset data, string message) => TestCompressor(data.ResultData, data.Data, new LZWDecompressor(Limit), message);


        #region Compressor
        [Test]
        public void LZWCompressorTesting()
        {
            TestCompressor(LZW_WriteDataset.Data_1, "0 Should be encoded as 0");
            TestCompressor(LZW_WriteDataset.Data_2, "0 1 Should be encoded as 0 and 1");
            TestCompressor(LZW_WriteDataset.Data_3, "0 1 0 1 Should be encoded as 0 1 258");
            TestCompressor(LZW_WriteDataset.Data_4, "0, 1, 0, 1, 0, 1 Should be encoded as 0 1 258 258");
        }
        [Test]
        public void LZWCompressorLimitedTesting()
        {
            Limit = 259;
            TestCompressor(LZW_WriteDataset.Data_5, "Limit overflow should use some clear codes");
            TestCompressor(LZW_WriteDataset.Data_6, "Limit overflow x6");
            Limit = 260;
            TestCompressor(LZW_WriteDataset.Data_7, "Limit overflow 260 x6");
        }
        #endregion

        #region Decompressor

        private void TestReadInvalid(Dataset data) => TestAction(data.Data, data.ResultData, new LZWDecompressor(Limit), (compressor, input, output, result) => { });

        [Test]
        public void LZWDecompressorNegative()
        {

            Assert.Throws<ArgumentException>(
            () =>
            {
                TestReadInvalid(LZW_ReadDataset.Data_Invalid_1);
            }, "Should result in an error");

            Assert.DoesNotThrow(
            () => {
                TestReadInvalid(LZW_ReadDataset.Data_Invalid_2);
            }, "Shouldn't result in an error 1");

            int a = 0;
            
            Assert.DoesNotThrow(
            () => {
                TestReadInvalid(LZW_ReadDataset.Data_Invalid_3);
            }, "Shouldn't result in an error 2");
            
            //Assert.DoesNotThrow(
            //() => {
            //    TestInvalid(LZW_ReadDataset.Data_Invalid_4);
            //}, "Shouldn't result in an error 3");
        }

        [Test]
        public void TestVariousReads()
        {
            TestDecompressor(LZW_ReadDataset.Data_1.Invert, "256 10 257 should result in a 10");
            TestDecompressor(LZW_ReadDataset.Data_2.Invert, "256 10 11 257 should result in a 10 11");
            TestDecompressor(LZW_ReadDataset.Data_3.Invert, "Part of example should be read");
        }

        [Test]
        public void WriteTestsRead()
        {
            TestDecompressor(LZW_WriteDataset.Data_1, "0 Should be encoded as 0");
            TestDecompressor(LZW_WriteDataset.Data_2, "0 1 Should be encoded as 0 and 1");
            TestDecompressor(LZW_WriteDataset.Data_3, "0 1 0 1 Should be encoded as 0 1 258");
            TestDecompressor(LZW_WriteDataset.Data_4, "0, 1, 0, 1, 0, 1 Should be encoded as 0 1 258 258");
        }

        [Test]
        public void WriteTestsReadLimited()
        {
            Limit = 259;
            TestDecompressor(LZW_WriteDataset.Data_5, "Limit overflow should use some clear codes");
            TestDecompressor(LZW_WriteDataset.Data_6, "Limit overflow x6");
            Limit = 260;
            TestDecompressor(LZW_WriteDataset.Data_7, "Limit overflow 260 x6");
        }
        #endregion
    }
}
