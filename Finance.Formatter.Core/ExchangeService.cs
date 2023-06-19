using Finance.Formatter.Models;
using Finance.Formatter.Models.Config;
using Microsoft.Extensions.Options;

namespace Finance.Formatter.Core
{
    public class ExchangeService
    {
        private readonly IOptions<AppConfig> _configuration;

        public ExchangeService(IOptions<AppConfig> configuration)
        {
            _configuration = configuration;
        }

        public decimal? ConvertToUSD(decimal? value, Currency currency)
        {
            if (value == 0)
            {
                return value;
            }

            switch (currency)
            {
                case Currency.UAH:
                    return value * (decimal)_configuration.Value.UahToUsdMultiplier;

                case Currency.EUR:
                    return value * (decimal)_configuration.Value.EurToUsdMultiplier;

                default:
                    throw new ArgumentException($"Cannot convert to specified '{currency}' currency.");
            }
        }
    }
}
