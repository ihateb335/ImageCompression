using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CompressionLibrary;
using CompressionLibrary.RLE;

namespace CompressionLibraryTesting
{
    public class Program
    {
        public string CurrentDir => Directory.GetCurrentDirectory();
        static void Main(string[] args)
        {
            //            FileCompressor compressor = new RLECompressor();
            //            string input = @"C:\Users\MrZahn\source\repos\University\Laboratory\5 курс\1 семестр\ImageCompressionData\Lab_01\1 Class\01.bmp";
            //            string output = "output.bmp";
            //            string decompressed = "output_decompressed.bmp";
            //
            //            compressor.Use(input, output);
            //            Console.WriteLine(
            //$@"
            //Initial = {compressor.InitialSize}
            //Output = {compressor.ResultedSize}
            //Coefficient = {compressor.CompressionCoefficient, 2:F4}
            //Time = {compressor.CompressionDuration.ToString()}
            //            ");
            //
            //            compressor = new RLEDecompressor();
            //            compressor.Use(output, decompressed);
            //            Console.WriteLine(
            //$@"
            //Initial = {compressor.InitialSize}
            //Output = {compressor.ResultedSize}
            //Coefficient = {compressor.CompressionCoefficient,2:F4}
            //Time = {compressor.CompressionDuration.ToString()}
            //            ");

            foreach (var item in new byte[385]
                  .Concat(
                        new byte[] { 1, 2 }
                        .Concat(
                            new byte[381]
                            .Select((x, i) => (byte)((i + 3) % 6))
                        )
                  )
            )
            {
                Console.Write($"{item} ");
            }
            Console.WriteLine();
            foreach (var item in new byte[] { 255, 0, 0, 0, 127 }
                                   .Concat(
                                       new byte[384]
                                       .Select((x, i) => (byte)(i % 6))
                                       )
            )
            {
                Console.Write($"{item} ");
            }
        }
    }
}
