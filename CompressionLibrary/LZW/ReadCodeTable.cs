using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibrary.LZW
{
    public class ReadCodeTable : CodeTable
    {
        Dictionary<ushort, ByteString> Table = new Dictionary<ushort, ByteString>();
        public override int Count => Table.Count;

        public ReadCodeTable() : base() { }

        /// <summary>
        /// Populatess table with codes and bytes
        /// </summary>
        public override void InitTable()
        {
            base.InitTable();
            Table.Clear();
            for (ushort i = 0; i <= 255; i++)
            {
                Table[i] = ByteString.CopyFrom((byte)i);
            }
        }

        /// <summary>
        /// Get string from code
        /// </summary>
        /// <param name="code">Code of the string</param>
        /// <returns>String of bytes</returns>
        public ByteString GetString(ushort code) {
            ByteString vs;

            if (code == ClearCode) return ByteString.Empty;

            if(!Table.TryGetValue(code, out vs))
                throw new ArgumentException("There is no string for this code");
            return vs;
        }

        /// <summary>
        /// Check if there is a code
        /// </summary>
        /// <param name="code">Code of the string</param>
        /// <returns></returns>
        public bool Contains(ushort code) => Table.ContainsKey(code) || code == EndOfInformation || code == ClearCode;

        /// <summary>
        /// Get first byte of the byte string
        /// </summary>
        /// <param name="code">Code of the byte string</param>
        /// <returns>First byte</returns>
        public byte FirstChar(ushort code) => GetString(code)[0];

        /// <summary>
        /// Add string to the table
        /// </summary>
        public override void AddString(ByteString str) { 
            Table[GetCode()] = str;
        }
    }
}
