using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using CompressionLibrary.BitOperations;
using System.Collections;

namespace CompressionLibraryTesting
{
    public class BitIntegration
    {
        MemoryStream Memory;
        BitReader Reader;
        BitWriter Writer;
        Random Random = new Random();
        bool closed;

        byte[] MemoryValues;
        ushort[] RawData;

        [SetUp]
        public void Setup()
        {
            Reload();
        }
        [TearDown]
        public void TearDown()
        {
            Close();
        }

        private void Reload()
        {
            closed = false;
            Memory = new MemoryStream();
            Reader = new BitReader(Memory);
            Writer = new BitWriter(Memory);
        }
        private void Close()
        {
            if (!closed)
            {
                closed = true;
                Close();
            }
        }

       

        /// <summary>
        /// Test read
        /// </summary>
        /// <param name="action">what is need to be performed</param>
        /// <param name="input">input bytes</param>
        /// <param name="actual">actual result</param>
        /// <param  name="message">(optional) message</param>
        private void TestRead(Func<byte[]> writeAction, Func<byte[]> readAction, string message = "")
        {
            var expected = writeAction();
            MemoryValues = Memory.ToArray();
            Memory.Seek(0, SeekOrigin.Begin);
            var actual = readAction();
            Close();

            Assert.AreEqual(expected, actual, message);
            Reload();
        }

        [Test]
        public void TestRead8()
        {
            TestRead(() => {
                var result = new byte[] { 255 };

                Writer.Write(result);

                return result;
            }, () =>
            {
                var result = new byte[1];

                result[0] = Reader.ReadByte();

                return result;
            }, "Read 1 byte");
        }
        
        [Test]
        public void TestRead7()
        {
            TestRead(() => {
                Writer.Write8(255, 7);
                Writer.Flush();

                return new byte[] { 127 };
            }, () =>
            {
                var result = new byte[1];

                result[0] = Reader.Read8(7);

                return result;
            }, "Read 1 byte");
        }
        
        [Test]
        public void TestRead9()
        {
            TestRead(() => {
                ushort value = 259;
                Writer.Write16(value, 9);
                Writer.Flush();

                var m = Memory.ToArray();

                return BitConverter.GetBytes(value);
            }, () =>
            {
                var m = Memory.ToArray();
                var result = Reader.ReadN(9);

                return result;
            }, "Read 1 byte");
        }

        #region Write sequence 9  

        private byte[] WriteSequence9(IEnumerable<ushort> toWrite)
        {
            IEnumerable<byte> bytes = new List<byte>();
            byte[] buffer; int length;
            foreach (var value in toWrite)
            {   
                buffer = BitConverter.GetBytes(value);
                length = Math.Max(Convert.ToString(value, 2).Length, 9);
                Writer.Write16(value, length);

                bytes = bytes.Concat(buffer);
            }
            

            Writer.Flush();
            RawData = toWrite.ToArray();

            return bytes.ToArray();
        }
        
        private byte[] ReadRandomSequence()
        {
            IEnumerable<byte> result = new List<byte>();
            var ushorts = new List<ushort>();

            int length = 9; ushort value;
            

            while (true)
            {
                if (Reader.BaseStream.Position == Reader.BaseStream.Length) break;
                value = Reader.ReadUInt16(length);
                length = Math.Max(Convert.ToString(value, 2).Length, 9);
                ushorts.Add(value);
                result = result.Concat(BitConverter.GetBytes(value));
            } ;

            return result.ToArray();
        }

        [Test]
        public void TestReadSequences()
        {
            TestRead(() => WriteSequence9(new ushort[]
            {
                325,
                240,
                439,
                211,
                303,
                367,
            }), () => ReadRandomSequence(), "Read sequence out of 6 values");
        }

        [Test]
        public void TestRandomSmallSeries()
        {
            for (int i = 0; i < 10; i++)
            {
                TestRead(() => WriteSequence9(new ushort[6].Select(x => (ushort)Random.Next(0, 512))
                ), () => ReadRandomSequence(), $"Read sequence out of 6 random values #{i + 1}");
            }
        }

        [Test]
        public void TestRandomLargeSeries()
        {
            TestRead(() => WriteSequence9(new ushort[25].Select(x => (ushort)Random.Next(0, 512))
            ), () => ReadRandomSequence(), "Read sequence out of 25 random values"); 
            TestRead(() => WriteSequence9(new ushort[50].Select(x => (ushort)Random.Next(0, 512))
            ), () => ReadRandomSequence(), "Read sequence out of 50 random values"); 
            TestRead(() => WriteSequence9(new ushort[100].Select(x => (ushort)Random.Next(0, 512))
            ), () => ReadRandomSequence(), "Read sequence out of 100 random values");
        }

        #endregion

        #region Write ascending sequence
        private byte[] WriteSequenceAscending(ushort start, ushort maxCode)
        {
            IEnumerable<byte> bytes = new List<byte>();
            byte[] buffer; int length;

            var result = new ushort[maxCode - start + 1];

            for (ushort value = start; value <= maxCode; value++)
            {
                buffer = BitConverter.GetBytes(value);
                length = Math.Max(Convert.ToString(value, 2).Length, 9);
                Writer.Write16(value, length);

                bytes = bytes.Concat(buffer);

                result[value - start] = value;
            }
           

            Writer.Flush();
            RawData = result;

            return bytes.ToArray();
        }

        private byte[] ReadSequenceAscending(ushort start, ushort maxCode)
        {
            IEnumerable<byte> result = new List<byte>();
            var ushorts = new List<ushort> { };

            int length = Math.Max(Convert.ToString(start + 1, 2).Length, 9);

            for (ushort value = start; value <= maxCode; value++)
            {
                value = Reader.ReadUInt16(length);
                ushorts.Add(value);

                length = Math.Max(Convert.ToString(ushorts.LastOrDefault() + 1, 2).Length, 9);

                result = result.Concat(BitConverter.GetBytes(value));
            }

            return result.ToArray();
        }
        [TestCase((ushort)0, (ushort)10)]
        [TestCase((ushort)0, (ushort)128)]
        [TestCase((ushort)120, (ushort)128)]
        [TestCase((ushort)510, (ushort)513)]
        [TestCase((ushort)0, (ushort)513)]
        [TestCase((ushort)510, (ushort)1026)]
        [TestCase((ushort)1024, (ushort)4096)]
        [TestCase((ushort)0, (ushort)4096)]
        public void TestSequential(ushort start, ushort max) {
            TestRead(() => WriteSequenceAscending(start, max), 
                () => ReadSequenceAscending(start, max), $"Read sequence, starting from {start}, ending at {max}");
        }

        #endregion
    }
}
