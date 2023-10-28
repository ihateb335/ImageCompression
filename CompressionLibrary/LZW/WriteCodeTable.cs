using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibrary.LZW
{
    public class WriteCodeTable : CodeTable
    {
        private Dictionary<ByteString, ushort> Table { get; set; } = new Dictionary<ByteString, ushort>();
        public WriteCodeTable() :base() { }

        public override void InitTable()
        {
            base.InitTable();
            Table.Clear();
            for (ushort i = 0; i <= 255; i++)
            {
                Table[ByteString.CopyFrom((byte)i)] = i;
            }
        }
        public bool Contains(ByteString key) => Table.ContainsKey(key);
        public ushort this[ByteString key] => Table[key];
      
        public override int Count => Table.Count;

        /// <summary>
        /// Add string to the table
        /// </summary>
        public override void AddString(ByteString str)
        {
            Table[str] = GetCode();
        }
    }
}
