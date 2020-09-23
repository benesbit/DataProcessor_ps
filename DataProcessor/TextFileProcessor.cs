using System.IO;

namespace DataProcessor
{
    class TextFileProcessor
    {
        public string InputFilePath { get; }
        public string OutputFilePath { get; }

        public TextFileProcessor(string inputFilePath, string outputFilePath)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
        }

        public void Process()
        {
            using (var inputFileStream = new FileStream(InputFilePath, FileMode.Open))
            using (var inputStremReader = new StreamReader(inputFileStream))
            using (var outputFileStream = new FileStream(OutputFilePath, FileMode.Create))
            using (var outputStreamWriter = new StreamWriter(outputFileStream))
            {
                while (!inputStremReader.EndOfStream)
                {
                    string line = inputStremReader.ReadLine();
                    string processedLine = line.ToUpperInvariant();
                    outputStreamWriter.WriteLine(processedLine);
                }
            }
        }
    }
}
