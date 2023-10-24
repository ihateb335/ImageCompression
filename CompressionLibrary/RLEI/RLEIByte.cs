using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibrary.RLEI
{
    public sealed class RLEIByte : IEquatable<RLEIByte>
    {
        private int[] _data;

        public int N => _data.Length;
        public bool EOF => _data.Any(b => b <= -1);
        public bool HasBytes => _data.Any(b => b > -1);

        public RLEIByte(int n = 3)
        {
            if(n < 1) throw new ArgumentException("N must be greater than 1");
            _data = new int[n];
            for (int i = 0; i < N; i++) _data[i] = -1;
        }

        public void ReadByte(Stream fileStream)
        {
            for (int i = 0; i < N; i++)
            {
                _data[i] = fileStream.ReadByte();
            }
        }

        public void WriteByte(Stream fileStream)
        {
            for (int i = 0; i < N; i++)
            {
               if (_data[i] <= -1) break;
               fileStream.WriteByte((byte)_data[i]);
            }
        }

        public bool Equals(RLEIByte other)
        {
            if (N != other.N) return false;
            bool equal = true;

            for (int i = 0; i < N && equal; i++)
            {
               equal = other._data[i] == _data[i] && equal;
            }

            return equal;
        }
    }

    public static class RLEIExtensions
    {
        public static RLEIByte ReadRLEIByte(this Stream fileStream, int n = 3)
        {
            var rleIByte = new RLEIByte(n);
            rleIByte.ReadByte(fileStream);
            return rleIByte;
        }
        public static void WriteRLEIByte(this Stream fileStream, RLEIByte @byte)
        {
            @byte.WriteByte(fileStream);
        }
    }
}
