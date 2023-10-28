using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibraryTesting
{
    internal class Data<T>
    {
        public IEnumerable<T> ToCopy { get; set; } = new List<T>();
        public IEnumerable<T> Initial { get; set; } = new List<T>();
        public IEnumerable<T> ToAppend { get; set; } = new List<T>();
        public IEnumerable<T> Generate(ushort times = 1)
        {
            IEnumerable<T> copyum = new List<T>(Initial);
            for (ushort i = 1; i < times; i++)
            {
                copyum = copyum.Concat(ToCopy);
            }
            copyum = copyum.Concat(ToAppend);
            return copyum;
        }
    }

    internal class LZW_WriteDataset : Dataset
    {
        public override IEnumerable<byte> ResultData => UShortData.Select(x => BitConverter.GetBytes(x)).Aggregate((accumulate, next) => accumulate.Concat(next).ToArray());
        private IEnumerable<ushort> UShortData { get; set; } 


      
        public static LZW_WriteDataset Data_1 => new LZW_WriteDataset
        {
            RawData = new byte[] { 
                0
            },
            UShortData = new ushort[] {256, 0, 257}
        }; 
        public static LZW_WriteDataset Data_2 => new LZW_WriteDataset
        {
            RawData = new byte[] { 
                0, 1
            },
            UShortData = new ushort[] { 256, 0, 1, 257 }            
        };
        public static LZW_WriteDataset Data_3 => new LZW_WriteDataset
        {
            RawData = new byte[] { 
                0, 1, 0, 1
            },
            UShortData = new ushort[] { 256, 0, 1, 258, 257 }            
        };
        public static LZW_WriteDataset Data_4 => new LZW_WriteDataset
        {
            RawData = new byte[] { 
                0, 1, 0, 1, 0, 1
            },
            UShortData = new ushort[] { 256, 0, 1, 258, 258, 257 }
        };
        /// <summary>
        /// Length 259
        /// </summary>
        public static LZW_WriteDataset Data_5 => new LZW_WriteDataset
        {
            RawData = new byte[] { 
                0, 1, 0, 1, 1,
                2, 3, 1, 2, 3
            },
            UShortData = new ushort[] {
                256,    
                0,      // 0 1 = 258
                1,      // 1 0 = 259
                258,    // 0 1 1 = 260
                1,      // At table limit write last
                256,    // Clear
                2,      // 2 3 = 258
                3,      // 3 1 = 259
                1,      // 1 2 = 260
                2,      // At table limit write last
                256,    // Clear
                3,      // Write before end of stream
                257     // End of stream
            }
        };
        /// <summary>
        /// Length 259
        /// </summary>
        public static LZW_WriteDataset Data_6 {
            get {
                ushort times = 6;
                var input = new Data<byte> { ToCopy = new byte[] { 0, 0, 0, 0, 1 } };
                var output = new Data<ushort> { 
                    ToCopy = new ushort[] { 256, 0, 258, 0, 1 },
                    ToAppend = new ushort[] { 256, 257 }
                };

                return new LZW_WriteDataset
                {
                    RawData = input.Generate(times),
                    UShortData = output.Generate(times)
                };
            }
        }

        /// <summary>
        /// Length 260
        /// </summary>
        public static LZW_WriteDataset Data_7 {
            get {
                ushort times = 6;
                var input = new Data<byte> { ToCopy = new byte[] { 0, 0, 0, 1, 1, 0 } };
                var output = new Data<ushort> { 
                    ToCopy = new ushort[] { 256, 0, 258, 1, 1, 0 },
                    ToAppend = new ushort[] { 256, 257 }
                };

                return new LZW_WriteDataset
                {
                    RawData = input.Generate(times),
                    UShortData = output.Generate(times)
                };
            }
        }
            
           
    }
}
