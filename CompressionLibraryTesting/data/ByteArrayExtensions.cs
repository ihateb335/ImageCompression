using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibraryTesting
{
    internal static class ByteArrayExtensions
    {
        public static byte[] AsUShortByteArray(this int a) => BitConverter.GetBytes((ushort)a);
        public static byte[] Add( this byte[] a, byte[] b) => a.Concat(b).ToArray();
        public static byte[] Add( this byte a, byte[] b) => b.Prepend(a).ToArray();
        public static byte[] Add( this byte[] a, byte b) => a.Append(b).ToArray();
    }
}
