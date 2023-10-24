using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibraryTesting
{
    internal class RLEI_Dataset : Dataset
    {
        public static RLEI_Dataset Data_1 => new RLEI_Dataset {
            Data = new byte[]
            {
                0, 0, 0,
            },
            ResultData = new byte[]
            {
                128, 0, 0, 0,
            }
        };
        public static RLEI_Dataset Data_2 => new RLEI_Dataset {
            Data = new byte[]
            {
                1, 2, 3,
            },
            ResultData = new byte[]
            {
               128, 1, 2, 3
            }
        }; 
        public static RLEI_Dataset Data_3 => new RLEI_Dataset {
            Data = new byte[]
            {
                1, 2, 
            },
            ResultData = new byte[]
            {
               128, 1, 2
            }
        };
        public static RLEI_Dataset Data_4 => new RLEI_Dataset {
            Data = new byte[]
            {
                1,
            },
            ResultData = new byte[]
            {
               128, 1
            }
        };
        public static RLEI_Dataset Data_5 => new RLEI_Dataset
        {
            Data = new byte[]
            {
                1, 2, 3,
                1, 2, 3,
                1, 2, 3,
                1, 2, 3,
            },
            ResultData = new byte[]
            {
               131, 1, 2, 3,
            }
        }; 
        public static RLEI_Dataset Data_6 => new RLEI_Dataset
        {
            Data = new byte[]
            {
                1, 2, 3,
                1, 2, 3,
                1, 2, 3,
                0, 0, 0,
                0, 0, 1,
                0, 1, 0,
                0, 1, 1,
            },
            ResultData = new byte[]
            {
                130,
                1, 2, 3, 
                3,
                0, 0, 0,
                0, 0, 1,
                0, 1, 0,
                0, 1, 1,
            }
        };
        public static RLEI_Dataset Data_7 => new RLEI_Dataset
        {
            Data = new byte[]
            {
                0, 0, 0,
                0, 0, 1,
                0, 1, 0,
                0, 1, 1,
                1, 2, 3,
                1, 2, 3,
                1, 2, 3,
            },
            ResultData = new byte[]
            {
                3,
                0, 0, 0,
                0, 0, 1,
                0, 1, 0,
                0, 1, 1,
                130,
                1, 2, 3,
            }
        };

        public static RLEI_Dataset Data_8 => new RLEI_Dataset
        {
            Data = new byte[385],
            ResultData = new byte[]
            {
                255, 0, 0, 0, 128, 0
            }
        };
        public static RLEI_Dataset Data_9 => new RLEI_Dataset
        {
            Data = new byte[385]
                   .Select((x, i) => (byte)(i % 6))
                   .ToArray(),
            ResultData = 
            new byte[]{ 127,}
            .Concat(
                new byte[384]
                .Select((x, i) => (byte)(i % 6))
                )
            .Concat(new byte[] { 128, 0 }).ToArray()
        };
        public static RLEI_Dataset Data_10 => new RLEI_Dataset
        {
            Data = new byte[385]
                  .Concat(
                        new byte[] { 1, 2 }
                        .Concat(
                            new byte[381]
                            .Select((x, i) => (byte)((i + 3) % 6))
                        )
                  )
                  .ToArray(),
           ResultData =
           new byte[] { 255, 0, 0, 0, 127 }
           .Concat(
               new byte[384]
               .Select((x, i) => (byte)(i % 6))
               )
           .ToArray()
        };
    }
}
