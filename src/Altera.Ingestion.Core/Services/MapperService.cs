using Altera.Ingestion.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Altera.Ingestion.Core.Services;

public class MapperService : IMapperService
{
    private readonly IServiceProvider _serviceProvider;

    public MapperService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public TDestination Map<TSource, TDestination>(TSource source)
    {
        var mapper = _serviceProvider.GetRequiredService<IMapper<TSource, TDestination>>();

        return mapper.Map(source);
    }
}
