using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibraryTesting
{
    internal class RLEData
    {
        public byte[] Data { get; set; }
        public byte[] ResultData { get; set; }

        public static RLEData Data_1 => new RLEData {
            Data = new byte[]
            {
                0, 0, 0,
            },
            ResultData = new byte[]
            {
                130, 0
            }
        };
        public static RLEData Data_2 => new RLEData {
            Data = new byte[]
            {
                1, 2, 3,
            },
            ResultData = new byte[]
            {
               2, 1, 2, 3
            }
        };
        public static RLEData Data_3 => new RLEData {
            Data = new byte[]
            {
                0, 0, 0, 1, 2, 3,
            },
            ResultData = new byte[]
            {
               130, 0, 2, 1, 2, 3
            }
        };
        public static RLEData Data_4 => new RLEData {
            Data = new byte[]
            {
                1, 2, 3, 0, 0, 0,
            },
            ResultData = new byte[]
            {
                2, 1, 2, 3, 130, 0,
            }
        };
        public static RLEData Data_5 => new RLEData {
            Data = new byte[]
            {
                1
            },
            ResultData = new byte[]
            {
                128, 1
            }
        };
        public static RLEData Data_6 => new RLEData {
            Data = new byte[129],
            ResultData = new byte[]
            {
                255, 0, 128, 0
            }
        };
        public static RLEData Data_7 => new RLEData {
            Data = new byte[129].Select((x, i) => (byte)((i + 1) % 128)).ToArray(),
            ResultData = new byte[] { 127 }.Concat(new byte[128].Select((x, i) => (byte)((i + 1) % 128))).Concat(new byte[] { 0, 1}).ToArray(),
        };

        public static RLEData Data_8 => new RLEData {
            Data = new byte[129].Concat(new byte[127].Select((x, i) => (byte)(i + 1))).ToArray(),
            ResultData = new byte[] { 255, 0, 127, 0 }.Concat(new byte[127].Select((x, i) => (byte)(i + 1))).ToArray(),
        };

      
    }
}
