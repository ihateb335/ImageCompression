using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using CompressionLibrary;

namespace CompressionLibraryTesting
{
    public abstract class DataCompressorTests
    {
        public string InputFile = "test_data.bin";
        public string OnputFile = "test_data_output.bin";

        protected Action<IEnumerable<byte>, IEnumerable<byte>, IEnumerable<byte>> DebugAction = (input, output, result) => { };
        protected void TestCompressor(IEnumerable<byte> input_data, IEnumerable<byte> output_data, DataCompressor Compressor, string message)
        {
            File.Delete(InputFile);
            File.Delete(OnputFile);

            var inputFile = File.Create(InputFile);
            var InputData = input_data.ToArray();

            inputFile.Write(InputData, 0, InputData.Length);
            inputFile.Close();

            Compressor.Use(InputFile, OnputFile);

            var result = File.ReadAllBytes(OnputFile);

            if (DebugAction != null) DebugAction.Invoke(input_data, output_data, result);

            CollectionAssert.AreEqual(output_data.ToArray(), result, message);
        }

        protected virtual void TestCompressor(Dataset data, string message) { }
        protected virtual void TestDecompressor(Dataset data, string message) { }
    }
}
