using static System.Console;

namespace DataProcessor
{
    internal class FileProcessor
    {
        public string InputFilePath { get;  }

        public FileProcessor(string filePath)
        {
            this.filePath = filePath;
        }
    }
}