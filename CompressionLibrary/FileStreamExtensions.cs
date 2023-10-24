using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibrary
{
    public static class FileStreamExtensions
    {
        public static void WriteUShort(this FileStream fileStream, ushort value) => fileStream.Write(BitConverter.GetBytes(value), 0, 2);
    }
}
