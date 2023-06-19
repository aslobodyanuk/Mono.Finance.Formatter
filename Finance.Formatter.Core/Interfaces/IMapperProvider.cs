using AutoMapper;

namespace Finance.Formatter.Core
{
    public interface IMapperProvider
    {
        IMapper GetMapper();
    }
}
