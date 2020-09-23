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
            // Using read all text
            //string originalText = File.ReadAllText(InputFilePath);
            //string proccesedText = originalText.ToUpperInvariant();
            //File.WriteAllText(OutputFilePath, proccesedText);

            // Using read all lines
            string[] lines = File.ReadAllLines(InputFilePath);
            lines[1] = lines[1].ToUpperInvariant(); // Assume at least 2 lines in the file
            File.WriteAllLines(OutputFilePath, lines);
        }
    }
}
