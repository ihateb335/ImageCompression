using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibraryTesting
{
    internal class RLE_Dataset : Dataset
    {
        public static RLE_Dataset Data_1 => new RLE_Dataset {
            Data = new byte[]
            {
                0, 0, 0,
            },
            ResultData = new byte[]
            {
                130, 0
            }
        };
        public static RLE_Dataset Data_2 => new RLE_Dataset {
            Data = new byte[]
            {
                1, 2, 3,
            },
            ResultData = new byte[]
            {
               2, 1, 2, 3
            }
        };
        public static RLE_Dataset Data_3 => new RLE_Dataset {
            Data = new byte[]
            {
                0, 0, 0, 1, 2, 3,
            },
            ResultData = new byte[]
            {
               130, 0, 2, 1, 2, 3
            }
        };
        public static RLE_Dataset Data_4 => new RLE_Dataset {
            Data = new byte[]
            {
                1, 2, 3, 0, 0, 0,
            },
            ResultData = new byte[]
            {
                2, 1, 2, 3, 130, 0,
            }
        };
        public static RLE_Dataset Data_5 => new RLE_Dataset {
            Data = new byte[]
            {
                1
            },
            ResultData = new byte[]
            {
                128, 1
            }
        };
        public static RLE_Dataset Data_6 => new RLE_Dataset {
            Data = new byte[129],
            ResultData = new byte[]
            {
                255, 0, 128, 0
            }
        };
        public static RLE_Dataset Data_7 => new RLE_Dataset {
            Data = new byte[129].Select((x, i) => (byte)((i + 1) % 128)).ToArray(),
            ResultData = new byte[] { 127 }.Concat(new byte[128].Select((x, i) => (byte)((i + 1) % 128))).Concat(new byte[] { 0, 1}).ToArray(),
        };

        public static RLE_Dataset Data_8 => new RLE_Dataset {
            Data = new byte[129].Concat(new byte[127].Select((x, i) => (byte)(i + 1))).ToArray(),
            ResultData = new byte[] { 255, 0, 127, 0 }.Concat(new byte[127].Select((x, i) => (byte)(i + 1))).ToArray(),
        };

      
    }
}
