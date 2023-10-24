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
        protected override void TestDecompressor(Dataset data, string message) => TestCompressor(data.ResultData, data.Data, new LZWCompressor(Limit), message);

       

        [Test]
        public void LZWCompressorTesting()
        {
            Limit = LIMIT;
            TestCompressor(LZW_Dataset.Data_1, "0 Should be encoded as 0");
            TestCompressor(LZW_Dataset.Data_2, "0 1 Should be encoded as 0 and 1");
            TestCompressor(LZW_Dataset.Data_3, "0 1 0 1 Should be encoded as 0 1 258");
            TestCompressor(LZW_Dataset.Data_4, "0, 1, 0, 1, 0, 1 Should be encoded as 0 1 258 258");
            Limit = 259;
            TestCompressor(LZW_Dataset.Data_5, "Limit overflow should use some clear codes");
        }
    }
}
