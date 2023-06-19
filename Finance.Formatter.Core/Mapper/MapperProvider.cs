using AutoMapper;
using Finance.Formatter.Models;
using Finance.Formatter.Models.Input;
using Finance.Formatter.Models.Interfaces;
using System.Globalization;

namespace Finance.Formatter.Core.Mapper
{
    public class MapperProvider : IMapperProvider
    {
        private readonly IMapper _mapper;

        public MapperProvider()
        {
            _mapper = Initialize();
        }

        public IMapper GetMapper() => _mapper;

        private IMapper Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<string, decimal?>().ConvertUsing(s => ParseDecimalSafe(s));
                cfg.CreateMap<string, string>().ConvertUsing(s => HandleEmptyStrings(s));

                cfg.CreateMap<MonobankFOPInputEntry, MonobankFOPEntryTyped>();
                cfg.CreateMap<MonobankCardInputEntry, MonobankCardEntryTyped>();
            });

            config.AssertConfigurationIsValid();
            return config.CreateMapper();
        }

        private string HandleEmptyStrings(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || 
                input.Equals(StaticResources.MONO_EMPTY_STRING, StringComparison.InvariantCultureIgnoreCase))
            {
                return string.Empty;
            }

            return input;
        }

        private decimal? ParseDecimalSafe(string input)
        {
            if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }

            return default;
        }
    }
}
