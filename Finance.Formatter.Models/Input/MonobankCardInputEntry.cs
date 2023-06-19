using CsvHelper.Configuration.Attributes;

namespace Finance.Formatter.Models.Input
{
    public class MonobankCardInputEntry
    {
        [Name("Date and time")]
        public string? DateTime { get; set; }

        [Name("Description")]
        public string? Description { get; set; }

        [Name("MCC")]
        public string? MCC { get; set; }

        [Name("Card currency amount, (UAH)")]
        public string? CardCurrencyAmount { get; set; }

        [Name("Operation amount")]
        public string? OperationAmount { get; set; }

        [Name("Operation currency")]
        public string? OperationCurrency { get; set; }

        [Name("Exchange rate")]
        public string? ExchangeRate { get; set; }

        [Name("Commission, (UAH)")]
        public string? Commission { get; set; }

        [Name("Cashback amount, (UAH)")]
        public string? CashbackAmount { get; set; }

        [Name("Balance")]
        public string? Balance { get; set; }
    }
}
