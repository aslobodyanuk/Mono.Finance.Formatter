namespace Finance.Formatter.Models.Interfaces
{
    public interface ITransaction
    {
        public DateTime DateTime { get; }

        public string? Description { get; }

        public decimal? OperationAmount { get; }

        public string? Currency { get; }

        public decimal? ExchangeRate { get; }

        public decimal? Commission { get; }
    }
}
