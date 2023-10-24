using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibrary
{
    public static class ByteStringExtensions
    {
        public static ByteString Concat(this ByteString str, byte b) => ByteString.CopyFrom(str.Append<byte>(b).ToArray());
        public static ByteString Concat(this ByteString str, ByteString other) => ByteString.CopyFrom(str.Concat<byte>(other).ToArray());
        public static ByteString ToByteString(this byte b) => ByteString.CopyFrom(b);
    }
}
