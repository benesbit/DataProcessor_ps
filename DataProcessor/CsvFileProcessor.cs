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
                IEnumerable<Order> records = csvReader.GetRecords<Order>();

                csvReader.Configuration.TrimOptions = TrimOptions.Trim;
                csvReader.Configuration.Comment = '@'; // Default is '#'
                csvReader.Configuration.AllowComments = true;
                //csvReader.Configuration.IgnoreBlankLines = false; // Deafult is true
                //csvReader.Configuration.Delimiter = ';'; // Default is ','
                //csvReader.Configuration.HasHeaderRecord = false; // Default is true


                foreach (Order record in records)
                {
                    Console.WriteLine(record.OrderNumber); // If no header, this should be Field1
                    Console.WriteLine(record.CustomerNumber); // If no header, this should be Field2, etc.
                    Console.WriteLine(record.Description);
                    Console.WriteLine(record.Quantity);
                }
            }
        }
    }
}
