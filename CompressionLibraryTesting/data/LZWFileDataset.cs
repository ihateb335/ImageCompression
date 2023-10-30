using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibraryTesting
{
    public class LZWFileDataset : Dataset
    {
        public LZWFileDataset(string path)
        {
            RawData = File.ReadAllBytes(path);
        }

        public static LZWFileDataset Whitenoise_10x10 =>
            new LZWFileDataset(@"C:\Users\MrZahn\source\repos\University\Laboratory\5 курс\1 семестр\ImageCompressionData\Lab_01\Tests\10x10.bmp");
        public static LZWFileDataset Mosaic =>
            new LZWFileDataset(@"C:\Users\MrZahn\source\repos\University\Laboratory\5 курс\1 семестр\ImageCompressionData\Lab_01\Tests\Mosaic.bmp");
        public static LZWFileDataset Whitenoise_small =>
            new LZWFileDataset(@"C:\Users\MrZahn\source\repos\University\Laboratory\5 курс\1 семестр\ImageCompressionData\Lab_01\Tests\Whitenoise_small.bmp");
        public static LZWFileDataset Whitenoise =>
            new LZWFileDataset(@"C:\Users\MrZahn\source\repos\University\Laboratory\5 курс\1 семестр\ImageCompressionData\Lab_01\Tests\Whitenoise.bmp");


    }
}
