using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessor
{
    class CsvFileProcessor
    {
        public string InputFilePath { get; }
        public string OutputFilPath { get; }

        public CsvFileProcessor(string inputFilePath, string outputFilePath)
        {
            InputFilePath = inputFilePath;
            OutputFilPath = outputFilePath;
        }
        public void Process()
        {
            using (StreamReader input = File.OpenText(InputFilePath))
            using (CsvReader csvReader = new CsvReader(input, CultureInfo.InvariantCulture))
            {
                IEnumerable<dynamic> records = csvReader.GetRecords<dynamic>();

                csvReader.Configuration.TrimOptions = TrimOptions.Trim;
                csvReader.Configuration.Comment = '@'; // Default is '#'
                csvReader.Configuration.AllowComments = true;
                //csvReader.Configuration.IgnoreBlankLines = false;

                foreach (var record in records)
                {
                    Console.WriteLine(record.OrderNumber);
                    Console.WriteLine(record.CustomerNumber);
                    Console.WriteLine(record.Description);
                    Console.WriteLine(record.Quantity);
                }
            }
        }
    }
}
