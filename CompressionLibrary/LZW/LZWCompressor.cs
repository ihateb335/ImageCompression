using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Protobuf;

namespace CompressionLibrary.LZW
{
    public class LZWCompressor : DataCompressor
    {
        private WriteCodeTable Table;
        public LZWCompressor(ushort limit = 65535): base()
        {
            Table = new WriteCodeTable();
            Table.Limit = limit;
        }
        protected override void Algorithm(Stream InputFile, Stream OutputFile)
        {
            Table.InitTable();
            OutputFile.WriteUShort(CodeTable.ClearCode);
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
                    OutputFile.WriteUShort(Table[CurString]);
                    //Add string to table
                    Table.AddString(temp);

                    CurString = currentByte.ToByteString();

                    //If table is full
                    if (Table.Count == Table.Limit)
                    {
                        //Write last encoded string
                        OutputFile.WriteUShort(Table[CurString]);
                        //Write clear code
                        OutputFile.WriteUShort(CodeTable.ClearCode);
                        Table.InitTable();
                        CurString = ByteString.Empty;
                    }
                }
            };
            if(CurString != ByteString.Empty) OutputFile.WriteUShort(Table[CurString]);
            OutputFile.WriteUShort(CodeTable.EndOfInformation);


        }
    }
}
