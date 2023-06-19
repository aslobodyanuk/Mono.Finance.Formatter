using CsvHelper;
using Finance.Formatter.Models.Enums;
using System.Globalization;

namespace Finance.Formatter.Core
{
    public class CSVParserService
    {
        public IEnumerable<T> ParseFile<T>(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            using (var textReader = new StreamReader(stream))
            using (var csv = new CsvReader(textReader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<T>().ToList();
                return records;
            }
        }

        public ExportType DetermineType(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            using (var textReader = new StreamReader(stream))
            {
                var lines = Enumerable.Range(0, 5).Select(x => textReader.ReadLine());
                var isFopCard = lines.Any(x => x.Contains("ITN", StringComparison.InvariantCultureIgnoreCase) &&
                                                x.Contains("IBAN", StringComparison.InvariantCultureIgnoreCase) &&
                                                x.Contains("Counterparty", StringComparison.InvariantCultureIgnoreCase));

                if (isFopCard)
                {
                    return ExportType.FOP;
                }

                return ExportType.Card;
            }
        }
    }
}
