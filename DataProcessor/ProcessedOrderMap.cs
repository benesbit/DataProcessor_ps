﻿using CsvHelper.Configuration;
using System.Globalization;

namespace DataProcessor
{
    class ProcessedOrderMap : ClassMap<ProcessedOrder>
    {
        public ProcessedOrderMap()
        {
            AutoMap(CultureInfo.InvariantCulture);

            Map(m => m.Customer).Name("CustomerNumber");
            Map(m => m.Amount).Name("Quantity").TypeConverter<RomanTypeConverter>();
        }
    }
}
