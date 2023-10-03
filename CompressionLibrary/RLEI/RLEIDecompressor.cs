using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibrary.RLEI
{
    public class RLEIDecompressor : FileCompressor
    {
        public int N { get; private set; }
        public RLEIDecompressor(int N = 3) : base()
        {
            this.N = N;
        }

        protected override void Algorithm(FileStream InputFile, FileStream OutputFile)
        {
            byte[] data = new byte[128 * N], write_data;
            int count = 0, readed = 0;

            bool is_different = false;

            void WriteFiles()
            {
                if (is_different || readed % N != 0 ) OutputFile.Write(data, 0, readed);
                else
                {
                    write_data = new byte[count.RLECount() * N].Select((x, i) => data[i % N]).ToArray();
                    OutputFile.Write(write_data, 0, write_data.Length);
                }
            }

            while (true)
            {
                count = InputFile.ReadByte();
                if (count < 0) break;
                is_different = count < RLEConstants.RLE_FLAG;

                readed = InputFile.Read(data, 0, is_different ? count.RLECount() * N : N);
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
