using Finance.Formatter.Models.Interfaces;

namespace Finance.Formatter.Models.Input
{
    public class MonobankCardEntryTyped : ITransaction
    {
        public DateTime DateTime { get; set; }

        public string? Description { get; set; }

        public int? MCC { get; set; }

        public decimal? CardCurrencyAmount { get; set; }

        public decimal? OperationAmount { get; set; }

        public string? OperationCurrency { get; set; }

        public decimal? ExchangeRate { get; set; }

        public decimal? Commission { get; set; }

        public decimal? CashbackAmount { get; set; }

        public decimal? Balance { get; set; }

        string? ITransaction.Currency => OperationCurrency;
    }
}
