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
        protected Action<IEnumerable<byte>, IEnumerable<byte>, IEnumerable<byte>> DebugAction = (input, output, result) => { };
        protected void TestCompressor(IEnumerable<byte> input_data, IEnumerable<byte> output_data, DataCompressor Compressor, string message)
        {
            var InputStream = new MemoryStream();
            var OutputStream = new MemoryStream();

            var InputData = input_data.ToArray();


            InputStream.Write(InputData, 0, InputData.Length);
            InputStream.Seek(0, SeekOrigin.Begin);

            Compressor.Use(InputStream, OutputStream);

            var result = OutputStream.ToArray();

            InputStream.Close();
            OutputStream.Close();

            if (DebugAction != null) DebugAction.Invoke(input_data, output_data, result);

            CollectionAssert.AreEqual(output_data.ToArray(), result, message);
        }

        protected virtual void TestCompressor(Dataset data, string message) { }
        protected virtual void TestDecompressor(Dataset data, string message) { }
    }
}
