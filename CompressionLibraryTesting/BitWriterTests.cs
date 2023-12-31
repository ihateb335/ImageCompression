﻿using System;
using System.Linq;
using System.IO;
using NUnit.Framework;
using CompressionLibrary;
using CompressionLibrary.BitOperations;

namespace CompressionLibraryTesting
{
    public class BitWriterTests
    {
        MemoryStream memory;
        BitWriter writer;

        [SetUp]
        public void Setup()
        {
            Reload();
        }

        private void Reload()
        {
            memory = new MemoryStream();
            writer = new BitWriter(memory);
        }

        private void TestWritten(Action action, byte[] result, string message = "")
        {
            action();
            writer.Close();
            var bytes = memory.ToArray();

            Assert.AreEqual(result, bytes, message);
            Reload();
        }
        [Test]
        public void TestInvalid() { 
            Assert.Throws<ArgumentOutOfRangeException>(() => writer.Write8((byte)0, 0), "should throw error if trying to write 0 bits");
            Assert.Throws<ArgumentOutOfRangeException>(() => writer.Write8((byte)0, 9), "should throw error if trying to write 9 bits");
            writer.Close();
        }

        #region Test write on 1 byte

        [Test]
        public void TestWrite3_5()
        {
            TestWritten(() => {
                writer.Write8(13, 3);
                writer.Write8(255, 5);
            }, new byte[] { 191 }, "Test write 3 bits, then 5");
        }
        [Test]
        public void TestWrite5_8()
        {
            TestWritten(() => {
                writer.Write8(20, 5);
                writer.Write8(255, 8);
            }, new byte[] { 167, 248 }, "Test write 3 bits, then 5");
        }
        [Test]
        public void TestWrite7_8()
        {
            TestWritten(() => {
                writer.Write8(5, 7);
                writer.Write8(255, 8);
            }, new byte[] { 11, 254 }, "Test write 3 bits, then 5");
        }
        [Test]
        public void TestWrite5_2_1()
        {
            TestWritten(() =>
            {
                writer.Write8(20, 5);
                writer.Write8(255, 2);
                writer.Write8(255, 1);
            }, new byte[] { 167 }, "Test write 5 bits, then 2, then 1");
        }
        [Test]
        public void TestWrite8_1()
        {
            TestWritten(() => {
                writer.Write8(183, 8);
                writer.Write8(1, 1);
            }, new byte[] { 183, 128 }, "Test write 3 bits, then 5");
        }

        #endregion

        [Test]
        public void TestInvalidBufferArray()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => writer.WriteN(new byte[] { }, 8), "should throw error if trying to write to the empty array");
            Assert.Throws<ArgumentOutOfRangeException>(() => writer.WriteN(new byte[] { 1 }, 0), "should throw error if trying to write 0 bits");
            Assert.Throws<ArgumentOutOfRangeException>(() => writer.WriteN(new byte[] { 1 }, 9), "should throw error if trying to write 9 bits to the 1-element array");
            Assert.Throws<ArgumentOutOfRangeException>(() => writer.WriteN(new byte[] { 1, 1, 1 }, 25), "should throw error if trying to write 25 bits to the 3-element array");

            Assert.DoesNotThrow(() => writer.WriteN(new byte[] { 1, 1, 1 }, 24), "shouldn't throw error if trying to write 24 bits to the 3-element array");
            Assert.DoesNotThrow(() => writer.WriteN(new byte[] { 1, 1, 1 }, 12), "shouldn't throw error if trying to write 12 bits to the 3-element array");

        }

        #region Test write on array

        [Test]
        public void TestBufferArrayWrite()
        {
            TestWritten(() => {
                writer.WriteN(new byte[] { 255, 255, 255 }, 12);
            }, new byte[] { 255, 240 }, "Test write on array write 12");

           
            TestWritten(() => {
                writer.WriteN(new byte[] { 255, 255, 255 }, 4);
            }, new byte[] { 240 }, "Test write on array write 4");

            TestWritten(() => {
                writer.WriteN(new byte[] { 255, 255, 255 }, 16);
            }, new byte[] { 255, 255 }, "Test write on array write 16");
             
            TestWritten(() => {
                writer.WriteN(new byte[] { 255, 255, 255 }, 15);
            }, new byte[] { 255, 254 }, "Test write on array write 15");
            
            TestWritten(() => {
                writer.WriteN(new byte[] { 255, 255, 255 }, 1);
            }, new byte[] { 128 }, "Test write on array write 1");

        }
        [TestCase((ushort)325)]
        [TestCase((ushort)240)]
        [TestCase((ushort)439)]
        public void TestWriteUShort9(ushort value)
        {
            var bytes = BitConverter.GetBytes(value);
            TestWritten(() => writer.Write16(value, 9), new byte[] {bytes[0], (byte)(bytes[1] << 7) });
        }

        [Test]
        public void TestWriteBy9()
        {
            const int count = 9;
            var input = new ushort[]
                {
                    325,
                    240,
                    439,
                };
            TestWritten(() => {
                foreach (var item in input)
                {
                    writer.Write16(item, count);
                }
            }, new byte[]
            {
                69, 248, 45, 224
            }, $"Test write on array by {count}");
        }
         [Test]
        public void TestWriteBy11()
        {
            const int count = 11;
            var input = new ushort[]
                {
                    1024,
                    1025,
                    1026,
                    1027,
                    1028,
                };
            TestWritten(() => {
                foreach (var item in input)
                {
                    writer.Write16(item, count);
                }
            }, new byte[]
            {
                0, 128, 48, 10, 1, 192, 72
            }, $"Test write on array by {count}");
        }

        #endregion
    }
}
