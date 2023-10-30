using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibrary.BitOperations
{
    public class BitReader : System.IO.BinaryReader
    {
        private byte byteBuffer = 0;
        private int bitCount = 0;

        protected Encoding Encoding;
        protected bool LeaveOpen;

        public BitReader(Stream s) : base(s) { Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true); }
        public BitReader(Stream s, Encoding encoding) : base(s, encoding) { Encoding = encoding; }
        public BitReader(Stream s, Encoding encoding, bool leaveOpen) : base(s, encoding, leaveOpen) { LeaveOpen = leaveOpen; }



        byte readedByte, result = 0;
        int difference;
        /// <summary>
        /// Read 8 bits at max
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public byte Read8(int bits = 8) {
            if (bits == 0 || bits > 8) throw new ArgumentOutOfRangeException("Bit count cannot equal to 0 or be greater than 8");

            if (bitCount < bits)
            {
                readedByte = base.ReadByte();

                //Read to the empty buffer
                if (bitCount == 0)
                {
                    result = (byte)(readedByte >> (8 - bits));

                    byteBuffer = (byte)(readedByte << bits);
                    bitCount = (8 - bits);
                }
                //We want to coalesce remaining buffer bits with new bits from the file
                else
                {
                    difference = bits - bitCount;
                    //Move bufferized bits to the left place in byte 
                    result = (byte)(byteBuffer >> (8 - bits));
                    //Clean bits by shifting to the right and to the left
                    //Shift to the right to move the bits, then coalesce
                    result |= (byte)(readedByte >> (8 - difference));

                    //Clean readed bits from the buffer
                    byteBuffer = (byte)(readedByte << difference);
                    bitCount = 8 - difference;
                }
            }
            //Read from buffer, when there are remaining bits and
            //the number of bits less or equal to the number of bits in the buffer
            else
            {
                //Read the bits needed
                result = (byte)(byteBuffer >> (8 - bits));

                //Clean the buffer out of them
                byteBuffer = (byte)(byteBuffer << bits);
                bitCount = bitCount - bits;
            }


            return result;
        }

        public byte[] ReadN(int bits) {

            if (bits == 0) throw new ArgumentOutOfRangeException("Bit count cannot equal to 0");

            var mod = bits % 8;
            var div = bits / 8;
            var result = new byte[div + (mod == 0 ? 0 : 1)];

            for (int i = 0; i < div; i++)
            {
                result[i] = Read8(8);
            }
            if (mod != 0)
            {
                result[div] = Read8(mod);
            }

            return result;
        }

        #region ReadTypes
        public override bool ReadBoolean() => Convert.ToBoolean(Read8());
        public override byte ReadByte() => Read8();
        public override sbyte ReadSByte() => Convert.ToSByte(Read8());

        public override long ReadInt64() => BitConverter.ToInt64(ReadN(64), 0);
        public override int ReadInt32() => BitConverter.ToInt32(ReadN(32), 0);
        public override short ReadInt16() => BitConverter.ToInt16(ReadN(16), 0);

        public override ulong ReadUInt64() => BitConverter.ToUInt64(ReadN(64), 0);
        public override uint ReadUInt32() => BitConverter.ToUInt32(ReadN(32), 0);
        public override ushort ReadUInt16() => BitConverter.ToUInt16(ReadN(16), 0);

        public override byte[] ReadBytes(int count) => ReadN(count * 8);
        public long ReadInt64(int bits) {
            if (bits > 64) throw new ArgumentOutOfRangeException("Bit count cannot be larger than 64");

            return BitConverter.ToInt64(ReadN(bits), 0);
        }
        public int ReadInt32(int bits) {
            if (bits > 32) throw new ArgumentOutOfRangeException("Bit count cannot be larger than 32");

            return BitConverter.ToInt32(ReadN(bits), 0); 
        }
        public short ReadInt16(int bits) {
            if (bits > 16) throw new ArgumentOutOfRangeException("Bit count cannot be larger than 16");

            return BitConverter.ToInt16(ReadN(bits), 0);
        }
        public ulong ReadUInt64(int bits) {
            if (bits > 64) throw new ArgumentOutOfRangeException("Bit count cannot be larger than 64");

            return BitConverter.ToUInt64(ReadN(bits), 0);
        }
        public uint ReadUInt32(int bits) {
            if (bits > 32) throw new ArgumentOutOfRangeException("Bit count cannot be larger than 32");

            return BitConverter.ToUInt32(ReadN(bits), 0); 
        }
        public ushort ReadUInt16(int bits) {
            if (bits > 16) throw new ArgumentOutOfRangeException("Bit count cannot be larger than 16");

            return BitConverter.ToUInt16(ReadN(bits), 0);
        }

        #endregion

        public override int Read(byte[] buffer, int index, int count)
        {
            return base.Read(buffer, index, count);
        }

        public override int Read(char[] buffer, int index, int count)
        {
            return base.Read(buffer, index, count);
        }

    }
}
