using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using CompressionLibrary.Huffman;
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
            var bytes = new byte[10].Select(i => (byte)0).Concat(
                new byte[5].Select(i => (byte)1)
                ).Concat(
                 new byte[8].Select(i => (byte)2)
                ).Concat(
                 new byte[13].Select(i => (byte)3)
                ).Concat(
                 new byte[10].Select(i => (byte)4)
                )
                .ToArray();
            using (var sr = new MemoryStream(bytes))
            {
                var a = new HuffmanTree(sr);

                foreach (var item in new byte[] {0, 1, 2, 3, 4})
                {
                   var code = a.GetCode(item);
                    Console.WriteLine($"Code: {Convert.ToInt32(code,2)}, bits = {code.Length}");
                }

                var b = new MemoryStream();

                a.WriteTree(b);
            }


        }
    }
}
