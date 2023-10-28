using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibraryTesting
{
    public class LZW_ReadDataset : Dataset
    {
        public ushort[] UShortData { get; set; }
        public override IEnumerable<byte> Data => UShortData.Select(x => BitConverter.GetBytes(x)).Aggregate((accumulate, next) => accumulate.Concat(next).ToArray());

        public Dataset Invert =>  new Dataset
            {
                RawData = ResultData,
                RawResultData = Data
            };



    public static LZW_ReadDataset Data_Invalid_1 = new LZW_ReadDataset
        {
            UShortData = new ushort[] { 10, 257 },
            RawResultData = new byte[] { }
        };
        
        public static LZW_ReadDataset Data_Invalid_2 = new LZW_ReadDataset
        {
            UShortData = new ushort[] { 257 },
            RawResultData = new byte[] { }
        };
        public static LZW_ReadDataset Data_Invalid_3 = new LZW_ReadDataset
        {
            UShortData = new ushort[] { 256 },
            RawResultData = new byte[] { }
        };
        public static LZW_ReadDataset Data_Invalid_4 = new LZW_ReadDataset
        {
            UShortData = new ushort[] { },
            RawResultData = new byte[] { }
        };
        public static LZW_ReadDataset Data_1 = new LZW_ReadDataset
        {
            UShortData = new ushort[] { 256, 10, 257},
            RawResultData = new byte[] { 10 }
        };

        public static LZW_ReadDataset Data_2 = new LZW_ReadDataset
        {
            UShortData = new ushort[] { 256, 10, 11, 257 },
            RawResultData = new byte[] { 10, 11 }
        };
        public static LZW_ReadDataset Data_3 = new LZW_ReadDataset
        {
            UShortData = new ushort[] { 
                256,    // CLR
                10,     // /
                11,     // W
                12,     // E
                13,     // D
                258,    // /W
                12,     // E
                262,    // /WE
                257 },  // END
            RawResultData = new byte[] {
                10,         // /
                11,         // W
                12,         // E
                13,         // D
                10, 11,     // /W
                12,         // E
                10, 11, 12  // /WE
            }
        };
    }
}
