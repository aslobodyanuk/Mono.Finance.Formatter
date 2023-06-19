using AutoMapper;
using Finance.Formatter.Models;
using Finance.Formatter.Models.Config;
using Finance.Formatter.Models.Enums;
using Finance.Formatter.Models.Input;
using Finance.Formatter.Models.Interfaces;
using Microsoft.Extensions.Options;

namespace Finance.Formatter.Core
{
    public class ProcessingService
    {
        private readonly IMapper _mapper;
        private readonly IOptions<AppConfig> _configuration;
        private readonly ExchangeService _exchangeService;
        private readonly CSVParserService _parserService;

        public ProcessingService(
            IMapper mapper,
            IOptions<AppConfig> configuration,
            ExchangeService exchangeService,
            CSVParserService parserService)
        {
            _mapper = mapper;
            _configuration = configuration;
            _exchangeService = exchangeService;
            _parserService = parserService;
        }

        public IEnumerable<Metadata<ITransaction>> ProcessData(IEnumerable<string> filePaths)
        {
            return filePaths.Select(x => ProcessFile(x)).SelectMany(x => x);
        }

        public IEnumerable<Metadata<ITransaction>> ProcessFile(string filePath)
        {
            var fileType = _parserService.DetermineType(filePath);

            switch (fileType)
            {
                case ExportType.Card:
                    {
                        var parsed = _parserService.ParseFile<MonobankCardInputEntry>(filePath);
                        return ProcessCardData(filePath, parsed);
                    }

                case ExportType.FOP:
                    {
                        var parsed = _parserService.ParseFile<MonobankFOPInputEntry>(filePath);
                        return ProcessFopData(filePath, parsed);
                    }

                default:
                    throw new ArgumentException("Unknown export file type.");
            }
        }

        public IEnumerable<Metadata<ITransaction>> ProcessFopData(string filePath, IEnumerable<MonobankFOPInputEntry> input)
        {
            var mapped = _mapper.Map<IEnumerable<MonobankFOPEntryTyped>>(input);
            var withMetadata = mapped?.Select(x => RetrieveMetadata(x, Currency.USD, filePath, true)).ToList();
            return FilterOutIgnoredKeywords(withMetadata);
        }

        public IEnumerable<Metadata<ITransaction>> ProcessCardData(string filePath, IEnumerable<MonobankCardInputEntry> input)
        {
            var mapped = _mapper.Map<IEnumerable<MonobankCardEntryTyped>>(input);
            var withMetadata = mapped?.Select(x => RetrieveMetadata(x, Currency.UAH, filePath)).ToList();
            return FilterOutIgnoredKeywords(withMetadata);
        }

        private IEnumerable<Metadata<ITransaction>> FilterOutIgnoredKeywords(IEnumerable<Metadata<ITransaction>> input)
        {
            var ignoredKeywords = _configuration?.Value?.IgnoredKeywords ?? Enumerable.Empty<string>();
            
            return input.Where(x => ignoredKeywords.Any(k => x.Entry.Description.Contains(k)) == false);
        }

        private Metadata<ITransaction> RetrieveMetadata<T>(T input, Currency cardCurrency, string filePath, bool isFop = false) where T : ITransaction
        {
            var foundKeyword = FindKeyword(input);
            var cardCurrencyEquivalent = input?.OperationAmount;
            var currency = input?.Currency ?? cardCurrency.ToString();
            var commission = input?.Commission ?? 0;

            // FOP Exchange Rate seem to require division
            if (input?.ExchangeRate > 0 && cardCurrency == Currency.USD)
            {
                cardCurrencyEquivalent = input?.OperationAmount / input?.ExchangeRate;
            }

            // FOP Exchange Rate seem to require multiplication
            if (input?.ExchangeRate > 0 && cardCurrency == Currency.UAH)
            {
                cardCurrencyEquivalent = input?.OperationAmount * input?.ExchangeRate;
            }

            var cardCurrencyEquivalentWithComission = cardCurrencyEquivalent + commission;

            var usdEquivalent = cardCurrencyEquivalent;
            var usdEquivalentWithCommission = cardCurrencyEquivalentWithComission;

            if (cardCurrency != Currency.USD)
            {
                usdEquivalent = _exchangeService.ConvertToUSD(usdEquivalent, cardCurrency);
                usdEquivalentWithCommission = _exchangeService.ConvertToUSD(usdEquivalentWithCommission, cardCurrency);
            }

            return new Metadata<ITransaction>()
            {
                IsMatch = foundKeyword != default,
                Keyword = foundKeyword?.Keyword,
                Category = foundKeyword?.Category,
                USDEquivalent = usdEquivalent ?? 0,
                USDEquivalentWithCommission = usdEquivalentWithCommission ?? 0,
                Entry = input,
                FilePath = filePath,
                FileName = Path.GetFileNameWithoutExtension(filePath),
                IsFop = isFop
            };
        }

        private KeywordConfig FindKeyword(ITransaction input)
        {
            var description = input?.Description ?? string.Empty;
            return _configuration?.Value?.Keywords?.FirstOrDefault(x => description.Contains(x.Keyword, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
