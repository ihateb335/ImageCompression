using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CompressionLibrary
{
    public abstract class DataCompressor
    {
        /// <summary>
        /// Initial file size in bytes
        /// </summary>
        public long InitialSize { get; internal set; }
        /// <summary>
        /// Compressed or decompressed file size in bytes
        /// </summary>
        public long ResultedSize { get; internal set; }

        /// <summary>
        /// Compression coefficient 
        /// </summary>
        public double CompressionCoefficient => (double)InitialSize / ResultedSize;
        /// <summary>
        /// Compression duration
        /// </summary>
        public TimeSpan CompressionDuration { get; internal set; }

        /// <summary>
        /// Compress/Decompress File
        /// </summary>
        /// <param name="InputFile">File to compress/decompress</param>
        /// <param name="OutputFile">Result of compression/decompression</param>
        public void Use(string InputFile, string OutputFile)
        {
            //Start to read file
            var input = File.OpenRead(InputFile);
            var output = File.OpenWrite(OutputFile);

            var timer = new Stopwatch();

            //Start time before the algorithm
            timer.Start();
            //Template-method containing current algorithm
            Algorithm(input, output);
            //Stop timer
            timer.Stop();

            output.Flush();

            //Stop reading files
            input.Close();
            output.Close();

            //Collect statistic
            var inputInfo = new FileInfo(InputFile);
            var outputInfo = new FileInfo(OutputFile);

            CompressionDuration = timer.Elapsed;
            InitialSize = inputInfo.Length;
            ResultedSize = outputInfo.Length;
        }
        protected abstract void Algorithm(FileStream InputFile, FileStream OutputFile );
    }
}
