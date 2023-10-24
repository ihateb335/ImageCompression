using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibraryTesting
{
    public class Dataset
    {
        public IEnumerable<byte> Data { get; set; }
        public IEnumerable<byte> ResultData { get; set; }
    }
}
