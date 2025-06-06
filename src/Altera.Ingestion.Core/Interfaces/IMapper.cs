﻿namespace Altera.Ingestion.Core.Interfaces;

public interface IMapper<TSource, TDestination>
{
    TDestination Map(TSource source);
}
