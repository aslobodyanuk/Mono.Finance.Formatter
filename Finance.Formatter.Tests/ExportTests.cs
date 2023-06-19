using Finance.Formatter.Core;

namespace Finance.Formatter.Tests
{
    [TestClass]
    public class ExportTests : TestBase
    {
        [AutoResolve] ExportService _exportService = null;
        [AutoResolve] ProcessingService _processingService = null;

        [TestMethod]
        public async Task ExportGenericFiles()
        {
            var exportFilePath = "C:\\temp\\output.xlsx";

            var files = new string[]
            {
                "C:\\Users\\andrii.slobodianiuk\\Desktop\\report_18-06-23_16-55-42.csv",
                "C:\\Users\\andrii.slobodianiuk\\Desktop\\report_18-06-23_16-56-06.csv"
            };

            CloseAllExcelInstances();

            var results = _processingService.ProcessData(files).OrderBy(x => x.Entry.DateTime).ToList();
            _exportService.ExportTransactions(results, exportFilePath);

            OpenExcelFile(exportFilePath);
        }

    }
}