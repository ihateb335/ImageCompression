using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibrary.RLEI
{
    public class RLEICompressor : FileCompressor
    {
        public int N { get; private set; }
        public RLEICompressor(int N = 3) : base()
        {
            this.N = N;
        }

        protected override void Algorithm(FileStream InputFile, FileStream OutputFile)
        {
            //Bytes A
            RLEIByte current;
            bool different = false;

            var sequence = new List<RLEIByte>() { };

            while (true)
            {
                //Read byte
                current = InputFile.ReadRLEIByte(N);

                //If EOF exit
                if (current.EOF) break;

                //If the buffer is empty add byte to buffer
                if (sequence.Count == 0)
                {
                    sequence.Add(current);
                    continue;
                }

                //If the buffer contains 1 byte check what group is it
                if (sequence.Count == 1)
                {
                    different = !current.Equals(sequence.Last());
                    sequence.Add(current);
                    continue;
                }

                //If the buffer is full write run to the file
                if (sequence.Count == 128)
                {
                    //Write bytes to the file
                    OutputFile.WriteByte(sequence.Count.RLEFlaggedByte(different));
                    if (different) sequence.ForEach(x => OutputFile.WriteRLEIByte(x));
                    else OutputFile.WriteRLEIByte(sequence.First());
                    sequence.Clear();

                    //Add current to the sequence
                    sequence.Add(current);
                    continue;
                }

                //If the last byte of the buffer is equal to current byte and the group is non-homogenous, then write down the group, except for the last byte
                if (sequence.Last().Equals(current) && different)
                {
                    OutputFile.WriteByte((sequence.Count - 1).RLEFlaggedByte(different));
                    //Write all **different** bytes
                    foreach (var b in sequence.Take(sequence.Count - 1)) OutputFile.WriteRLEIByte(b);
                    different = false;
                    sequence = new List<RLEIByte> { current };

                }
                //If the last byte of the buffer is different from current byte and the group is homogenous, then write down the group and clear it
                else if (!sequence.Last().Equals(current) && !different)
                {
                    OutputFile.WriteByte(sequence.Count.RLEFlaggedByte(different));
                    OutputFile.WriteRLEIByte(sequence.First());
                    sequence.Clear();
                }

                sequence.Add(current);

            }
            //Write bytes to the file after file was read, specifically the remaining bytes in the run
            if (sequence.Count > 0)
            {
                //Write bytes to the file
                OutputFile.WriteByte(sequence.Count.RLEFlaggedByte(different));
                if (different) sequence.ForEach(x => OutputFile.WriteRLEIByte(x));
                else OutputFile.WriteRLEIByte(sequence.First());
                sequence.Clear();
            }
            //Write remaining bytes in the byte group, if there are any. 
            if (current.HasBytes) { 
                OutputFile.WriteByte(0.RLEFlaggedByte(false));
                OutputFile.WriteRLEIByte(current);
            }  
        }
    }
}
