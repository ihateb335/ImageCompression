using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CompressionLibrary;

namespace Lab_01
{
    internal class DataCompressionModel
    {
        public DataCompression Compression { get; set; }
        public string MethodName { get; set; }
        public string Extension { get; set; }
        public override string ToString() => MethodName;
    }
}
