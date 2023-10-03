using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibrary.RLE
{
    public class RLEDecompressor : FileCompressor
    {
        protected override void Algorithm(FileStream InputFile, FileStream OutputFile)
        {
            byte[] data = new byte[128], write_data;
            int count = 0, readed = 0;

            bool is_different = false;

            //Write data to decompressed file
            void WriteFiles()
            {
                if (is_different) OutputFile.Write(data, 0, readed);
                else
                {
                    write_data = new byte[count.RLECount()].Select(x => data[0]).ToArray();
                    OutputFile.Write(write_data, 0, write_data.Length);
                }
            }

            while (true)
            {
                //Try to read current byte
                count = InputFile.ReadByte();
                if (count < 0) break;
                //Determine what run is it
                is_different = count < RLEConstants.RLE_FLAG;
                //Read the run
                readed = InputFile.Read(data, 0, is_different ? count.RLECount() : 1);
                //Determine the EOF
                if ((!is_different && readed != 1) || (is_different && readed != count.RLECount())) break;

                //Write decompressed data
                WriteFiles();
            }
            //Write remaining decompressed data if there are any
            if (count > 0)
            {
                WriteFiles();
            }
        }
    }
}
