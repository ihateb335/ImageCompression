using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using CompressionLibrary;
using CompressionLibrary.BitOperations;
using System.Diagnostics;

using BinaryBitLib;

namespace CompressionLibraryTesting
{
    public class Program
    {
        public string CurrentDir => Directory.GetCurrentDirectory();

        public static void BitReader()
        {
            Stream file = File.Open(@"C:\Users\MrZahn\source\repos\University\Laboratory\5 курс\1 семестр\ImageCompressionData\Lab_01\1 Class\RED.bmp", FileMode.Open);

            var reader = new BinaryBitReader(file);

            //Start time before the algorithm
            var timer = new Stopwatch();
            timer.Start();
            for (int i = 0; i < file.Length; i++)
            {
                reader.ReadByte();
            }
            timer.Stop();
            Console.WriteLine($"Other bit reader: {timer.ElapsedMilliseconds}");
        }

        public static void MyBitReader()
        {
            Stream file = File.Open(@"C:\Users\MrZahn\source\repos\University\Laboratory\5 курс\1 семестр\ImageCompressionData\Lab_01\1 Class\RED_test.bmp", FileMode.Open);

            file.Seek(0, SeekOrigin.Begin);

            var reader = new BitReader(file);
            //Start time before the algorithm
            var timer = new Stopwatch();
            timer.Start();
            for (int i = 0; i < file.Length; i++)
            {
                reader.ReadByte();
            }
            timer.Stop();
            Console.WriteLine($"My bit reader: {timer.ElapsedMilliseconds}");
        }

        static void Test()
        {
            Task.Run(() => { BitReader(); });
            Task.Run(() => { MyBitReader(); });

            Thread.Sleep(8000);
        }
        static void Main(string[] args)
        {
            using (var sr = new BinaryReader(File.OpenRead(@"C:\Users\MrZahn\source\repos\University\Laboratory\5 курс\1 семестр\ImageCompressionData\Lab_01\Tests\Stripes-small.lzw")))
            {
                while (sr.BaseStream.Position != sr.BaseStream.Length)
                {
                    Console.Write($"{sr.ReadUInt16()} ");
                }
            }


        }
    }
}
