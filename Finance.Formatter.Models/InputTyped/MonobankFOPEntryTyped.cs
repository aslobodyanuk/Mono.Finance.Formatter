using Finance.Formatter.Models.Interfaces;

namespace Finance.Formatter.Models.Input
{
    public class MonobankFOPEntryTyped : ITransaction
    {
        public DateTime DateTime { get; set; }

        public string? PurposeOfPayment { get; set; }

        public string? Counterparty { get; set; }

        public string? ITN { get; set; }

        public string? IBAN { get; set; }

        public decimal? AmountNBUExchangeRateEquivalent { get; set; }

        public decimal? OperationAmount { get; set; }

        public string? Currency { get; set; }

        public decimal? ExchangeRate { get; set; }

        public decimal? Commission { get; set; }

        public decimal? Balance { get; set; }

        string? ITransaction.Description => PurposeOfPayment;
    }
}
