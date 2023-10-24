using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibrary.RLEI
{
    public class RLEIDecompressor : DataCompressor
    {
        public int N { get; private set; }
        public RLEIDecompressor(int N = 3) : base()
        {
            this.N = N;
        }

        protected override void Algorithm(Stream InputFile, Stream OutputFile)
        {
            byte[] data = new byte[128 * N], write_data;
            int count = 0, readed = 0;

            bool is_different = false;

            //Write data to decompressed file
            void WriteFiles()
            {
                //If the bytes are different or there are less bytes in the group than N write them down
                if (is_different || readed % N != 0 ) OutputFile.Write(data, 0, readed);
                //Write the byte group
                else
                {
                    write_data = new byte[count.RLECount() * N].Select((x, i) => data[i % N]).ToArray();
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
                //Read the run based on the run type
                readed = InputFile.Read(data, 0, is_different ? count.RLECount() * N : N);
                //Determine the EOF
                if ((!is_different && readed != N) || (is_different && readed != count.RLECount() * N)) break;

                WriteFiles();
            }
            if (count > 0)
            {
                WriteFiles();
            }

        }

    }
}
