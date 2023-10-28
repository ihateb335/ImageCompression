using System;
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
        }
        [Test]
        public void TestInvalid() { 
            Assert.Throws<ArgumentOutOfRangeException>(() => writer.Write((byte)0, 0), "should throw error if trying to write 0 bits");
            Assert.Throws<ArgumentOutOfRangeException>(() => writer.Write((byte)0, 9), "should throw error if trying to write 9 bits");
            writer.Close();
        }

        #region Test write on 1 byte

        [Test]
        public void TestWrite3_5()
        {
            TestWritten(() => {
                writer.Write(160, 3);
                writer.Write(255, 5);
            }, new byte[] { 191 }, "Test write 3 bits, then 5");
        }
        [Test]
        public void TestWrite5_8()
        {
            TestWritten(() => {
                writer.Write(160, 5);
                writer.Write(255, 8);
            }, new byte[] { 167, 248 }, "Test write 3 bits, then 5");
        }
        [Test]
        public void TestWrite7_8()
        {
            TestWritten(() => {
                writer.Write(160, 7);
                writer.Write(255, 8);
            }, new byte[] { 161, 254 }, "Test write 3 bits, then 5");
        }
        [Test]
        public void TestWrite5_2_1()
        {
            TestWritten(() =>
            {
                writer.Write(160, 5);
                writer.Write(255, 2);
                writer.Write(255, 1);
            }, new byte[] { 167 }, "Test write 5 bits, then 2, then 1");
        }

        #endregion

        [Test]
        public void TestInvalidBufferArray()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => writer.Write(new byte[] { }, 8), "should throw error if trying to write to the empty array");
            Assert.Throws<ArgumentOutOfRangeException>(() => writer.Write(new byte[] { 1 }, 0), "should throw error if trying to write 0 bits");
            Assert.Throws<ArgumentOutOfRangeException>(() => writer.Write(new byte[] { 1 }, 9), "should throw error if trying to write 9 bits to the 1-element array");
            Assert.Throws<ArgumentOutOfRangeException>(() => writer.Write(new byte[] { 1, 1, 1 }, 25), "should throw error if trying to write 25 bits to the 3-element array");

            Assert.DoesNotThrow(() => writer.Write(new byte[] { 1, 1, 1 }, 24), "shouldn't throw error if trying to write 24 bits to the 3-element array");
            Assert.DoesNotThrow(() => writer.Write(new byte[] { 1, 1, 1 }, 12), "shouldn't throw error if trying to write 12 bits to the 3-element array");

        }

        #region Test write on array

        [Test]
        public void TestBufferArrayWrite()
        {
            TestWritten(() => {
                writer.Write(new byte[] { 255, 255, 255 }, 12);
            }, new byte[] { 255, 240 }, "Test write on array write 12");

            Reload();
            TestWritten(() => {
                writer.Write(new byte[] { 255, 255, 255 }, 4);
            }, new byte[] { 240 }, "Test write on array write 4");

            Reload();
            TestWritten(() => {
                writer.Write(new byte[] { 255, 255, 255 }, 16);
            }, new byte[] { 255, 255 }, "Test write on array write 16");
             
            Reload();
            TestWritten(() => {
                writer.Write(new byte[] { 255, 255, 255 }, 15);
            }, new byte[] { 255, 254 }, "Test write on array write 15");
            
            Reload();
            TestWritten(() => {
                writer.Write(new byte[] { 255, 255, 255 }, 1);
            }, new byte[] { 128 }, "Test write on array write 1");

        }

        #endregion
    }
}
