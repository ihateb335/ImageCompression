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

            Use(input, output);

            //Stop reading files
            input.Close();
            output.Close();
        }

        public void Use(Stream Input, Stream Output)
        {
            var timer = new Stopwatch();

            //Start time before the algorithm
            timer.Start();
            //Template-method containing current algorithm
            Algorithm(Input, Output);
            //Stop timer
            timer.Stop();

            Output.Flush();

            //Collect statistics
            CompressionDuration = timer.Elapsed;
            InitialSize = Input.Length;
            ResultedSize = Output.Length;
        }
        protected abstract void Algorithm(Stream InputFile, Stream OutputFile );
    }
}
