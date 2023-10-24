using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibrary
{
    public abstract class DataCompression
    {
        public DataCompression(DataCompressor compressor, DataCompressor decompressor)
        {
            Compressor = compressor;
            Decompressor = decompressor;
        }

        public DataCompressor Compressor { get; private set; }
        public DataCompressor Decompressor { get; private set; }

        public void Decompress(string InputFile, string OutputFile) => Decompressor.Use(InputFile, OutputFile);
        public void Compress(string InputFile, string OutputFile) => Compressor.Use(InputFile, OutputFile);
    }
}
