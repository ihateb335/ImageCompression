using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NUnit.Framework;
using CompressionLibrary;
using CompressionLibrary.BitOperations;

namespace CompressionLibraryTesting
{
    public class BitReaderTests
    {
        MemoryStream memory;
        BitReader reader;
        Random Random = new Random();
        bool closed;

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
            memory = new MemoryStream();
            reader = new BitReader(memory);
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
        private void TestRead(Func<byte[]> action, byte[] input, byte[] actual, string message = "")
        {
            memory.Write(input, 0, input.Length);
            memory.Seek(0, SeekOrigin.Begin);
            var expected = action();
            Close();

            Assert.AreEqual(actual, expected, message);
        }

        [Test(Description = "Test negative")]
        public void TestInvalid()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => reader.Read8(0), "should throw error if trying to write 0 bits");
            Assert.Throws<ArgumentOutOfRangeException>(() => reader.Read8(9), "should throw error if trying to write 9 bits");
        }

        [Test(Description = "Test negative")]
        public void TestBulkInvalid()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => reader.ReadN(0), "should throw error if trying to write 0 bits");
            Assert.DoesNotThrow(() => { 
                try {
                    reader.ReadN(255);
                } catch (EndOfStreamException e) {
                } 
            }, "should throw error if trying to write 9 bits");
        }

        #region Test Read


        [Test(Description = "Test read 8 bits (1)")]
        public void TestRead8()
        {
            TestRead(
                () => new byte[] { reader.Read8(8) },
                new byte[] { 255 },
                new byte[] { 255 },
                "Read full byte from stream"
                );
        }


        [Test(Description = "Test read less than byte (2)")]
        public void TestRead3()
        {
            TestRead(
                () => new byte[] { reader.Read8(3) },
                new byte[] { 255 },
                new byte[] { 7 },
                "Read 3 bits from stream"
                );
        }

        [Test(Description = "Test read less than byte, twice, but sum is 8 (2)")]
        public void TestRead3_5()
        {
            TestRead(
                () => new byte[] { reader.Read8(3), reader.Read8(5) },
                new byte[] { 255 },
                new byte[] { 7, 31 },
                "Read from stream 3 bits, then 5"
                );
        }

        [Test(Description = "Test read more than byte, but sum is 8 (2,3)")]
        public void TestRead3_8_3()
        {
            TestRead(
                () => new byte[] { reader.Read8(3), reader.Read8(8), reader.Read8(3) },
                new byte[] { 255, 255 },
                new byte[] { 7, 255, 7 },
                "Read from stream 3 bits, then 8, then 3"
                );
        }

        [Test(Description = "Test read less and more than, byte, but sum is 8 (1,2,3)")]
        public void TestRead3_4_8_1()
        {
            TestRead(
                () => new byte[] { reader.Read8(3), reader.Read8(4), reader.Read8(8), reader.Read8(1) },
                new byte[] { 255, 255 },
                new byte[] { 7, 15, 255, 1 },
                "Read from stream 3 bits, then 4, then 8 and 1"
            );
        }
        [Test(Description = "Test read more than byte, but less than 8 (2,3.1)")]
        public void TestRead4_7()
        {
            TestRead(
                () => new byte[] { reader.Read8(4), reader.Read8(7) },
                new byte[] { 255, 255 },
                new byte[] { 15, 127 },
                "Read from stream 4 bits, then 7"
            );
        }
        [Test(Description = "Test read byte, less and more than byte, but less than 8 and sum is 8 (1,2,3.1)")]
        public void TestRead8_4_7_5()
        {
            TestRead(
                () => new byte[] {
                    reader.Read8(8),
                    reader.Read8(4),
                    reader.Read8(7),
                    reader.Read8(5)
                },
                new byte[] { 255, 255, 255 },
                new byte[] { 255, 15, 127, 31 },
                "Read from stream 8, 4, 7, 5"
            );
        }
        [Test(Description = "Test read 7, 2 and 7 bit, but sum is 8(2,3.1)")]
        public void TestRead7_2_7()
        {
            TestRead(
                () => new byte[] {
                    reader.Read8(7),
                    reader.Read8(2),
                    reader.Read8(7),
                },
                new byte[] { 255, 255 },
                new byte[] { 127, 3, 127 },
                "Read from stream 7, 2, 7"
            );
        }
        #endregion

        #region Test bulk read

        [TestCase(8, ExpectedResult = 8)]
        [TestCase(128, ExpectedResult = 128)]
        [TestCase(int.MinValue, ExpectedResult = int.MinValue)]
        public int TestBulk(int input)
        {
            var inputArray = BitConverter.GetBytes(input);
            byte[] output = new byte[4];
            TestRead(() => { output = reader.ReadN(32); return output; }, inputArray, inputArray, "Read Write int");
            return BitConverter.ToInt32(output, 0);
        }

        [Test]
        public void TestBulk9_11_12()
        {
            int random = Random.Next(int.MinValue, int.MaxValue);
            var inputArray = BitConverter.GetBytes(random);
            var outputArray = new byte[] {
                inputArray[0], // 8
                (byte)(inputArray[1] >> 7), // 1
                (byte)((byte)(inputArray[1] << 1) | (byte)(inputArray[2] >> 7) ), // 8
                (byte)((byte)(inputArray[2] << 1) >> 5 ), // 3
                (byte)((byte)(inputArray[2] << 4) | (byte)(inputArray[3] >> 4 ) ), // 8
                (byte)((byte)(inputArray[3] << 4) >> 4 ), // 4
            
            };
            TestRead(() => reader.ReadN(9).Concat(reader.ReadN(11)).Concat(reader.ReadN(12)).ToArray(), inputArray, outputArray, "Read Write int");
        }
        [Test]
        public void TestBulkx100_9_11_12()
        {
            for (int i = 0; i <= 100; i++)
            {
                TestBulk9_11_12();
                Reload();
            }
        }

        [Test]
        public void TestBuffer()
        {
            var inputArray = new byte[17];
            Random.NextBytes(inputArray);

            var outputArray = new byte[18];
            outputArray[0] = (byte)(inputArray[0] >> 4);
            for (int i = 0; i < 16; i++)
            {
                outputArray[i + 1] = (byte)((byte)(inputArray[i] << 4) | (byte)(inputArray[i + 1] >> 4));
            }
            outputArray[17] = (byte)((byte)(inputArray[16] << 4) >> 4);

            TestRead(() =>
            {
                var result = new byte[18];
                result[0] = reader.Read8(4);
                for (int i = 1; i < 17; i++) result[i] = reader.Read8();
                result[17] = reader.Read8(4);
                return result;
            }, inputArray, outputArray, "Test bufferized read");
        }

        [TestCase(9, (ushort)32837, (ushort)325)]
        public void TestReadOutOf(int bits, ushort input, ushort actual)
        {
            TestRead(() => reader.ReadN(bits), BitConverter.GetBytes(input), BitConverter.GetBytes(actual), "Read 9 out of 10");
        }
        #endregion
    }
}
