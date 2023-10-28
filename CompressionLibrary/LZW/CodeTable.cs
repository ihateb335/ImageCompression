using System;
using System.Collections.Generic;
using Google.Protobuf;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibrary.LZW
{
    public abstract class CodeTable
    {
        public CodeTable()
        {
            InitTable();
        }
        public virtual void InitTable() => SequenceIndex = 1;

        private ushort SequenceIndex = 1;

        protected ushort GetCode() => (ushort)(EndOfInformation + SequenceIndex++);

        public static ushort ClearCode => 256;
        public static ushort EndOfInformation => 257;

        public ushort Limit { get; set; } = 65535;

        public virtual int Count { get; }

        /// <summary>
        /// Add string to the table
        /// </summary>
        public abstract void AddString(ByteString str);
    }
}
