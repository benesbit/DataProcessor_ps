using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataProcessor
{
    class BinaryFileProcessor
    {
        public string InputFilePath { get; }
        public string OutputFilePath { get; }

        public BinaryFileProcessor(string inputFilePath, string outputFilePath)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
        }

        public void Process()
        {
            using (FileStream input = File.Open(InputFilePath, FileMode.Open, FileAccess.Read))
            using (FileStream output = File.Create(OutputFilePath))
            {
                const int endOfStream = -1;

                int largestByte = 0;

                int currentByte = input.ReadByte();

                while (currentByte != endOfStream)
                {
                    output.WriteByte((byte)currentByte);

                    if (currentByte > largestByte)
                    {
                        largestByte = currentByte;
                    }

                    currentByte = input.ReadByte();
                }

                output.WriteByte((byte)largestByte);
            }
        }
    }
}
