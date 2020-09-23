using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        }
    }
}
