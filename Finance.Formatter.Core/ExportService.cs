using FastExcel;
using Finance.Formatter.Models;
using Finance.Formatter.Models.Config;
using Finance.Formatter.Models.Interfaces;
using Microsoft.Extensions.Options;
using Cell = FastExcel.Cell;

namespace Finance.Formatter.Core
{
    public class ExportService
    {
        const string TransactionsSheetName = "Transactions";

        private readonly IOptions<AppConfig> _configuration;

        public ExportService(IOptions<AppConfig> configuration)
        {
            _configuration = configuration;
        }

        public void ExportTransactions(IEnumerable<Metadata<ITransaction>> transactions, string filePath)
        {
            // Get your template and output file paths
            var templateFile = new FileInfo(_configuration.Value.TemplatePath);
            var outputFile = new FileInfo(filePath);

            File.Delete(outputFile.FullName);
            File.Copy(templateFile.FullName, outputFile.FullName);

            // Create an instance of FastExcel
            using (var fastExcel = new FastExcel.FastExcel(outputFile))
            {
                //Create a worksheet with some rows
                var worksheet = fastExcel.Read(TransactionsSheetName);
                var rows = new List<Row>();
                var rowNumber = 2;

                foreach (var transaction in transactions)
                {
                    var cells = GetTransactionCells(transaction.Entry).ToList();
                    var index = cells.Count + 1;

                    cells.Add(new Cell(index++, Convert.ToDouble(transaction.USDEquivalent)));
                    cells.Add(new Cell(index++, transaction.IsMatch));
                    cells.Add(new Cell(index++, transaction.IsFop));
                    cells.Add(new Cell(index++, transaction.Keyword));
                    cells.Add(new Cell(index++, transaction.Category));
                    cells.Add(new Cell(index++, transaction.FileName));
                    cells.Add(new Cell(index++, transaction.FilePath));

                    rows.Add(new Row(rowNumber, cells));
                    rowNumber++;
                }

                worksheet.Rows = worksheet.Rows.Concat(rows);

                // Write the data
                fastExcel.Update(worksheet, TransactionsSheetName);
            }
        }

        private IEnumerable<Cell> GetTransactionCells(ITransaction transaction)
        {
            var index = 1;
            yield return new Cell(index++, transaction.DateTime.ToString("dd/MM/yyyy HH:mm"));
            yield return new Cell(index++, transaction.Description);
            yield return new Cell(index++, Convert.ToDouble(transaction.OperationAmount));
            yield return new Cell(index++, transaction.Currency);
            yield return new Cell(index++, Convert.ToDouble(transaction.Commission));
            yield return new Cell(index++, Convert.ToDouble(transaction.ExchangeRate));
        }
    }
}
