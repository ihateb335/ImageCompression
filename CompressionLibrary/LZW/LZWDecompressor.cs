using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Protobuf;

using CompressionLibrary.BitOperations;
using BinaryBitLib;
namespace CompressionLibrary.LZW
{
    using CurrentBitReader = BitReader;
    public class LZWDecompressor : DataCompressor
    {
        private ReadCodeTable Table;
        public LZWDecompressor(ushort limit = 65535, bool enableCompression = false) : base()
        {
            Table = new ReadCodeTable();
            Table.Limit = limit;
            EnableCompression = enableCompression;
        }

        bool EnableCompression;

        CurrentBitReader bitInputFile;
        BinaryReader InputFile;
        private ushort ReadUInt16()
        {
            ushort result = 0;
            if (EnableCompression)
                result = bitInputFile.ReadUInt16(Table.BitCount);
                //result = Convert.ToUInt16(bitInputFile.ReadUInt(Table.BitCount));
            else result = InputFile.ReadUInt16();
            return result;
        }


        protected override void Algorithm(Stream InputFile, Stream OutputFile)
        {
            this.InputFile = new BinaryReader(InputFile);
            bitInputFile = new CurrentBitReader(InputFile);

            var Output = new BinaryWriter(OutputFile);

            ushort newCode, oldCode;
            ByteString symbol, chain;

            //Check for end of stream

            if (InputFile.EndOfStream()) return;

            //Read code
            newCode = ReadUInt16();

            //Check For codes
            if (newCode != CodeTable.ClearCode) throw new ArgumentException("Decoded file should start from the code");
            if (newCode == CodeTable.EndOfInformation) return;

            Table.InitTable();

            //Check for end of stream
            if (InputFile.EndOfStream()) return;

            newCode = ReadUInt16();

            oldCode = newCode;

            //Convert to symbol
            symbol = Table.GetString(newCode);
            Output.WriteByteString(symbol);

            //Check for end of stream
            if (InputFile.EndOfStream()) return;
            while (true)
            {
                newCode = ReadUInt16();
                if (newCode == CodeTable.EndOfInformation) break;
                if (newCode == CodeTable.ClearCode)
                {
                    Table.InitTable();
                    newCode = ReadUInt16();
                    if (newCode == CodeTable.EndOfInformation) break;
                    if (newCode == CodeTable.ClearCode) throw new IOException("After ClearCode there cannot be another ClearCode");

                    symbol = Table.GetString(newCode);
                    Output.WriteByteString(symbol);

                    oldCode = newCode;
                    continue;
                }
                if (Table.Contains(newCode))
                {
                    chain = Table.GetString(newCode);
                } else
                {
                    chain = Table.GetString(oldCode).Add(symbol);
                }
                Output.WriteByteString(chain);
                symbol = ByteString.CopyFrom(chain[0]);
                Table.AddString( 
                    Table.GetString(oldCode)
                    .Add(symbol)
                    );
                oldCode = newCode;

            }
        }
    }
}
