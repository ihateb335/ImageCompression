using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionLibrary.RLE
{
    
    public class RLECompressor : FileCompressor
    {
        protected override void Algorithm(FileStream InputFile, FileStream OutputFile)
        {
            //Bytes A
            int current = 0;
            //EOF flag
            bool eof = false;
            bool different = false;

            var sequence = new List<byte>() {};

            while (true)
            {
                //Read byte
                current = InputFile.ReadByte();

                eof = current < 0;
                //If EOF exit
                if (eof) break;

                if (sequence.Count == 0)
                {
                    sequence.Add((byte)current);
                    continue;
                }


                if (sequence.Count == 1)
                {
                    different = current != sequence.Last();
                    sequence.Add((byte)current);
                    continue;
                }
                

                if(sequence.Count == 128)
                {
                    //Write bytes to the file
                    OutputFile.WriteByte(sequence.Count.RLEFlaggedByte(different));
                    if (different) sequence.ForEach(x => OutputFile.WriteByte(x));
                    else OutputFile.WriteByte(sequence.First());
                    sequence.Clear();

                    //Add current to the sequence
                    sequence.Add((byte)current);
                    continue;
                }

                if(sequence.Last() == current && different)
                {
                    OutputFile.WriteByte((sequence.Count - 1).RLEFlaggedByte(different));
                    //Write all **different** bytes
                    foreach (var b in sequence.Take(sequence.Count - 1)) OutputFile.WriteByte(b);
                    different = false;
                    sequence = new List<byte> { (byte)current };
 
                } else if (sequence.Last() != current && !different){
                    OutputFile.WriteByte(sequence.Count.RLEFlaggedByte(different));
                    OutputFile.WriteByte(sequence.First());
                    sequence.Clear();
                }

                sequence.Add((byte)current);

            }

            //Write bytes to the file
            OutputFile.WriteByte(sequence.Count.RLEFlaggedByte(different));
            if (different) sequence.ForEach(x => OutputFile.WriteByte(x));
            else OutputFile.WriteByte(sequence.First());
            sequence.Clear();

        }
    }
}
