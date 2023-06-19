using CsvHelper.Configuration.Attributes;

namespace Finance.Formatter.Models.Input
{
    public class MonobankFOPInputEntry
    {
        [Name("Date and time")]
        public string? DateTime { get; set; }
        [Name("Purpose of payment")]
        public string? PurposeOfPayment { get; set; }
        [Name("Counterparty")]
        public string? Counterparty { get; set; }
        [Name("ITN")]
        public string? ITN { get; set; }
        [Name("IBAN")]
        public string? IBAN { get; set; }
        [Name("Amaunt (NBU exchange rate equivalent)")]
        public string? AmountNBUExchangeRateEquivalent { get; set; }
        [Name("Operation amount")]
        public string? OperationAmount { get; set; }
        [Name("Сurrency")]
        public string? Currency { get; set; }
        [Name("Exchange rate")]
        public string? ExchangeRate { get; set; }
        [Name("Commission, (UAH)")]
        public string? Commission { get; set; }
        [Name("Balance")]
        public string? Balance { get; set; }
    }
}
