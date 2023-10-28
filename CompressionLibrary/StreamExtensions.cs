using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Protobuf;

namespace CompressionLibrary
{
    public static class StreamExtensions
    {
        public static void WriteUShort(this Stream fileStream, ushort value) => fileStream.Write(BitConverter.GetBytes(value), 0, 2);
        public static void WriteByteString(this Stream stream, ByteString str)
        {
            var result = str.ToByteArray();
            stream.Write(result, 0, result.Length);
        }

        public static void WriteByteString(this BinaryWriter stream, ByteString str) => stream.BaseStream.WriteByteString(str);
        public static bool EndOfStream(this Stream stream) => stream.Position >= stream.Length;
    }
}
