using Finance.Formatter.Models.Interfaces;

namespace Finance.Formatter.Models
{
    public class Metadata<T> where T : ITransaction
    {
        public bool IsMatch { get; set; }

        public string Keyword { get; set; }

        public string Category { get; set; }

        public decimal USDEquivalent { get; set; }

        public decimal USDEquivalentWithCommission { get; set; }

        public T Entry { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public bool IsFop { get; set; }

        public override string ToString()
        {
            return $"{Entry.Description} - {Category} - {USDEquivalent}";
        }
    }
}
