using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Protobuf;

using CompressionLibrary.BitOperations;
using BinaryBitLib;

namespace CompressionLibrary.LZW
{
    using CurrentBitWriter = BitWriter;
    public class LZWCompressor : DataCompressor
    {
        private WriteCodeTable Table;
        public LZWCompressor(ushort limit = 65535, bool enableCompression = false): base()
        {
            Table = new WriteCodeTable();
            Table.Limit = limit;
            EnableCompression = enableCompression;
        }

        bool EnableCompression;

        CurrentBitWriter bitOutputFile;
        BinaryWriter OutputFile;
        private void WriteToFile(ushort code)
        {
            if(EnableCompression) 
                bitOutputFile.Write16(code, Table.BitCount );
            else OutputFile.Write(code);
        }

        protected override void Algorithm(Stream InputFile, Stream OutputFile)
        {
            Table.InitTable();
            this.OutputFile = new BinaryWriter(OutputFile);
            bitOutputFile = new CurrentBitWriter(OutputFile);

            WriteToFile(CodeTable.ClearCode);
            ByteString CurString = ByteString.Empty, temp = ByteString.Empty;

            //Readed Bytes A
            int current = 0; byte currentByte = 0;
            //EOF flag
            bool eof = false;
            

            while (true)
            {
                //ReadNextByte()
                current = InputFile.ReadByte();
                eof = current < 0;
                if (eof) break;
                currentByte = (byte)current;

                //CurStr + C in Table?
                temp = CurString.Add(currentByte);
                if (Table.Contains(temp))
                {
                    CurString = temp;
                }
                else
                {
                    //Write current string
                   WriteToFile(Table[CurString]);
                    //Add string to table
                    Table.AddString(temp);

                    CurString = currentByte.ToByteString();

                    //If table is full
                    if (Table.Count == Table.Limit)
                    {
                        //Write last encoded string
                       WriteToFile(Table[CurString]);
                        //Write clear code
                       WriteToFile(CodeTable.ClearCode);
                        Table.InitTable();
                        CurString = ByteString.Empty;
                    }
                }
            };
            if(CurString != ByteString.Empty)WriteToFile(Table[CurString]);
            WriteToFile(CodeTable.EndOfInformation);
            bitOutputFile.Flush();

        }
    }
}
