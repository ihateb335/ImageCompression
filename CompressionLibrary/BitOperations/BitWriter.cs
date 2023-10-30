using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibrary.BitOperations
{
    public class BitWriter : System.IO.BinaryWriter
    {

        private byte byteBuffer = 0;
        private int bitCount = 0;

        protected Encoding Encoding;
        protected bool LeaveOpen;

        public BitWriter(Stream s) : base(s) { Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true); }
        public BitWriter(Stream s, Encoding encoding) : base(s, encoding) { Encoding = encoding; }
        public BitWriter(Stream s, Encoding encoding, bool leaveOpen) : base(s, encoding,leaveOpen) { LeaveOpen = leaveOpen; }

        public new void Flush()
        {
            if(bitCount != 0) base.Write(byteBuffer);
            byteBuffer = 0;
            bitCount = 0;
            base.Flush();
        }

        public void Write8(byte value, int bits = 8)
        {
            if (bits == 0 || bits > 8) throw new ArgumentOutOfRangeException("Bit count cannot equal to 0 or be greater than 8");
           
            //Offset to move bits
            var offset = 8 - bitCount - bits;
            //Value to write should be right after bufferized value
            byte tmp = (byte)((byte)(value << (8 - bits)) >> (8 - bits)) ;

            if (offset >= 0) byteBuffer |= (byte)(tmp << offset);
            else byteBuffer |= (byte)(tmp >> -offset);

            //The buffer is not full
            if(bits + bitCount < 8)
            {
                bitCount = bitCount + bits;
            } else
            {
                //Buffer is full
                var prevBitCount = bitCount;
                bitCount = 8;
                Flush();
                //There are some bits left (e.g. when you write to the non-empty buffer with 8 bits)
                if (bits + prevBitCount > 8)
                {
                    offset = (bits + prevBitCount) % 8;
                    byteBuffer = (byte)(value << (8 - offset));
                    bitCount = offset;
                }
            }
        }
        public void WriteN(byte[] buffer, int bits) {
            if (bits == 0 || bits > buffer.Length * 8 ) throw new ArgumentOutOfRangeException($"Bit count cannot equal to 0 or be greater than {buffer.Length * 8} ");
            int i = 0;
            int i8() => i * 8 + 8;
            for (; i < buffer.Length && i8() <= bits; i++)
            {
                Write8(buffer[i], 8);
            }
            if(bits % 8 != 0)
            {
                Write8(buffer[i], bits % 8);
            }

        }
        public override void Write(byte value) => Write8(value, 8);

        #region Other
        public override void Write(byte[] buffer) { foreach (var value in buffer) Write8(value, 8); }
        public override void Write(byte[] buffer, int index, int count) => Write(buffer.Skip(index).Take(count).ToArray());

        public override void Write(sbyte value) => Write(BitConverter.GetBytes(value));
        public override void Write(bool value) => Write(BitConverter.GetBytes(value));
        public override void Write(char ch) => Write(BitConverter.GetBytes(ch));
        #endregion

        #region Characters and strings
        public override void Write(char[] chars) => Write(Encoding.GetBytes(chars));
        public override void Write(char[] chars, int index, int count) => Write(Encoding.GetBytes(chars.Skip(index).Take(count).ToArray()));
        public override void Write(string value) => Write(Encoding.GetBytes(value));
        #endregion

        #region Integers

        public override void Write(short value) => Write(BitConverter.GetBytes(value));
        public override void Write(ushort value) => Write(BitConverter.GetBytes(value));
        public override void Write(int value) => Write(BitConverter.GetBytes(value));
        public override void Write(uint value) => Write(BitConverter.GetBytes(value));
        public override void Write(long value) => Write(BitConverter.GetBytes(value));
        public override void Write(ulong value) => Write(BitConverter.GetBytes(value));

        #endregion

        #region Longer ints
        public void Write16(short value, int bits) => WriteN(BitConverter.GetBytes(value), bits);
        public void Write32(int value, int bits) => WriteN(BitConverter.GetBytes(value), bits);
        public void Write64(long value, int bits) => WriteN(BitConverter.GetBytes(value), bits);
        public void Write16(ushort value, int bits) => WriteN(BitConverter.GetBytes(value), bits);
        public void Write32(uint value, int bits) => WriteN(BitConverter.GetBytes(value), bits);
        public void Write64(ulong value, int bits) => WriteN(BitConverter.GetBytes(value), bits);

        #endregion

        #region Floats

        public override void Write(float value) => Write(BitConverter.GetBytes(value));
        public override void Write(decimal value) => Write(BitConverter.GetBytes(Convert.ToDouble(value)));
        public override void Write(double value) => Write(BitConverter.GetBytes(value));

        #endregion



        public override void Close()
        {
            Flush();
            base.Close();
        }
    }
}
