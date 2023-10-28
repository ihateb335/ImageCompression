using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibraryTesting
{
    public class Dataset
    {
        public virtual IEnumerable<byte> Data => RawData;
        public virtual IEnumerable<byte> ResultData => RawResultData;

        public IEnumerable<byte> RawData { get; set; }
        public IEnumerable<byte> RawResultData { get; set; }
    }
}
