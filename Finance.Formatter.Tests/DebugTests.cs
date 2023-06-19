using Finance.Formatter.Core;
using Finance.Formatter.Models.Input;
using System.Text.Json;

namespace Finance.Formatter.Tests
{
    [TestClass]
    public class DebugTests : TestBase
    {
        [AutoResolve] CSVParserService _csvParserService = null;
        [AutoResolve] ProcessingService _processingService = null;

        [TestMethod]
        public async Task ParseGenericFile()
        {
            var files = new string[]
            {
                "C:\\Users\\andrii.slobodianiuk\\Desktop\\report_09-06-23_15-17-33.csv",
                "C:\\Users\\andrii.slobodianiuk\\Desktop\\report_09-06-23_15-17-12.csv"
            };

            var results = _processingService.ProcessData(files).OrderBy(x => x.Entry.DateTime).ToList();
        }

        [TestMethod]
        public async Task ParseFopFile()
        {
            var filePath = "C:\\Users\\andrii.slobodianiuk\\Desktop\\report_09-06-23_15-17-33.csv";
            var parsed = _csvParserService.ParseFile<MonobankFOPInputEntry>(filePath);

            var result = _processingService.ProcessFopData(filePath, parsed);
            var json = JsonSerializer.Serialize(result);
        }

        [TestMethod]
        public async Task ParseCardFile()
        {
            var filePath = "C:\\Users\\andrii.slobodianiuk\\Desktop\\report_09-06-23_15-17-12.csv";
            var parsed = _csvParserService.ParseFile<MonobankCardInputEntry>(filePath);

            var result = _processingService.ProcessCardData(filePath, parsed);
            var json = JsonSerializer.Serialize(result);
        }
    }
}