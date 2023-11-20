using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using CompressionLibrary;
using System.Collections.Generic;

namespace CompressionLibraryTesting
{
    public class DataCompressionTests
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

            System.Diagnostics.Debug.WriteLine("new byte[] {" + inputData.Select(x => x.ToString()).Aggregate("",(cur, next) => $"{cur} {next}, ") + " }");
            CollectionAssert.AreEqual(inputData, result);
        }
    }
}
