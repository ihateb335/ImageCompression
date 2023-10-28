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

        protected void TestAction(IEnumerable<byte> inputData, IEnumerable<byte> outputData, DataCompressor Compressor, Action<DataCompressor, byte[], byte[], byte[]> testAction)
        {
            var InputStream = new MemoryStream();
            var OutputStream = new MemoryStream();

            var InputData = inputData.ToArray();

            InputStream.Write(InputData, 0, InputData.Length);
            InputStream.Seek(0, SeekOrigin.Begin);

            Compressor.Use(InputStream, OutputStream);

            var result = OutputStream.ToArray();

            InputStream.Close();
            OutputStream.Close();

            if (DebugAction != null) DebugAction.Invoke(inputData, outputData, result);

            testAction(Compressor, InputData, outputData.ToArray(), result);
        }
        protected void TestCompressor(IEnumerable<byte> inputData, IEnumerable<byte> outputData, DataCompressor Compressor, string message) =>
             TestAction(inputData, outputData, Compressor, (compressor, input, output, result) => {
                 CollectionAssert.AreEqual(outputData.ToArray(), result, message);
             });



        protected virtual void TestCompressor(Dataset data, string message) { }
        protected virtual void TestDecompressor(Dataset data, string message) { }
    }
}
