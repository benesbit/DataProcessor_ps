using CsvHelper;
using CsvHelper.Configuration;
using System.IO.Abstractions;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace DataProcessor
{
    class CsvFileProcessor
    {
        private readonly IFileSystem _fileSystem;

        public string InputFilePath { get; }
        public string OutputFilPath { get; }

        public CsvFileProcessor(string inputFilePath, string outputFilePath)
            : this(inputFilePath, outputFilePath, new FileSystem()) { }

        public CsvFileProcessor(string inputFilePath, string outputFilePath, IFileSystem fileSystem)
        {
            InputFilePath = inputFilePath;
            OutputFilPath = outputFilePath;
            _fileSystem = fileSystem;
        }
        public void Process()
        {
            using (StreamReader input = _fileSystem.File.OpenText(InputFilePath))
            using (CsvReader csvReader = new CsvReader(input, CultureInfo.InvariantCulture))
            using (StreamWriter output = _fileSystem.File.CreateText(OutputFilPath))
            using (var csvWriter = new CsvWriter(output, CultureInfo.InvariantCulture))
            {
                csvReader.Configuration.TrimOptions = TrimOptions.Trim;
                csvReader.Configuration.Comment = '@'; // Default is '#'
                csvReader.Configuration.AllowComments = true;
                //csvReader.Configuration.IgnoreBlankLines = false; // Deafult is true
                //csvReader.Configuration.Delimiter = ';'; // Default is ','
                //csvReader.Configuration.HasHeaderRecord = false; // Default is true
                //csvReader.Configuration.HeaderValidated = null;
                //csvReader.Configuration.MissingFieldFound = null;
                csvReader.Configuration.RegisterClassMap<ProcessedOrderMap>();

                IEnumerable<ProcessedOrder> records = csvReader.GetRecords<ProcessedOrder>();

                //csvWriter.WriteRecords(records);

                csvWriter.WriteHeader<ProcessedOrder>();
                csvWriter.NextRecord();

                var recordsArray = records.ToArray();
                for (int i = 0; i < recordsArray.Length; ++i)
                {
                    csvWriter.WriteField(recordsArray[i].OrderNumber);
                    csvWriter.WriteField(recordsArray[i].Customer);
                    csvWriter.WriteField(recordsArray[i].Amount);

                    bool isLastRecord = i == recordsArray.Length - 1;

                    if (!isLastRecord)
                    {
                        csvWriter.NextRecord();
                    }
                }

                //foreach (ProcessedOrder record in records)
                //{
                //    Console.WriteLine(record.OrderNumber); // If no header, this should be Field1
                //    //Console.WriteLine(record.CustomerNumber); // If no header, this should be Field2, etc.
                //    //Console.WriteLine(record.Description);
                //    //Console.WriteLine(record.Quantity);
                //    Console.WriteLine(record.Customer);
                //    Console.WriteLine(record.Amount);
                //}
            }
        }
    }
}
