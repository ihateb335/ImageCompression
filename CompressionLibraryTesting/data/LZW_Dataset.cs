using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibraryTesting
{
    internal class LZW_Dataset : Dataset
    {
        public static LZW_Dataset Data_1 => new LZW_Dataset
        {
            Data = new byte[] { 
                0
            },
            ResultData = 
            256.AsUShortByteArray()
            .Concat( 0.AsUShortByteArray() )
            .Concat( 257.AsUShortByteArray() ),
        }; 
        public static LZW_Dataset Data_2 => new LZW_Dataset
        {
            Data = new byte[] { 
                0, 1
            },
            ResultData = 
            256.AsUShortByteArray()
            .Concat( 0.AsUShortByteArray() )
            .Concat( 1.AsUShortByteArray() )
            .Concat( 257.AsUShortByteArray() ),
        };
        public static LZW_Dataset Data_3 => new LZW_Dataset
        {
            Data = new byte[] { 
                0, 1, 0, 1
            },
            ResultData = 
            256.AsUShortByteArray()
            .Concat( 0.AsUShortByteArray() )
            .Concat( 1.AsUShortByteArray() )
            .Concat( 258.AsUShortByteArray() )
            .Concat( 257.AsUShortByteArray() ),
        };
        public static LZW_Dataset Data_4 => new LZW_Dataset
        {
            Data = new byte[] { 
                0, 1, 0, 1, 0, 1
            },
            ResultData = 
            256.AsUShortByteArray()
            .Concat( 0.AsUShortByteArray() )
            .Concat( 1.AsUShortByteArray() )
            .Concat( 258.AsUShortByteArray() )
            .Concat( 258.AsUShortByteArray() )
            .Concat( 257.AsUShortByteArray() ),
        };
        public static LZW_Dataset Data_5 => new LZW_Dataset
        {
            Data = new byte[] { 
                0, 1, 0, 1, 1, 2, 3, 1, 2, 3
            },
            ResultData = 
            256.AsUShortByteArray()
            .Concat( 0.AsUShortByteArray() ) // 0 1 = 258
            .Concat( 1.AsUShortByteArray() ) // 1 0 = 259
            .Concat( 258.AsUShortByteArray() ) // 0 1 1 = 260
            .Concat( 1.AsUShortByteArray()) // Write and clear
            .Concat( 256.AsUShortByteArray()) // Clear
            .Concat( 2.AsUShortByteArray() ) // 2 3 = 258
            .Concat( 3.AsUShortByteArray() ) // 3 1 = 259
            .Concat( 1.AsUShortByteArray() ) // 1 2 = 260
            .Concat( 258.AsUShortByteArray()) //Write before end of stream
            .Concat( 257.AsUShortByteArray() ), //End of stream
        };
    }
}
